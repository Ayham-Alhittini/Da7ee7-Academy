using Da7ee7_Academy.Data;
using Da7ee7_Academy.Entities;
using Da7ee7_Academy.Enums;
using Da7ee7_Academy.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Da7ee7_Academy.Controllers
{
    public class TestContoller: BaseApiController
    {
        private readonly DataContext _context;
        public TestContoller(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Test()
        {
            string referrer = Request.Headers["Referer"].ToString();
            return Ok(referrer);
        }
    }
}
