using InventoryTransactionsApp.DAL;
using Microsoft.EntityFrameworkCore;
using InventoryTransactionsApp.Models;
using System;

namespace InventoryTransactionsApp.EF
{
    public class ProductEF : IProduct
    {
        private readonly InventoryDbContext _context;
        public ProductEF(InventoryDbContext context)
        {
            _context = context;
        }

        public Product Add(Product entity)
        {
            try
            {
                _context.Products.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var deleteProduct = GetById(id);
                if (deleteProduct != null)
                {
                    _context.Products.Remove(deleteProduct);
                    _context.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Category not found");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            var results = _context.Products.ToList();
            return results;
        }

        public Product GetById(int id)
        {
            var result = _context.Products.Where(c => c.ProductId == id).FirstOrDefault();
            if (result == null)
            {
                throw new ArgumentException("Product not found");
            }
            return result;
        }

        public Product Update(Product entity)
        {
            try
            {
                var updateProduct = GetById(entity.ProductId);
                if (updateProduct != null)
                {
                    updateProduct.Name = entity.Name;
                    updateProduct.StockLevel = entity.StockLevel;
                    _context.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Product not found");
                }
                return updateProduct;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public IEnumerable<Product> GetProductsByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be null or empty", nameof(productName));
            }

            var results = _context.Products
                .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
                .ToList();

            return results;
        }

    }
}