using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Da7ee7_Academy.Interfaces;
using Da7ee7_Academy.Entities;
using Da7ee7_Academy.DTOs;
using Da7ee7_Academy.Helper;
using System.Web;
using Da7ee7_Academy.Data;
using Da7ee7_Academy.Extensions;

namespace Da7ee7_Academy.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;
        private readonly IPhotoRepository _photoRepository;
        public AccountController(UserManager<AppUser> userManager, IEmailService emailService, ITokenService tokenService,
            IPhotoRepository photoRepository,
            IWebHostEnvironment env, DataContext context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _env = env;
            _context = context;
            _photoRepository = photoRepository;
        }

        [Authorize]
        [HttpGet("who-am-i")]
        public ActionResult WhoAmI()
        {
            return Ok(_context.Users.Find(User.GetUserId()).FirstName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("teacher-register")]
        public async Task<ActionResult> TeacherRegister([FromForm]TeacherRegisterDto teacherRegister)
        {

            if (await UserExist(teacherRegister.FullName, teacherRegister.Email, teacherRegister.PhoneNumber))
            {
                return BadRequest("Teacher exist");
            }

            if (!_photoRepository.CheckPhotoSended(teacherRegister.ImageFile))
            {
                return BadRequest("Only image allowed");
            }

            var FullNameParts = teacherRegister.FullName.Split(' ');


            ///adding user
            var user = new AppUser
            {
                Email = teacherRegister.Email,
                UserName = teacherRegister.Email,
                FullName = teacherRegister.FullName,
                Role = RolesSrc.Teacher,
                PhoneNumber = teacherRegister.PhoneNumber,
                FirstName = FullNameParts.First(),
                EmailConfirmed = true
            };

            var generatedPassword = PasswordGenerator.Generate();

            var result = await _userManager.CreateAsync(user, generatedPassword);

            if (!result.Succeeded)
            {
                var errors = new List<string>();
                errors.AddRange(result.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, RolesSrc.Teacher);
            if (!roleResult.Succeeded)
            {
                var errors = new List<string>();
                errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            ///add the teacher entity
            var teacher = new Teacher
            {
                Id = user.Id,
                Major = teacherRegister.Major,
                Gender = teacherRegister.Gender,
            };

            _context.Teachers.Add(teacher);

            ///add the user photo

            var file = await _photoRepository.SavePhotoAsync(teacherRegister.ImageFile, @"Uploads\Users_Picture");

            user.FileId = file.Id;
            user.UserPhotoUrl = _env.GetUrlRoot() + Url.Action("GetImages", "Files", new { photoId = file.Id });

            //save changes
            await _context.SaveChangesAsync();


            var message = new EmailMessage(new string[] { user.Email! }, "Account created",
                $"The admin have been create your account as a teacher in Da7ee7 Academy \nEmail: {user.Email}\nPassword: {generatedPassword}");///need to be edited to take us to front end page
            _emailService.SendEmail(message);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("student-register")]
        public async Task<ActionResult> StudentRegister(StudentRegisterDto registerStudent)
        {

            if (await UserExist(registerStudent.FullName, registerStudent.Email, registerStudent.PhoneNumber))
            {
                return BadRequest("Student exist");
            }

            var FullNameParts = registerStudent.FullName.Split(' ');

            ///adding user
            var user = new AppUser
            {
                Email = registerStudent.Email,
                UserName = registerStudent.Email,
                FullName = registerStudent.FullName,
                Role = RolesSrc.Student,
                PhoneNumber = registerStudent.PhoneNumber,
                FirstName = FullNameParts.First(),
            };

            var result = await _userManager.CreateAsync(user, registerStudent.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, RolesSrc.Student);
            if (!roleResult.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            ///add the student entity as well
            var student = new Student
            {
                Id = user.Id,
                Gender = "ذكر",
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();


            ///send email to confirm the email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email });
            var message = new EmailMessage(new string[] { user.Email! }, "Confirmation email link", _env.GetUrlRoot() + confirmationLink!);///need to be edited to take us to front end page
            _emailService.SendEmail(message);

            return Ok();
        }

        [Authorize]
        [HttpPost("change-photo")]
        public async Task<ResponseModel> ChangePhoto([Required] IFormFile photo)
        {
            var response = new ResponseModel();
            if (!_photoRepository.CheckPhotoSended(photo))
            {
                response.StatusCode = 400;
                response.Errors.Add("only image allowed");

                return response;
            }

            var user = await _context.Users.Include(u => u.File)
                .FirstOrDefaultAsync(u => u.Id == User.GetUserId());

            var file = await _photoRepository.SavePhotoAsync(photo, @"Uploads\Users_Picture");

            var oldFile = user.File;

            user.FileId = file.Id;
            user.UserPhotoUrl = _env.GetUrlRoot() + Url.Action("GetImages", "Files", new { photoId = file.Id });

            if (oldFile != null) 
            {
                _photoRepository.DeletePhoto(oldFile);
            }

            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Photo changed successfuly";

            return response;
        }

        [Authorize(Roles ="Student")]
        [HttpGet("get-profile")]
        public async Task<ActionResult> GetProfile()
        {
            var student = await _context.Students
            .Include(u => u.AppUser)
            .FirstOrDefaultAsync(u => u.Id == User.GetUserId());
            var userDto = new UserDto
            {
                Email = student.AppUser.Email,
                FirstName = student.AppUser.FirstName,
                FullName = student.AppUser.FullName,
                Gender = student.Gender,
                PhoneNumber = student.AppUser.PhoneNumber,
                Role = student.AppUser.Role,
                UserId = student.Id,
            };
            return Ok(userDto);
        }

        [Authorize]
        [HttpPost("update-profile")]
        public async Task<ActionResult> UpdatePhoneNumber(ChangeProfileDto profile)
        {
            if (!User.IsInRole("Student"))
            {
                return Forbid();
            }

            var user = await _context.Users.FindAsync(User.GetUserId());

            user.PhoneNumber = profile.PhoneNumber;

            var student = await _context.Students.FindAsync(User.GetUserId());

            student.Gender = profile.Gender;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<ResponseModel> ConfirmEmail(string token, string email)
        {
            var response = new ResponseModel();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("User not found");
                return response;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                response.StatusCode = 400;
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Email confirmed successfuly";

            return response;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(user => user.Email == loginDto.LoginProvider || user.PhoneNumber == loginDto.LoginProvider);

            if (user == null)
            {
                return Unauthorized("Invalid login provider");
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized("You must confirm you email to log in");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized("Invalid password");
            }

            string gender = null;
            if (user.Role != "Admin")
            {
                if (user.Role == "Student") {
                    var student = await _context.Students.FindAsync(user.Id);
                    gender = student.Gender;
                } else {
                    var teacher = await _context.Teachers.FindAsync(user.Id);
                    gender = teacher.Gender;
                }
            }

            UserDto userDto = new UserDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                Token = await _tokenService.CreateToken(user),
                Role = user.Role,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = gender,
            };

            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ResponseModel> ForgotPassword([Required] string email)
        {
            var response = new ResponseModel();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("User not found");
                return response;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var forgotPasswordLink = _env.GetUrlRoot() + "/reset-password?token=" + HttpUtility.UrlEncode(token) + "&email=" + email;///take us to the front end page

            var message = new EmailMessage(new string[] { user.Email! }, "Password Reset link", forgotPasswordLink);
            _emailService.SendEmail(message);

            response.IsSuccess = true;
            response.Message = "An email have been send to your email";
            return response;
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ResponseModel> ResetPassword(ResetPassword resetPassword)
        {
            var response = new ResponseModel();
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("User not found");
                return response;
            }
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPasswordResult.Succeeded)
            {
                response.StatusCode = 400;
                response.Errors.AddRange(resetPasswordResult.Errors.Select(e => e.Description));
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Password changed successfuly";
            return response;
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ResponseModel> ChangePassword(ChangePasswordDto changePassword)
        {
            var response = new ResponseModel();
            var user = await _userManager.FindByIdAsync(User.GetUserId());

            
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                response.StatusCode = 400;
                
                if (changePasswordResult.Errors.Any(e => e.Code == "PasswordMismatch"))
                {
                    response.Errors.Add("كلمة المرور غير صحيحة");
                }
                else
                {
                    response.Errors.Add("كلمة المرور الجديدة ضعيفة");
                }
                
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Password changed successfuly";
            return response;
        }

        //Private Methods
        private async Task<bool> UserExist(string fullName, string email, string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(user => user.FullName == fullName 
                                                          || user.Email == email
                                                          || user.PhoneNumber == phoneNumber);
        }
    }
}
