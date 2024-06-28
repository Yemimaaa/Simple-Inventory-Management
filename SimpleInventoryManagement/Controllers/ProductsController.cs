using InventoryTransactionsApp.DAL;
using InventoryTransactionsApp.Models;
using InventoryTransactionsApp.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTransactionsApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProduct _product;
        public ProductsController(IProduct product)
        {
            _product = product;
        }

        // GET: ProductsController
        public ActionResult Index(string productName = "")
        {
            IEnumerable<Product> products;
            if (productName != "")
            {
                products = _product.GetProductsByName(productName);
            }
            else
            {
                products = _product.GetAll();
            }
            return View(products);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            var product = _product.GetById(id);
            return View(product);
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                var result = _product.Add(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Failed to create Produc: {ex.Message}";
                return View(product);
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _product.GetById(id);
            return View(product);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                var result = _product.Update(product);

                TempData["Message"] = $"Product {product.Name} updated successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.ErrorMessage = "Product not updated";
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _product.GetById(id);
            return View(product);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteProduct(int ProductId)
        {
            try
            {
                _product.Delete(ProductId);
                TempData["Message"] = $"Product {ProductId} deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                ViewBag.ErrorMessage = $"Failed to delete product: {ex.Message}";
                return View();
            }
        }
    }
}