using WorldDominion.Models;
using Newtonsoft.Json;

namespace WorldDominion.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; // IHttpContextAccessor 인터페이스 사용-> HTTP 요청에 대한 정보 접근
        private const string _cartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Cart? GetCart()
        {
            // 세션에서 _cartSessionKey에 해당하는 값을 가져옴
            var cartJson = _httpContextAccessor.HttpContext.Session.GetString(_cartSessionKey);
            
            // 가져온 값이 null이면 빈 장바구니 객체를 반환, 그렇지 않으면 Json으로 직렬화 
            return cartJson == null ? new Cart() : JsonConvert.DeserializeObject<Cart>(cartJson);
        }

        public void SaveCart(Cart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);

            // _cartSessionKey에 해당하는 세션에 Json 문자열을 저장
            _httpContextAccessor.HttpContext.Session.SetString(_cartSessionKey, cartJson);
        }

        public void DestroyCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove(_cartSessionKey);
        }
    }
}