using InventoryTransactionsApp.Models;
using InventoryTransactionsApp.ViewModels;
using InventoryTransactionsApp.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransaction _transaction;
        private readonly IProduct _product;
        private readonly InventoryDbContext _dbContext;

        public TransactionsController(ITransaction transaction, IProduct product, InventoryDbContext dbContext)
        {
            _transaction = transaction;
            _product = product;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _dbContext.Transactions
                                             .Include(t => t.Product) // Assuming you have a navigation property
                                             .Select(t => new ViewTransactionInfo
                                             {
                                                 TransactionId = t.TransactionId,
                                                 ProductName = t.Product.Name, // Accessing the product name
                                                 Type = t.Type, // Convert enum to string if necessary
                                                 Quantity = t.Quantity,
                                                 Date = t.Date,
                                                 StockLevel = t.Product.StockLevel
                                             })
                                             .ToListAsync();

            return View(transactions);
        }


        public IActionResult Create()
        {
            var viewModel = new ViewTransactionInfo
            {
                Products = _product.GetAll().ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _transaction.Add(transaction);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.Products = _product.GetAll();
            return View(transaction);
        }
        public ActionResult Details(int id)
        {
            var transaction = _transaction.GetByIdJoin(id);
            return View(transaction);
        }

    }
}
