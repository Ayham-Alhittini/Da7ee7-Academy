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
    [Authorize(Roles = "Student, Admin")]
    public class StudentController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StudentController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("course/{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
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
    }
}
