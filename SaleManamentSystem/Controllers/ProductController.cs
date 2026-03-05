using System.Web.Mvc;
using SaleManamentSystem.Models;
using SaleManamentSystem.Repository;
using System.Collections.Generic;

namespace SaleManamentSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository = new ProductRepository();

        public ActionResult Index()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }


        [HttpGet]
        public ActionResult AddProduct()
        {
            return PartialView("_AddProduct");
        }

        [HttpPost]
        public ActionResult AddProduct(ProductEntity product, string submitButton)
        {
            if (!string.IsNullOrEmpty(product.ProductID))
            {
                product.ProductID = product.ProductID.Trim().ToUpper();
            }

            if (ModelState.IsValid)
            {
                if (_productRepository.GetProductByID(product.ProductID) != null)
                {
                    ModelState.AddModelError("ProductID", "Mã sản phẩm này đã tồn tại!");
                    return PartialView("_AddProduct", product);
                }

                bool isSaved = _productRepository.AddNewProduct(product);

                if (isSaved)
                {
                    if (submitButton == "Continue")
                    {
                        ModelState.Clear();
                        return PartialView("_AddProduct", new ProductEntity());
                    }
                     return Content("<script>window.location.reload();</script>");

                }
            }

            return PartialView("_AddProduct", product);
        }

        [HttpGet]
        public ActionResult EditProduct(string productID)
        {
            var product = _productRepository.GetProductByID(productID);
            return PartialView("_EditProduct", product);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductEntity product)
        {
            if (ModelState.IsValid)
            {
                if (_productRepository.UpdateProduct(product))
                    return RedirectToAction("Index");
            }
            return PartialView("_EditProduct", product);
        }

        public ActionResult Details(string productID)
        {
            var product = _productRepository.GetProductByID(productID);
            return PartialView("_Details", product);
        }

        [HttpPost]
        public ActionResult DeleteProduct(string productID)
        {
            if (!_productRepository.IsProductInInvoice(productID))
            {
                _productRepository.DeleteProduct(productID);
            }
            else
            {
                TempData["Error"] = "Sản phẩm đã có trong hóa đơn, không thể xóa!";
            }
            return RedirectToAction("Index");
        }
    }
}