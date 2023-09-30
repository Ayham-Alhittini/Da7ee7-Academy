using Da7ee7_Academy.Data;
using Da7ee7_Academy.Extensions;
using Da7ee7_Academy.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Da7ee7_Academy.Controllers
{
    public class TestContoller: BaseApiController
    {
        private readonly DataContext _context;
        private readonly IPhotoRepository _photoRepository;
        private readonly IWebHostEnvironment _env;
        public TestContoller(DataContext context, IPhotoRepository photoRepository, IWebHostEnvironment env)
        {
            _context = context;
            _photoRepository = photoRepository;
            _env = env;
        }

        [HttpGet]
        public IActionResult Test()
        {
            if (!_env.IsDevelopment())
            {
                return Ok(new
                {
                    Message = "Sorry this endpoint only accessable in development mode"
                });
            }

            string referrer = Request.Headers["Referer"].ToString();

            if (!referrer.StartsWith(_env.GetUrlRoot()))
            {
                return Ok(new
                {
                    Error = "Access video content only from website"
                });
            }

            return Ok(referrer);
        }
    }
}
