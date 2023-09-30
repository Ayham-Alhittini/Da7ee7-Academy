using AutoMapper;
using Da7ee7_Academy.Data;
using Da7ee7_Academy.DTOs;
using Da7ee7_Academy.Entities;
using Da7ee7_Academy.Extensions;
using Da7ee7_Academy.Interfaces;
using Da7ee7_Academy.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Da7ee7_Academy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        private readonly IWebHostEnvironment _env;
        public AdminController(DataContext context, IMapper mapper, IPhotoRepository photoRepository, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _photoRepository = photoRepository;
            _env = env;

        }

        [HttpGet("get-courses")]
        public ActionResult GetCourses()
        {
            var result = _context.Courses
                .Select(course => new
                {
                    Id = course.Id,
                    CourseCover = course.CoursePhotoUrl,
                    Subject = course.Subject,
                    Major = course.Major,
                    Teacher = course.Teacher.AppUser.FullName,
                    NumberOfStudents =course.EnrolledStudents.Count(),
                    TotalCourseTime = course.Sections.Sum(section => section.TotalSectionTime)
                }).ToList();

            return Ok(result);
        }

        [HttpGet("get-teachers")]///difference between this endpoint and other "get-teacher" endpoint that this one is only for admin, and it contains more details about the teachers
        public ActionResult GetTeachers()
        {
            var result = _context.Teachers
                .Select(t => new
                {
                    t.Id,
                    t.AppUser.UserPhotoUrl,
                    t.AppUser.FullName,
                    t.Major,
                    t.Gender,
                    t.AppUser.Email,
                    t.AppUser.PhoneNumber,
                    CoursesCount = t.Courses.Count,
                    TotalTeachedStudents = t.TeachedStudents.Count(),
                })
                .ToList();
            return Ok(result);
        }


        [HttpPost("add-sale-point")]
        public ActionResult AddSalePoint(AddSalePointDto salePointDto)
        {
            var salePoint = _mapper.Map<Entities.SalePoint>(salePointDto);
            _context.SalePoints.Add(salePoint);

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("delete-sale-point/{id}")]
        public ActionResult DeleteSalePoint(int id)
        {
            var salePoint = _context.SalePoints.Find(id);

            if (salePoint == null) 
            { 
                return NotFound("Sale point not found");
            }

            _context.SalePoints.Remove(salePoint);
            _context.SaveChanges();

            return Ok();
        }


        [HttpPost("add-blog")]
        public ActionResult AddBlog([FromForm] AddBlogDto NewBlogDto)
        {
            var blog = new Blog
            {
                Title = NewBlogDto.Title,
                Content = NewBlogDto.Content,
            };

            var file = _photoRepository.SavePhoto(NewBlogDto.BlogPhoto, @"wwwroot\Uploads\Blogs_Picture");

            blog.AppFileId = file.Id;

            blog.PhotoUrl = _env.GetUrlRoot() + Url.Action("GetImages", "Files", new { photoId = file.Id });

            _context.Blogs.Add(blog);

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("delete-blog/{id}")]
        public ActionResult DeleteBlog(int id)
        {
            var blog = _context.Blogs
                .Include(b => b.AppFile)
                .FirstOrDefault(b => b.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            _photoRepository.DeletePhoto(blog.AppFile);

            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("generate-cards/{count}")]
        public ActionResult GenerateCards(int count)
        {
            if (count <= 0)
            {
                return BadRequest("Enter a valid number");
            }

            var cards = new List<CourseCard>();

            for (int i = 0; i < count; ++i)
            {
                var card = new CourseCard();
                cards.Add(card);
            }

            _context.Cards.AddRange(cards);

            _context.SaveChanges();

            return Ok(cards.Select(c => c.CardNumber));
        }

    }
}
