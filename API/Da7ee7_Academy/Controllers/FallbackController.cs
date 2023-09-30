using Microsoft.AspNetCore.Mvc;

namespace Da7ee7_Academy.Controllers
{
    public class FallbackController: Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "index.html"), "text/HTML");
        }
    }
}
