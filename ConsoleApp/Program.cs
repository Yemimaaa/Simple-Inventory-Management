using System;
using System.Linq;
using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menu Utama:");
                Console.WriteLine("1. Add New Product");
                Console.WriteLine("2. Update Data");
                Console.WriteLine("3. Delete Data");
                Console.WriteLine("4. Lihat Inventaris");
                Console.WriteLine("5. Exit");
                Console.Write("Pilih opsi: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddNewProduct();
                            break;
                        case 2:
                            UpdateProduct();
                            break;
                        case 3:
                            DeleteProduct();
                            break;
                        case 4:
                            LihatInventaris();
                            break;
                        case 5:
                            return;
                        default:
                            Console.WriteLine("Opsi tidak valid. Tekan sembarang tombol untuk mencoba lagi.");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Input tidak valid. Tekan sembarang tombol untuk mencoba lagi.");
                    Console.ReadKey();
                }
            }
        }

        static void AddNewProduct()
        {
            using (var context = new InventoryDbContext())
            {
                Console.Clear();
                Product newProduct = new Product();
                Console.Write("Masukkan Product: ");
                newProduct.Name = Console.ReadLine();
                Console.Write("Masukkan Stock Level: ");
                if (int.TryParse(Console.ReadLine(), out int stockLevel))
                {
                    newProduct.StockLevel = stockLevel;
                }
                else
                {
                    Console.WriteLine("StockLevel tidak valid. Tekan sembarang tombol untuk kembali ke menu utama.");
                    Console.ReadKey();
                    return;
                }
                context.Products.Add(newProduct);
                context.SaveChanges();
                Console.WriteLine("Product berhasil ditambahkan! Tekan sembarang tombol untuk kembali ke menu utama.");
                Console.ReadKey();
            }
        }

        static void UpdateProduct()
        {
            using (var context = new InventoryDbContext())
            {
                Console.Clear();
                Console.Write("Masukkan ID Product yang ingin diperbarui: ");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    Product product = context.Products.Find(productId);
                    if (product != null)
                    {
                        Console.Write("Masukkan Product Name baru (kosongkan untuk tidak mengubah): ");
                        string name = Console.ReadLine();
                        if (!string.IsNullOrEmpty(name))
                        {
                            product.Name = name;
                        }
                        Console.Write("Masukkan Jumlah Stock baru (kosongkan untuk tidak mengubah): ");
                        string stockLevel = Console.ReadLine();
                        if (int.TryParse(stockLevel, out int stock))
                        {
                            product.StockLevel = stock;
                        }
                        context.SaveChanges();
                        Console.WriteLine("Product berhasil diperbarui! Tekan sembarang tombol untuk kembali ke menu utama.");
                    }
                    else
                    {
                        Console.WriteLine("Product tidak ditemukan. Tekan sembarang tombol untuk kembali ke menu utama.");
                    }
                }
                else
                {
                    Console.WriteLine("Input tidak valid. Tekan sembarang tombol untuk kembali ke menu utama.");
                }
                Console.ReadKey();
            }
        }

        static void DeleteProduct()
        {
            using (var context = new InventoryDbContext())
            {
                Console.Clear();
                Console.Write("Masukkan ID Product yang ingin dihapus: ");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    Product product = context.Products.Find(productId);
                    if (product != null)
                    {
                        Console.Write($"Yakin ingin menghapus Product: {product.Name} (y/n)? ");
                        if (Console.ReadLine().ToLower() == "y")
                        {
                            context.Products.Remove(product);
                            context.SaveChanges();
                            Console.WriteLine("Product berhasil dihapus! Tekan sembarang tombol untuk kembali ke menu utama.");
                        }
                        else
                        {
                            Console.WriteLine("Penghapusan dibatalkan. Tekan sembarang tombol untuk kembali ke menu utama.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Product tidak ditemukan. Tekan sembarang tombol untuk kembali ke menu utama.");
                    }
                }
                else
                {
                    Console.WriteLine("Input tidak valid. Tekan sembarang tombol untuk kembali ke menu utama.");
                }
                Console.ReadKey();
            }
        }

        static void LihatInventaris()
        {
            using (var context = new InventoryDbContext())
            {
                Console.Clear();
                Console.WriteLine("Inventaris Product:");
                Console.WriteLine("ID\tName\tJumlahStok");
                foreach (var product in context.Products)
                {
                    Console.WriteLine($"{product.ProductId}\t{product.Name}\t{product.StockLevel}");
                }
                Console.WriteLine("Tekan sembarang tombol untuk kembali ke menu utama.");
                Console.ReadKey();
            }
        }
    }
}