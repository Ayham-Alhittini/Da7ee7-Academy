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
        public AccountController(UserManager<AppUser> userManager, IEmailService emailService, ITokenService tokenService,
            IWebHostEnvironment env, DataContext context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _env = env;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("teacher-register")]
        public async Task<ResponseModel> TeacherRegister(TeacherRegisterDto teacherRegister)
        {
            var response = new ResponseModel();

            if (await UserExist(teacherRegister.FullName, teacherRegister.Email, teacherRegister.PhoneNumber))
            {
                response.StatusCode = 400;
                response.Errors.Add("Teacher exist");
                return response;
            }

            var FullNameParts = teacherRegister.FullName.Split(' ');

            if (FullNameParts.Length < 4)
            {
                response.StatusCode = 400;
                response.Errors.Add("Full name at least have 4 name");
                return response;
            }

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
                response.StatusCode = 400;
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, RolesSrc.Teacher);
            if (!roleResult.Succeeded)
            {
                response.StatusCode = 400;
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            ///add the teacher entity
            var teacher = new Teacher
            {
                Id = user.Id,
                Major = teacherRegister.Major,
                Gender = teacherRegister.Gender,
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();


            var message = new EmailMessage(new string[] { user.Email! }, "Account created",
                $"The admin have been create your account as a teacher in Da7ee7 Academy \nEmail: {user.Email}\nPassword: {generatedPassword}");///need to be edited to take us to front end page
            _emailService.SendEmail(message);

            response.IsSuccess = true;
            response.Message = "Teacher added successfuly";
            return response;
        }

        [AllowAnonymous]
        [HttpPost("student-register")]
        public async Task<ResponseModel> StudentRegister([FromForm]StudentRegisterDto registerStudent)
        {
            var response = new ResponseModel();

            if (await UserExist(registerStudent.FullName, registerStudent.Email, registerStudent.PhoneNumber))
            {
                response.StatusCode = 400;
                response.Errors.Add("Student exist");
                return response;
            }

            var FullNameParts = registerStudent.FullName.Split(' ');

            if (FullNameParts.Length < 4)
            {
                response.StatusCode = 400;
                response.Errors.Add("Full name at least have 4 name");
                return response;
            }

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

            ///store the photo if any
            var photo = registerStudent.UserImage;
            if (CheckPhotoSended(photo))
            {
                var file = await SavePhoto(photo);
                user.FileId = file.Id;
                user.UserPhotoUrl = GetUrlRoot() + Url.Action("GetImages", "Files", new { photoId = file.Id });
            }

            var result = await _userManager.CreateAsync(user, registerStudent.Password);

            if (!result.Succeeded)
            {
                response.StatusCode = 400;
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, RolesSrc.Student);
            if (!roleResult.Succeeded)
            {
                response.StatusCode = 400;
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            ///add the student entity as well
            var student = new Student
            {
                Id = user.Id,
                Major = registerStudent.Major,
                Gender = registerStudent.Gender,
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();


            ///send email to confirm the email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email });
            var message = new EmailMessage(new string[] { user.Email! }, "Confirmation email link", GetUrlRoot() + confirmationLink!);///need to be edited to take us to front end page
            _emailService.SendEmail(message);

            response.IsSuccess = true;
            response.Message = "An email have been send to your account check you email";
            return response;
        }

        [Authorize]
        [HttpPost("change-photo")]
        public async Task<ResponseModel> ChangePhoto([Required] IFormFile photo)
        {
            var response = new ResponseModel();
            if (!CheckPhotoSended(photo))
            {
                response.StatusCode = 400;
                response.Errors.Add("only image allowed");

                return response;
            }

            var user = await _context.Users.Include(u => u.File)
                .FirstOrDefaultAsync(u => u.Id == User.GetUserId());

            var file = await SavePhoto(photo);

            var oldFile = user.File;

            user.FileId = file.Id;
            user.UserPhotoUrl = GetUrlRoot() + Url.Action("GetUserPicture", "Files", new { photoId = file.Id });

            if (oldFile != null) 
            {
                DeletePhoto(oldFile);
            }

            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Photo changed successfuly";

            return response;
        }

        [Authorize]
        [HttpPost("update-phone-number")]
        public async Task<ResponseModel> UpdatePhoneNumber([Required]string phoneNumber)
        {
            var response = new ResponseModel();
            var user = await _context.Users.FindAsync(User.GetUserId());
            
            if (await UserExist(user.FullName, user.Email, phoneNumber))
            {
                response.StatusCode = 400;
                response.Errors.Add("Phone number is exist");
                return response;
            }

            user.PhoneNumber = phoneNumber;
            await _context.SaveChangesAsync();


            response.IsSuccess = true;
            response.Message = "Phone number updated successfuly";
            return response;
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
        public async Task<ResponseModel<UserDto>> Login(LoginDto loginDto)
        {
            var response = new ResponseModel<UserDto>();
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                response.StatusCode = 401;
                response.Errors.Add("Invalid email");
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.StatusCode = 401;
                response.Errors.Add("You must confirm you email to log in");
                return response;
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                response.StatusCode = 401;
                response.Errors.Add("Invalid password");
                return response;
            }

            UserDto userDto = new UserDto
            {
                UserId = user.Id,
                Token = await _tokenService.CreateToken(user)
            };

            response.IsSuccess = true;
            response.Message = "Login successfuly";
            response.Result = userDto;

            return response;
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

            var forgotPasswordLink = GetUrlRoot() + "/reset-password?token=" + HttpUtility.UrlEncode(token) + "&email=" + email;///take us to the front end page

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
            var user = await _userManager.FindByEmailAsync(changePassword.Email);
            if (user == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("User not found");
                return response;
            }
            if (changePassword.OldPassword == changePassword.NewPassword)
            {
                response.StatusCode = 400;
                response.Errors.Add("New password must be different than old password");
                return response;
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                response.StatusCode = 400;
                response.Errors.AddRange(changePasswordResult.Errors.Select(e => e.Description));
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Password changed successfuly";
            return response;
        }

        //Private Methods
        private void DeletePhoto(AppFile file)
        {
            _context.Files.Remove(file);///Remove from database

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);
            System.IO.File.Delete(filePath);//Remove from folder
        }
        private async Task<AppFile> SavePhoto(IFormFile photo)
        {
            var file = new AppFile
            {
                ContentType = photo.ContentType,
                Path = @"Uploads\Users_Picture"
            };

            var extension = photo.FileName.Split('.').LastOrDefault();

            file.FileName = file.Id + "." + extension;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            _context.Files.Add(file);

            return file;
        }
        private bool CheckPhotoSended(IFormFile image)
        {
            return image != null && image.Length > 0 && image.ContentType.Contains("image");
        }
        private async Task<bool> UserExist(string fullName, string email, string phoneNumber)
        {
            return await _userManager.Users.AnyAsync(user => user.FullName == fullName 
                                                          || user.Email == email
                                                          || user.PhoneNumber == phoneNumber);
        }
        private string GetUrlRoot()
        {
            return _env.IsDevelopment() ? "https://localhost:7124" : "production url";
        }
    }
}
