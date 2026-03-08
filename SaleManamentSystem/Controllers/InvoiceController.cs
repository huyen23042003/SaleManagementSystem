using SaleManamentSystem.Models;
using SaleManamentSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SaleManamentSystem.Controllers
{
    public class InvoiceController : Controller
    {
            
        private readonly InvoiceReporitory _invoiceRepository = new InvoiceReporitory();
        public ActionResult Index()
        {
            List<InvoiceEntity> invoices = _invoiceRepository.GetAllInvoices();
            return View(invoices);
        }


        public ActionResult Details(string invoiceID)
        {
            var invoice = _invoiceRepository.GetInvoiceByID(invoiceID);
            return PartialView("_Details", invoice);
        }


        [HttpGet]
        public ActionResult AddInvoice()
        {

            PopulateViewBags();
            var newInvoice =new InvoiceEntity
            {
                InvoiceDetails = new List<InvoiceDetailEntity>()
            };

            return PartialView("_AddInvoice",newInvoice);
        }

        [HttpPost]
        public ActionResult AddInvoice(InvoiceEntity invoice, string submitButton)
        {
            try
            {
                if (!string.IsNullOrEmpty(invoice.InvoiceID))
                {
                    invoice.InvoiceID = invoice.InvoiceID.Trim().ToUpper();
                }

                if (ModelState.IsValid)
                {
                    // Kiểm tra trùng mã
                    if (_invoiceRepository.GetInvoiceByID(invoice.InvoiceID) != null)
                    {
                        ModelState.AddModelError("InvoiceID", "Mã hóa đơn này đã tồn tại!");
                        PopulateViewBags(); // Phải nạp lại dữ liệu cho Dropdown
                        return PartialView("_AddInvoice", invoice);
                    }

                    // Lưu dữ liệu
                    bool isSaved = _invoiceRepository.AddInvoice(invoice);
                    if (isSaved)
                    {
                        if (submitButton == "Continue")
                        {
                            ModelState.Clear();
                            PopulateViewBags();
                            var newInvoice = new InvoiceEntity { InvoiceDetails = new List<InvoiceDetailEntity>() };
                            return PartialView("_AddInvoice", newInvoice);
                        }
                        return Content("<script>window.location.reload();</script>");
                    }
                }

                PopulateViewBags(); 

                if (invoice.InvoiceDetails == null)
                {
                    invoice.InvoiceDetails = new List<InvoiceDetailEntity>();
                }

                return PartialView("_AddInvoice", invoice);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CRITICAL ERROR: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        private void PopulateViewBags()
        {
            CustomerRepository customerRepo = new CustomerRepository();
            ViewBag.Customers = customerRepo.GetAllCustomers().Select(c => new SelectListItem
            {
                Value = c.CustomerID,
                Text = string.Format(c.CustomerID + " - " + c.CustomerName)
            }).ToList();

            ProductRepository productRepo = new ProductRepository();
            ViewBag.Products = productRepo.GetAllProducts();
        }
        [HttpGet]
        public ActionResult EditInvoice(string invoiceID)
        {
            InvoiceEntity invoice = _invoiceRepository.GetInvoiceByID(invoiceID);
            if(invoice == null)
            {
                return HttpNotFound();
            }
            PopulateViewBags();
            return PartialView("_EditInvoice",invoice);
        }

        [HttpPost]
        public ActionResult EditInvoice(InvoiceEntity invoice)
        {
            if (ModelState.IsValid)
            {
                if (_invoiceRepository.UpdateInvoice(invoice))
                {
                    return Content("<script>window.location.reload();</script>");
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi Model: " + error.ErrorMessage);
            }
            PopulateViewBags();
            return PartialView("_EditInvoice", invoice);
        }

        [HttpPost]
        public ActionResult DeleteInvoice(string invoiceID)
        {
            if (_invoiceRepository.DeleteInvoice(invoiceID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Xảy ra lỗi khi xóa hóa đơn này";
                return RedirectToAction("Index");


            }

        }

        [HttpGet]
        public JsonResult CheckExistingInvoice(string invoiceID)
        {
            if (string.IsNullOrEmpty(invoiceID)) return Json(false, JsonRequestBehavior.AllowGet);

            invoiceID = invoiceID.Trim().ToUpper();
            bool exists = _invoiceRepository.GetInvoiceByID(invoiceID) != null;

            return Json(exists, JsonRequestBehavior.AllowGet);
        }
    }
}