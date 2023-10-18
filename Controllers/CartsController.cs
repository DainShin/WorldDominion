using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WorldDominion.Models;

namespace WorldDominion.Controllers
{
    public class CartsController : Controller // tab 클릭 -> load library
    {
        private readonly string _cartSessionKey; 
        private readonly ApplicationDbContext _context;  // connection. to interact with database

        // constructor
        public CartsController(ApplicationDbContext context)
        {
            _cartSessionKey = "Cart";
            _context = context;
        }


        /*
            Task: 비동기작업 나타냄
            IActionResult: 해당 메서드가 컨트롤러의 액션메서드이며 결과로 뷰를 반환
            async : 메서드 선언 앞에 사용. 해당 메서드가 비동기적으로 실행됨을 나타냄. 비동기 메서드는 해당 메서드 안에서 await 키워드를 사용할 수 있도록 해줌
            await : 비동기 메서드 안에서 사용됨. 비동기 작업이 완료될때까지 기다림. await는 Task<T>  또는 Task를 반환하는 비동기 메서드에서 사용됨. 반환타입은 Task<T> 또는 Task여야함


        */
        public async Task<IActionResult> Index()
        {
            // Get our cart (either an existing or generate a new one)
            var cart = GetCart();

            if(cart == null) 
            {
                return NotFound();
            }

            // If the cart has items, we need to add the product reference for those items
            if(cart.CartItems.Count > 0)
            {
                foreach (var cartItem in cart.CartItems)
                {
                    /* 데이터베이스 Products 테이블을 proudct에 담음

                       SELECT * FROM Products 
                       JOIN Departments ON Products.DepartmentId = Departments.Id
                       WHERE Products.Id = 1 LIMIT 1 
                     */
                    var product = await _context.Products  // Product 
                        .Include (p => p.Department)       // Product 정보 가져올때 Department 정보도 함께 가져오라는 의미(즉 Product테이블과 Department테이블의 조인 수행) 
                        .FirstOrDefaultAsync (p => p.Id == cartItem.ProductId); // Product 테이블의 Id와 cartItem에 있는 ProductId가 일치하는 것 중 첫번째 제품을 비동기적으로 조회

                    if (product != null)
                    {
                        cartItem.Product = product;  // We are interacting with object. All works with reference
                    }    
                    /*
                        var x = 5;
                        var y = x;
                        x = 10;

                        var u = new {name="Shaun", last="Mckinno"};
                        var o = u;
                        u.name = "Job";
                        o.name?  ==> Job
                    */
                }
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Getting the active cart
            var cart = GetCart();

            if(cart == null)
            {
                return NotFound(); // 404
            }

            // Checking if item already is in the cart
            // Find : 컬렉션에서 조건을 만족하는 첫번째 요소를 찾음.
            var cartItem = cart.CartItems.Find(cartItem => cartItem.ProductId == productId);

            // 조건에 맞는 장바구니 항목이 존재하는 경우 수량 추가
            if(cartItem != null && cartItem.Product != null)
            {
                cartItem.Quantity += quantity;
            }
            else // 조건에 맞는 장바구니 항목이 존재하지 않는 경우 
            {
                var product = await _context.Products  // Products 테이블에서 데이터 가져와서
                    .FirstOrDefaultAsync(p => p.Id == productId);  // 데이터베이스에서 produnctId에 해당하는 제품 찾기
                
                if(product == null)  // 만약 제품이 존재하지 않으면 
                {
                    return NotFound();  // NotFound()
                }

                // 제품이 존재하면
                cartItem = new CartItem { ProductId = productId, Quantity = quantity, Product = product}; // 새로운 CartItem 객체 생성, 각 값 할당
                cart.CartItems.Add(cartItem);   // 새로운 cartItem을 cart.CartItems 컬렉션에 추가    
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        private Cart? GetCart()
        {
            var cartJson = HttpContext.Session.GetString(_cartSessionKey); // HttpContext.Session : 현재 세션 상태에 접근하기 위한 세션 객체 나타냄

            return cartJson == null ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartJson);  // Cart객체로 역직렬화하여 읽어온 장바구니 데이터를 복원
        }

        private void SaveCart(Cart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            
            HttpContext.Session.SetString(_cartSessionKey, cartJson);
        }
    }
}