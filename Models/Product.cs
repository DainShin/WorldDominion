using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldDominion.Models
{
    // enum: 클래스와 동일한 수준에 있게 됨
    public enum ProductWeightUnit
    {
        // 위에서부터 0부터 시작
        GRAMS,
        KILOGRAMS,
        POUNDS,
        OUNCES,
        LITERS,
        UNITS,
        PIECES,
    }

    public class Product
    {
        [Key] 
        public int Id {get; set;} = 0;

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId {get; set;} = 0;

        [Required, StringLength(300)]
        public string Name {get; set;} = String.Empty;

        [StringLength(1000)]
        public string? Description {get; set;} = String.Empty;

        [StringLength(250)]
        public string? Image {get; set;} = String.Empty;

        [Required]
        [Range(0.01, 999999.99)]
        [DataType(DataType.Currency)]
        public decimal MSRP {get; set;} = 0.01M;

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Weight {get; set;} = 0.01M;  // M을 꼭 추가해야됨 (추가하지 않으면 double로 인식)

        // enum 형이 데이터를 제공하도록 열거형에 대한 필드 추가필요
        [Required]
        public ProductWeightUnit WeightUnit {get; set;} = ProductWeightUnit.UNITS;

        // 마이그레이션 추가하기 : dotnet ef migrations add 마이그레이션이름

        [ForeignKey("DepartmentId")]
        public virtual Department? Department{get; set;}  // ? -> optional
        // create the association to departments allows a department to be stored in an instance of a product
    }
}