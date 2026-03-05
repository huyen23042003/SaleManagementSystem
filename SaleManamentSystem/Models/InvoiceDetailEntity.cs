using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManamentSystem.Models
{
    public class InvoiceDetailEntity
    {
        public string InvoiceDetailID { get; set; }
        public string InvoiceID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        public string ProductID { get; set; }

        public string ProductName { get; set; }

        [Required(ErrorMessage = "Nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")] 
        public int Quantity { get; set; }

        public decimal Price { get; set; } 
        public decimal TotalPrice { get; set; } 
    }
}