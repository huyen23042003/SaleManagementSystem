using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SaleManamentSystem.Models
{

    public class CustomerEntity
    {
        [Key]
        [Required(ErrorMessage = "Vui lòng nhập mã khách hàng")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Mã khách hàng không được chứa khoảng trắng")]
        [DisplayName("Mã khách hàng")]
        public string CustomerID { get; set; }

        [DisplayName("Tên khách hàng")]
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string CustomerName { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Số điện thoại chỉ được nhập số")]
        [StringLength(10, MinimumLength =(9), ErrorMessage= "Số điện thoại tối thiểu ")]
        public string Phone { get; set; }
    }
}