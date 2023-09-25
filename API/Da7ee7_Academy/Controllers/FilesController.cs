using Da7ee7_Academy.Data;
using Da7ee7_Academy.Extensions;
using Da7ee7_Academy.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Da7ee7_Academy.Controllers
{
    public class FilesController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;
        public FilesController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /////////////////*************************///////////////
        ////////////////  Be careful before change the method name consider that it's used to access the method /////////////////

        [HttpGet("images/{photoId}")]
        public async Task<IActionResult> GetImages(string photoId)
        {
            var file = await _context.Files.FindAsync(photoId);

            if (file == null)
            {
                return Ok(new ResponseModel
                {
                    StatusCode = 404,
                    Errors = new List<string> { "Image not found"}
                });
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileBytes, file.ContentType);
        }


        ///While request can't came from unauthorized website we don't need to make it auth and check if student enroll in course
        ///because that guaranteed from the website
        [HttpGet("get-file/{fileId}")]
        public async Task<IActionResult> GetFile(string fileId)
        {
            string referrer = Request.Headers["Referer"].ToString();

            if (!referrer.StartsWith("https://localhost:4200/") && !referrer.StartsWith(_env.GetUrlRoot()))
            {
                return Ok(new
                {
                    Error = "Access only allowed from website"
                });
            }

            var file = await _context.Files.FindAsync(fileId);

            if (file == null)
            {
                return Ok(new ResponseModel
                {
                    StatusCode = 404,
                    Errors = new List<string> { "File not found" }
                });
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.Path, file.FileName);

            //var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var filestream = System.IO.File.OpenRead(filePath);

            return File(filestream, contentType: file.ContentType, fileDownloadName: file.FileName, enableRangeProcessing: true);
        }
    }
}
