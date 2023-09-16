using Da7ee7_Academy.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Da7ee7_Academy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController: BaseApiController
    {
        private readonly DataContext _context;
        public AdminController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("get-teachers")]
        public ActionResult GetTeachers()
        {
            var result = _context.Users.Where(user => user.Role == RolesSrc.Teacher)
                .Select(user => new { user.Id, user.FullName }).ToList();

            return Ok(result);
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
    }
}
