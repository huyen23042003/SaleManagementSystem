using System.Web.Mvc;
using SaleManamentSystem.Models;
using SaleManamentSystem.Repository;
using System.Collections.Generic;

namespace SaleManamentSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository = new CustomerRepository();

        public ActionResult Index()
        {
            var customers = _customerRepository.GetAllCustomers();
            return View(customers);
        }

        [HttpGet]
        public ActionResult AddCustomer()
        {
            return PartialView("_AddCustomer");
        }

        [HttpPost]
        public ActionResult AddCustomer(CustomerEntity customer, string submitButton)
        {
            if (!string.IsNullOrEmpty(customer.CustomerID))
            {
                customer.CustomerID = customer.CustomerID.Trim().ToUpper();
            }

            if (ModelState.IsValid)
            {
                if (_customerRepository.GetCustomerByID(customer.CustomerID) != null)
                {
                    ModelState.AddModelError("CustomerID", "Mã khách hàng này đã tồn tại!");
                    TempData["Error"] =  "Mã khách hàng này đã tồn tại!";

                    return PartialView("_AddCustomer", customer);
                }

                bool isSaved = _customerRepository.AddNewCustomer(customer);

                if (isSaved)
                {
                    if (submitButton == "Continue")
                    {
                        ModelState.Clear();
                        return PartialView("_AddCustomer", new CustomerEntity());
                    }
                    return Content("<script>window.location.reload();</script>");
                }
            }

            return PartialView("_AddCustomer", customer);
        }
        [HttpGet]
        public ActionResult EditCustomer(string customerID)
        {
            var customer = _customerRepository.GetCustomerByID(customerID);
            return PartialView("_EditCustomer", customer);
        }

        [HttpPost]
        public ActionResult EditCustomer(CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                if (_customerRepository.UpdateCustomer(customer))
                    return RedirectToAction("Index");
            }
            return PartialView("_EditCustomer", customer);
        }

        public ActionResult Details(string customerID)
        {
            var customer = _customerRepository.GetCustomerByID(customerID);
            return PartialView("_Details", customer);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(string customerID)
        {
            if (!_customerRepository.IsCustomerInInvoice(customerID))
            {
                _customerRepository.DeleteCustomer(customerID);
            }
            else
            {
                TempData["Error"] = "Khách hàng đã có trong hóa đơn, không thể xóa!";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult CheckExistingCustomer(string customerID)
        {
            if (string.IsNullOrEmpty(customerID))
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
            customerID = customerID.Trim().ToUpper();
            bool exists = _customerRepository.GetCustomerByID(customerID) != null;
            return Json(exists, JsonRequestBehavior.AllowGet);
        }
      
    }
}