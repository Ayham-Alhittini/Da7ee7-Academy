using AutoMapper;
using AutoMapper.QueryableExtensions;
using Da7ee7_Academy.Data;
using Da7ee7_Academy.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Da7ee7_Academy.Controllers
{
    public class UserController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("test-cards")]
        public async Task<ActionResult> TestCards()
        {
            var cards = await _context.Cards.Where(c => c.StudentId == null)
                .Select(c => c.CardNumber)
                .ToListAsync();
            return Ok(cards);
        }


        [HttpGet("get-courses/{major}")]
        public async Task<ActionResult> GetCourses(string major)
        {
            var courses = await _context.Courses
                .Where(course => course.Major == major)
                .ToListAsync();

            return Ok(_mapper.Map<CourseDto[]>(courses));
        }

        [HttpGet("get-teachers")]
        public async Task<ActionResult> GetTeachers()
        {
            var result = await _context.Teachers
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("get-teacher/{id}")]
        public async Task<ActionResult> GetTeacher(string id)
        {
            var teacher = await _context.Teachers
                .Where(t => t.Id == id)
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return Ok(teacher);
        }

        [HttpGet("get-teacher-profile/{teacherId}")]
        public async Task<ActionResult> GetTeacherProfile(string teacherId)
        {
            var teacher = await _context.Teachers
                .Where(teacher => teacher.Id == teacherId)
                .Include(teacher => teacher.AppUser)
                .Include(teacher => teacher.Courses)
                .FirstOrDefaultAsync();

            if (teacher == null)
            {
                return NotFound("Teacher not exist");
            }

            var teacherProfile = new TeacherProfileDto
            {
                Teacher = _mapper.Map<TeacherDto>(teacher),
                Courses = _mapper.Map<CourseDto[]>(teacher.Courses)
            };

            return Ok(teacherProfile);
        }

        [HttpGet("get-sale-points")]
        public async Task<ActionResult> GetSalePoints()
        {
            var result = await _context.SalePoints
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("get-blogs")]
        public async Task<ActionResult> GetBlogs()
        {
            var blogs = await _context.Blogs.ProjectTo<BlogDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(blogs);
        }

        [HttpGet("get-blog/{id}")]
        public async Task<ActionResult> GetBlog(int id)
        {
            var blog = await _context.Blogs
                .Where(b => b.Id == id)
                .ProjectTo<BlogDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return Ok(blog);
        }

        [HttpGet("get-home")]
        public async Task<ActionResult> GetHome()
        {
            var lastBlogs = await _context.Blogs.OrderByDescending(b => b.CreatedDate)
                .Take(4)
                .ProjectTo<BlogDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var arbitraryCourses = await _context.Courses
                .Take(4)
                .ToListAsync();

            var teachers = await _context.Teachers
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .ToListAsync();


            return Ok(new
            {
                courses = _mapper.Map<CourseDto[]>(arbitraryCourses),
                blogs = lastBlogs,
                teachers
            });
        }
    }
}
