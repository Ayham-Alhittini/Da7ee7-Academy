using Da7ee7_Academy.Data;
using Da7ee7_Academy.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Test(int courseId)
        {
            Dictionary<int, bool> watched = new Dictionary<int, bool> { { 1, true }, { 2, true }, { 3, true } };


            return Ok(watched.ContainsKey(courseId));
        }
    }
}
