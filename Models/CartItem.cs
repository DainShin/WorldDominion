// 네이밍할때 dash, 스페이스 사용 불가
namespace WorldDominion.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // Product에 저장되어있는 이름 등... 정보에 접근하기 위해
        public Product Product { get; set; } = new Product();
    }
}