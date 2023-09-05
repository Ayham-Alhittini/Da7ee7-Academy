using Da7ee7_Academy.Data;
using Da7ee7_Academy.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Da7ee7_Academy.Controllers
{
    public class FilesController: BaseApiController
    {
        private readonly DataContext _context;
        public FilesController(DataContext context)
        {
            _context = context;
        }

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

        
        ///we seprate it from GetImages because we will add validation to it later
        [HttpGet("get-file/{fileId}")]
        public async Task<IActionResult> GetFile(string fileId)
        {
            //string referrer = Request.Headers["Referer"].ToString();

            //if (referrer != "")
            //{
            //    return Ok(new ResponseModel
            //    {
            //        StatusCode = 401,
            //        Errors = new List<string> { "Access the video only from the website" }
            //    });
            //}

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
