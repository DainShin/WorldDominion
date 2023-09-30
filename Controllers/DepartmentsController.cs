using Microsoft.AspNetCore.Mvc; // 라이브러리

namespace WorldDominion
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Dictionary<string, Object>>data = new() 
            {
                new() { {"Id", "1"}, {"Name","Fruits & Veg"} },
                new() { {"Id", "2"}, {"Name","Meats"} },
                new() { {"Id", "3"}, {"Name","Sweets & Treats"} },
            };

            return View(data);
        }
    }
}