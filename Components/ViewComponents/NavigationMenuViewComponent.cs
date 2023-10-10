using Microsoft.AspNetCore.Mvc;
using WorldDominion.Models;

namespace WorldDominion.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        // 메소드. 메소드 이름은 Invoke여야만 함
        public IViewComponentResult Invoke()
        {
            var menuItem = new List<MenuItem>
            {
                new MenuItem {Controller = "Home", Action = "Index", Label = "Home"},
                new MenuItem {Controller = "Departments", Action = "Index", Label = "Departments"},
                new MenuItem {Controller = "Products", Action = "Index", Label = "Products"},
                new MenuItem {Controller = "Home", Action = "Privacy", Label = "Privacy"},
            };
            
            return View(menuItem);
        }
    }
}