using SaleManamentSystem.Models;
using SaleManamentSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaleManamentSystem.Controllers
{
    public class InvoiceController : Controller
    {
        public ActionResult Index()
        {
            InvoiceReporitory invoiceReporitory = new InvoiceReporitory();
            List<InvoiceEntity> invoices = invoiceReporitory.GetAllInvoices();
            return View(invoices);
        }

        [HttpGet]
        public ActionResult AddInvoice()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult AddInvoice(InvoiceEntity invoice)
        //{
        //    if(Modal.)
        //}


    }
}