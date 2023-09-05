using Da7ee7_Academy.Data;
using Da7ee7_Academy.DTOs;
using Da7ee7_Academy.Entities;
using Da7ee7_Academy.Enums;
using Da7ee7_Academy.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Da7ee7_Academy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseManagementController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;
        public CourseManagementController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("add-course")]
        public ResponseModel AddCourse([FromForm] AddCourseDto courseDto)
        {
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

            return new ResponseModel { IsSuccess = true, Message = "Course added successfuly"};
        }

        [HttpDelete("delete-course/{courseId}")]
        public ResponseModel DeleteCourse(int courseId)
        {
            var response = new ResponseModel();
            var course = _context.Courses.Find(courseId);

            if (course == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("Course not found");
                return response;
            }

            _context.Courses.Remove(course);//delete course from DB
            var filesToDelete =  _context.Files.Where(file => file.Path.Contains("Course#" + courseId)).ToList();

            _context.Files.RemoveRange(filesToDelete);/// delete files from DB

            _context.SaveChanges();

            var courseFolder = Path.Combine(Directory.GetCurrentDirectory(), @"Uploads\Courses\Course#" + courseId);
            // Delete the folder and its contents recursively.
            Directory.Delete(courseFolder, recursive: true);

            response.IsSuccess = true;
            response.Message = "Course deleted successfuly";

            return response;
        }



        [HttpPost("add-course-section")]
        public ResponseModel AddCourseSection(AddCourseSectionDto sectionDto)
        {
            var response = new ResponseModel();

            var course = _context.Courses.Include(course => course.Sections)
                .FirstOrDefault(course => course.Id == sectionDto.CourseId);

            if (course == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("Course not found");
                return response;
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

            response.IsSuccess = true;
            response.Message = "Section added successfuly";

            return response;
        }

        [HttpDelete("delete-course-section/{sectionId}")]///add section items first to test it better
        public ResponseModel DeleteCourseSection(int sectionId)
        {
            var response = new ResponseModel();

            var section = _context.Sections.Include(s => s.SectionItems)
                .ThenInclude(si => si.File)
                .FirstOrDefault(s => s.Id == sectionId);

            if (section == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("Section not found");

                return response;
            }


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
            
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Course section deleted successfuly";

            return response;
        }



        [HttpPost("add-section-item")]
        public ResponseModel AddSectionItem([FromForm] AddSectionItemDto sectionItemDto)
        {
            var response = new ResponseModel();

            var section = _context.Sections.Include(section => section.SectionItems)
                .FirstOrDefault(section => section.Id == sectionItemDto.SectionId);

            if (section == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("Section not found");

                return response;
            }

            //check the type
            int type = sectionItemDto.Type;
            string fileExtension = Path.GetExtension(sectionItemDto.File.FileName).ToLower();

            if (type == (int)SectionItemType.Attachment)
            {
                if (fileExtension != ".pdf")
                {
                    response.StatusCode = 400;
                    response.Errors.Add("Only accept pdf for attachments");

                    return response;
                }
            }
            else if (type == (int)SectionItemType.Lecture)
            {
                if (fileExtension != ".mp4")
                {
                    response.StatusCode = 400;
                    response.Errors.Add("only accept mp4 for lectures");

                    return response;
                }
            }
            else
            {
                response.StatusCode = 400;
                response.Errors.Add("Unexpected type, expect 1 as attachment and 2 as lecture");

                return response;
            }

            //create the section item
            var sectionItem = new SectionItem
            {
                SectionItemTitle = sectionItemDto.SectionItemTitle,
                OrderNumber = section.SectionItems.Count() + 1,
                Type = sectionItemDto.Type,
                SectionId = sectionItemDto.SectionId,
            };

            ///add the file
            var file = SaveSectionItemFile(sectionItemDto.File, section);


            //connect file to section item
            sectionItem.FileId = file.Id;
            sectionItem.ContentUrl = GetUrlRoot() + Url.Action("GetFile", "Files", new { fileId = file.Id });

            ///add the section item
            section.SectionItems.Add(sectionItem);

            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Section item added successfuly";

            return response;
        }

        [HttpDelete("delete-section-item/{sectionItemId}")]
        public ResponseModel DeleteSectionItem(int sectionItemId)
        {
            var response = new ResponseModel();
            var sectionItem = _context.SectionItems.Include(si => si.File)
                .FirstOrDefault(si => si.Id == sectionItemId);

            if (sectionItem == null)
            {
                response.StatusCode = 404;
                response.Errors.Add("Section item not found");
                
                return response;
            }

            //delete from DB
            _context.SectionItems.Remove(sectionItem);
            _context.Files.Remove(sectionItem.File);

            //delete from folder
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                sectionItem.File.Path, sectionItem.File.FileName);

            System.IO.File.Delete(filePath);

            ///save changes
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Section item deleted successfuly";
            
            return response;
        }
        
        

        ///Private Methods
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
