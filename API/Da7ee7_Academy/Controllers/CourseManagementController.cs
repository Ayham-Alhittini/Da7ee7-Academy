using AutoMapper;
using AutoMapper.QueryableExtensions;
using Da7ee7_Academy.Data;
using Da7ee7_Academy.DTOs;
using Da7ee7_Academy.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Da7ee7_Academy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseManagementController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public CourseManagementController(DataContext context,
            IWebHostEnvironment env, 
            IMapper mapper)
        {
            _context = context;
            _env = env;
            _mapper = mapper;
        }

        [HttpPost("add-course")]
        public ActionResult AddCourse([FromForm]AddCourseDto courseDto)
        {
            ///the course with same major and same subject and same teacher should be rejected
            ///

            if (CheckCourseExist(courseDto))
            {
                return BadRequest("Course already exist");
            }
            ///check the course cover
            if (!courseDto.CourseCover.ContentType.Contains("image"))
            {
                return BadRequest("only accept images for course cover");
            }

            var course = new Course
            {
                Subject = courseDto.Subject,
                TeacherId = courseDto.TeacherId,
                Major = courseDto.Major,
            };
            //save early to get the id of the course
            _context.Courses.Add(course);
            _context.SaveChanges();

            //Create folder for this course to store everything on in it
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"Uploads\Courses", "Course#" + course.Id);
            Directory.CreateDirectory(folderPath);


            ///save the image
            var file = SavePhoto(courseDto.CourseCover, course.Id); ///Save the Course cover photo on database and on files

            course.FileId = file.Id;
            course.CoursePhotoUrl = GetUrlRoot() + Url.Action("GetImages", "Files", new { photoId = file.Id });


            _context.SaveChanges(); ///for saving FileId and CoursePhotoUrl

            return Ok();
        }

        [HttpDelete("delete-course/{courseId}")]
        public ActionResult DeleteCourse(int courseId)
        {
            var course = _context.Courses.Find(courseId);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            _context.Courses.Remove(course);//delete course from DB
            var filesToDelete =  _context.Files.Where(file => file.Path.Contains("Course#" + courseId)).ToList();

            _context.Files.RemoveRange(filesToDelete);/// delete files from DB

            _context.SaveChanges();

            var courseFolder = Path.Combine(Directory.GetCurrentDirectory(), @"Uploads\Courses\Course#" + courseId);
            // Delete the folder and its contents recursively.
            Directory.Delete(courseFolder, recursive: true);

            return Ok();
        }

        [HttpPost("add-course-section")]
        public ActionResult AddCourseSection(AddCourseSectionDto sectionDto)
        {
            var course = _context.Courses.Include(course => course.Sections)
                .FirstOrDefault(course => course.Id == sectionDto.CourseId);

            if (course == null)
            {
                return Ok("Course not found");
            }

            var section = new Section
            {
                SectionTitle = sectionDto.SectionTitle,
                CourseId = sectionDto.CourseId,
                OrderNumber = course.Sections.Count() + 1,
            };

            course.Sections.Add(section);
            _context.SaveChanges();


            var path = $@"Uploads\Courses\Course#{course.Id}\Section#{section.Id}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), path);
            Directory.CreateDirectory(folderPath);

            return Ok();
        }

        [HttpDelete("delete-course-section/{sectionId}")]///add section items first to test it better
        public ActionResult DeleteCourseSection(int sectionId)
        {
            var section = _context.Sections
                .Include(section => section.SectionItems)
                .ThenInclude(sectionItems => sectionItems.File)
                .FirstOrDefault(section => section.Id == sectionId);

            if (section == null)
            {
                return NotFound("Section not found");
            }
            var course = _context.Courses
                .Where(course => course.Id == section.CourseId)
                .Include(course => course.Sections.OrderBy(s => s.OrderNumber))
                .FirstOrDefault();

            /*
             Need to delete
                1- the section itself from db, all the section items will deleted as well since it's set to be cascade
                2- all the files in database for those section items
                3- the section folder itself
             */

            //Delete the section from DB (delete the section and section items)
            _context.Sections.Remove(section);

            //Delete the Section items files from DB
            var sectionItemsFiles = section.SectionItems.Select(si => si.File).ToList();
            _context.Files.RemoveRange(sectionItemsFiles);

            ///Delete the section folder
            var path = $@"Uploads\Courses\Course#{section.CourseId}\Section#{section.Id}";
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
            Directory.Delete(fullPath, true);


            ///update order number for other sections
            int newOrderNumber = section.OrderNumber;

            for (int i = section.OrderNumber; i < course.Sections.Count; i++, newOrderNumber++)
            {
                course.Sections[i].OrderNumber = newOrderNumber;
            }

            _context.SaveChanges();

            return Ok();
        }



        [HttpPost("add-section-item")]
        [RequestSizeLimit(1000 * 1024 * 1024)]///MAX SIZE 1 GB
        public ActionResult AddSectionItem([FromForm] AddSectionItemDto sectionItemDto)
        {

            var section = _context.Sections.Include(section => section.SectionItems)
                .FirstOrDefault(section => section.Id == sectionItemDto.SectionId);

            if (section == null)
            {
                return NotFound("Section not found");
            }

            //check the type
            string fileExtension = Path.GetExtension(sectionItemDto.File.FileName).ToLower();
            int type = -1;
            if (fileExtension != ".pdf" && fileExtension != ".mp4")
            {
                return BadRequest("Unexpcted type only accept mp4 files for video and pdf for attachments");
            }
            else
            {
                switch(fileExtension)
                {
                    case ".pdf": type = 1; sectionItemDto.VideoLength = 0;
                        break;
                    case ".mp4": type= 2; 
                        break;
                }
            }

            //create the section item
            var sectionItem = new SectionItem
            {
                SectionItemTitle = sectionItemDto.SectionItemTitle,
                OrderNumber = section.SectionItems.Count() + 1,
                Type = type,
                SectionId = sectionItemDto.SectionId,
                CourseId = section.CourseId,
                VideoLength = sectionItemDto.VideoLength,
            };

            section.TotalSectionTime += sectionItem.VideoLength;

            ///add the file
            var file = SaveSectionItemFile(sectionItemDto.File, section);


            //connect file to section item
            sectionItem.FileId = file.Id;
            sectionItem.ContentUrl = GetUrlRoot() + Url.Action("GetFile", "Files", new { fileId = file.Id });

            ///add the section item
            section.SectionItems.Add(sectionItem);

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("delete-section-item/{sectionItemId}")]
        public ActionResult DeleteSectionItem(int sectionItemId)
        {
            var sectionItem = _context.SectionItems.Include(si => si.File)
                .FirstOrDefault(si => si.Id == sectionItemId);

            if (sectionItem == null)
            {
                return NotFound("Section item not found");
            }
            var section = _context.Sections
                .Where(section => section.Id == sectionItem.SectionId)
                .Include(si => si.SectionItems.OrderBy(si => si.OrderNumber))
                .FirstOrDefault();

            //delete from DB
            _context.SectionItems.Remove(sectionItem);
            _context.Files.Remove(sectionItem.File);

            //delete from folder
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                sectionItem.File.Path, sectionItem.File.FileName);

            System.IO.File.Delete(filePath);


            ////update order number for section items
            int newOrderNumber = sectionItem.OrderNumber;
            for (int i = sectionItem.OrderNumber; i < section.SectionItems.Count; i++, newOrderNumber++)
            {
                section.SectionItems[i].OrderNumber = newOrderNumber;
            }

            ///subtract time from section time
            section.TotalSectionTime -= sectionItem.VideoLength;

            ///save changes
            _context.SaveChanges();

            return Ok();
        }



        ///end-points to get the data to about the course to manage it
        
        [HttpGet("get-course-sections/{courseId}")]
        public ActionResult GetCourseSections(int courseId)
        {
            var course = _context.Courses.Find(courseId);

            if (course == null)
            {
                return NotFound("Course not exist");
            }    

            var sections = _context.Sections
                .Where(section => section.CourseId == courseId)
                .Select(section => new
                {
                    section.Id,
                    section.SectionTitle,
                    section.OrderNumber
                })
                .OrderBy(section => section.OrderNumber)
                .ToList();

            return Ok(sections);
        }


        [HttpGet("get-course-section/{sectionId}")]
        public ActionResult GetCourseSectoion(int sectionId)
        {
            var section = _context.Sections
                .Where(section => section.Id == sectionId)
                .ProjectTo<SectionDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            if (section == null)
            {
                return NotFound("Section not exist");
            }

            section.SectionItems = section.SectionItems.OrderBy(si => si.OrderNumber).ToList();

            return Ok(section);
        }

        ///Private Methods
        
        private bool CheckCourseExist(AddCourseDto courseDto)
        {
            return _context.Courses
                .Any(course => course.TeacherId == courseDto.TeacherId
                               && course.Major == courseDto.Major
                               && course.Subject == courseDto.Subject);
        }
        private AppFile SavePhoto(IFormFile photo, int courseId)
        {
            var file = new AppFile
            {
                ContentType = photo.ContentType,
                Path = @"Uploads\Courses\Course#" + courseId,
            };

            var extension = photo.FileName.Split('.').LastOrDefault();

            file.FileName = file.Id + "." + extension;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }
            
            _context.Files.Add(file);

            return file;
        }
        private AppFile SaveSectionItemFile(IFormFile formFile, Section section)
        {
            var file = new AppFile
            {
                ContentType = formFile.ContentType,
                Path = $@"Uploads\Courses\Course#{section.CourseId}\Section#{section.Id}",
            };

            var extension = formFile.FileName.Split('.').LastOrDefault();

            file.FileName = file.Id + "." + extension;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            _context.Files.Add(file);

            return file;
        }
        private string GetUrlRoot()
        {
            return _env.IsDevelopment() ? "https://localhost:7124" : "production url";
        }
    }
}
