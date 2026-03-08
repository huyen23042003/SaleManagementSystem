using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SaleManamentSystem.Models
{
    public class InvoiceEntity
    {
        [Required(ErrorMessage = "Bắt buộc nhập mã hóa đơn")]
        [DisplayName("Mã hóa đơn")]
        public string InvoiceID { get; set; }

        [Required(ErrorMessage = "Bắt buộc chọn khách hàng")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Mã hóa đơn không được chứa khoảng trắng")]

        [DisplayName("Mã khách hàng")]

        public string CustomerID { get; set; }

        [DisplayName("Tên khách hàng")]

        public string CustomerName { get; set; }

        [DisplayName("Ngày lập")]

        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [DisplayName("Tổng tiền")]

        public decimal TotalPrice { get; set; }

        public List<InvoiceDetailEntity> InvoiceDetails { get; set; } = new List<InvoiceDetailEntity>();

        public InvoiceEntity()
        {
            InvoiceDetails = new List<InvoiceDetailEntity>();
        }
    }
}