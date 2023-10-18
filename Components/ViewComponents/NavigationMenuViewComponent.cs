using Microsoft.AspNetCore.Mvc;
using WorldDominion.Models;

namespace WorldDominion.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent // ViewComponent 클래스 상속
    {
        // 메소드. 메소드 이름은 Invoke여야만 함. 이 메서드는 뷰컴퓨넌트가 호출될때 실행됨
        // IViewComponentResult는 뷰 컴포넌트의 결과를 나타내는 인터페이스
        public IViewComponentResult Invoke() // 
        {
            var menuItem = new List<MenuItem>
            {
                new MenuItem {Controller = "Home", Action = "Index", Label = "Home"},
                new MenuItem {Controller = "Departments", Action = "Index", Label = "Departments"},
                new MenuItem {Controller = "Products", Action = "Index", Label = "Products"},
                new MenuItem {Controller = "Carts", Action = "Index", Label = "View My Cart"},
                new MenuItem {Controller = "Home", Action = "About", Label = "About"},
                new MenuItem {Controller = "Home", Action = "Contact", Label = "Contact"},
                new MenuItem {Controller = "Home", Action = "Privacy", Label = "Privacy"},
            };
            
            return View(menuItem);  // 뷰컴포넌트에서 반환되는 것은 데이터를 가진 모델.
        }
    }
}