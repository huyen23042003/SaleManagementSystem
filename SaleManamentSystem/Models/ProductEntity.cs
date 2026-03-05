using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManamentSystem.Models
{
    public class ProductEntity
    {
        [Key]
        [DisplayName("Mã sản phẩm")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Mã khách hàng không được chứa khoảng trắng")]
        [Required(ErrorMessage ="Bắt buộc nhập mã sản phẩm")]
        public string ProductID { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]

        public string ProductName { get; set; }

        [DisplayName("Giá sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Chỉ được nhập số")]


        public decimal Price { get; set; }


    }
}