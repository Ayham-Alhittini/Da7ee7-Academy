using AutoMapper;
using Da7ee7_Academy.Data;
using Da7ee7_Academy.DTOs;
using Da7ee7_Academy.Entities;
using Da7ee7_Academy.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Da7ee7_Academy.Controllers
{
    [Authorize]
    public class StudentController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StudentController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("my-courses")]
        public async Task<ActionResult> MyCourses()
        {
            if (!User.IsInRole("Student"))
            {
                return Forbid("Access the enrollment is limited to student only, sorry admin ^-^");
            }

            var courses = await _context.Students_Courses
                .Include(sc => sc.Course)
                .Where(sc => sc.StudentId == User.GetUserId())
                .Select(sc => sc.Course)
                .ToListAsync();

            var teachers = await _context.Teachers.ToListAsync();

            return Ok(new 
            {
                courses = _mapper.Map<CourseDto[]>(courses),
                teachers = _mapper.Map<TeacherDto[]>(teachers)
            });
        }

        [HttpPost("enroll-in-course")]
        public async Task<ActionResult> EnrollInCourse(CourseEnrollDto enrollDto)
        {
            if (!User.IsInRole("Student"))
            {
                return Forbid("Access the enrollment is limited to student only, sorry admin ^-^");
            }

            ///validate the card number

            var cardNumber = await _context.Cards.FindAsync(enrollDto.CardNumber);
            if (cardNumber == null)
            {
                return Ok(new
                {
                    error = "رقم البطاقة غير صحيح"
                });///sended as ok because want to deal with the error on the form
            }

            ///check if card already taken
            
            if (cardNumber.StudentId != null)
            {
                return Ok(new
                {
                    error = "البطاقة مستعملة من قبل"
                });
            }
            
            ///mark the card as taken
            cardNumber.StudentId = User.GetUserId();
            cardNumber.CourseId = enrollDto.CourseId;

            var course = await _context.Courses.FindAsync(enrollDto.CourseId);
            
            if (course == null)
            {
                return NotFound("Course not exist");
            }

            var studentCourse = new Student_Course
            {
                CourseId = course.Id,
                StudentId = User.GetUserId(),
                TeacherId = course.TeacherId
            };

            _context.Students_Courses.Add(studentCourse);

            await _context.SaveChangesAsync();

            string NULL = null;
            return Ok(new
            {
                error = NULL
            });
        }

        [HttpGet("course/{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
            var isEnrolled = await _context.Students_Courses
                .AnyAsync(sc => sc.StudentId == User.GetUserId() && sc.CourseId == id) || User.IsInRole("Admin");

            if (!isEnrolled)
            {
                ///return course informations without lectures and files
                ///we seprated from down because the down one is fetch every thing about the course

                var toEnrollCourse = await _context.Courses.FindAsync(id);

                if (toEnrollCourse == null)
                {
                    return NotFound("Course not exist");
                }

                var toEnrollCourseDto = _mapper.Map<CourseDto>(toEnrollCourse);
                toEnrollCourseDto.isEnrolled = false;
                return Ok(toEnrollCourseDto);
            }


            var course = await _context.Courses
                .Include(course => course.Sections)
                .ThenInclude(section => section.SectionItems)
                .FirstOrDefaultAsync(course => course.Id == id);

            if (course == null)
            {
                return NotFound("Course not exist");
            }


            var watchedLst = await _context.WatchedLectures
                .Where(wl => wl.CourseId == id && wl.StudentId == User.GetUserId())
                .Select(wl => new
                {
                    wl.SectionItemId,
                    wl.WatchedDate
                })
                .ToListAsync();

            var watchedDictionary = watchedLst.ToDictionary(wl => wl.SectionItemId, wl => wl.WatchedDate);

            course.Sections = course.Sections.OrderBy(s => s.OrderNumber).ToList();

            for (int i = 0; i < course.Sections.Count; i++)
            {
                course.Sections[i].SectionItems =
                    course.Sections[i].SectionItems.OrderBy(sectionItems => sectionItems.OrderNumber).ToList();
            }

            var result = _mapper.Map<CourseDto>(course);
            foreach (var item in result.Sections)
            {
                foreach (var item2 in item.SectionItems)
                {
                    item2.WatchedDate = 
                        watchedDictionary.ContainsKey(item2.Id) ? watchedDictionary[item2.Id] : null;
                }
            }

            return Ok(result);
        }

        [HttpPut("mark-watched/{sectionItemId}")]
        public async Task<ActionResult> MarkWatched(int sectionItemId)
        {
            var sectionItem = await _context.SectionItems.FindAsync(sectionItemId);

            if (sectionItem == null)
            {
                return NotFound("Section Item not found");
            }

            var watchedLecture = await _context.WatchedLectures.FindAsync(User.GetUserId(), sectionItemId);

            if (watchedLecture == null)
            {
                watchedLecture = new WatchedLecture
                {
                    StudentId = User.GetUserId(),
                    CourseId = sectionItem.CourseId,
                    SectionItemId = sectionItemId,
                };

                _context.WatchedLectures.Add(watchedLecture);
            }
            else
            {
                watchedLecture.WatchedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get-teachers")]
        public async Task<ActionResult> GetTeachers()
        {
            var result = await _context.Teachers
                .Select(teacher => new
                {
                    teacher.Id,
                    teacher.Major,
                })
                .ToListAsync();
            return Ok(result);
        }
    }
}
