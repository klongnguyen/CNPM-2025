using CsvHelper;
using CsvHelper.Configuration;
using FashionStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FashionStore.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.Migrate();

            string baseDir = @"d:\Study\2025\CNPM 2025\dataset_fashion_store";
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null
            };

            // 1. Seed Channels
            if (!context.Channels.Any())
            {
                using var reader = new StreamReader(Path.Combine(baseDir, "dataset_fashion_store_channels.csv"));
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<dynamic>().ToList();
                foreach (var record in records)
                {
                    context.Channels.Add(new Channel
                    {
                        Name = record.channel,
                        Description = record.description
                    });
                }
                context.SaveChanges();
            }

            // 2. Seed Campaigns
            if (!context.Campaigns.Any())
            {
                using var reader = new StreamReader(Path.Combine(baseDir, "dataset_fashion_store_campaigns.csv"));
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<dynamic>().ToList();
                foreach (var record in records)
                {
                    context.Campaigns.Add(new Campaign
                    {
                        Id = int.Parse(record.campaign_id),
                        Name = record.campaign_name,
                        StartDate = DateTime.Parse(record.start_date),
                        EndDate = DateTime.Parse(record.end_date),
                        Channel = record.channel,
                        DiscountType = record.discount_type,
                        DiscountValue = record.discount_value
                    });
                }
                context.SaveChanges();
            }

            // 3. Seed Products
            if (!context.Products.Any())
            {
                using var reader = new StreamReader(Path.Combine(baseDir, "dataset_fashion_store_products.csv"));
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<dynamic>().ToList();
                foreach (var record in records)
                {
                    // Adding placeholder images based on category
                    string category = record.category?.ToString().ToLower() ?? "";
                    string imgUrl = "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?w=500&q=80"; // Default
                    if (category.Contains("dress")) imgUrl = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=500&q=80";
                    else if (category.Contains("shirt")) imgUrl = "https://images.unsplash.com/photo-1596755094514-f87e32f85e2c?w=500&q=80";
                    else if (category.Contains("shoe") || category.Contains("sneaker")) imgUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500&q=80";
                    else if (category.Contains("bag")) imgUrl = "https://images.unsplash.com/photo-1584916201218-f4242ceb4809?w=500&q=80";

                    context.Products.Add(new Product
                    {
                        Id = int.Parse(record.product_id),
                        Name = record.product_name,
                        Category = record.category,
                        Brand = record.brand,
                        Color = record.color,
                        Size = record.size,
                        CatalogPrice = decimal.Parse(record.catalog_price, CultureInfo.InvariantCulture),
                        CostPrice = decimal.Parse(record.cost_price, CultureInfo.InvariantCulture),
                        Gender = record.gender,
                        ImageUrl = imgUrl,
                        Description = $"Trải nghiệm phong cách thời trang cùng {record.product_name}. Thiết kế mang hơi hướng Minimalism & Swiss Style, tối giản nhưng thanh lịch."
                    });
                }
                context.SaveChanges();
            }

            // 4. Seed Stock
            if (!context.Stocks.Any())
            {
                using var reader = new StreamReader(Path.Combine(baseDir, "dataset_fashion_store_stock.csv"));
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<dynamic>().ToList();
                foreach (var record in records)
                {
                    context.Stocks.Add(new Stock
                    {
                        Country = record.country,
                        ProductId = int.Parse(record.product_id),
                        StockQuantity = int.Parse(record.stock_quantity)
                    });
                }
                context.SaveChanges();
            }

            // 5. Seed Customers
            if (!context.Customers.Any())
            {
                using var reader = new StreamReader(Path.Combine(baseDir, "dataset_fashion_store_customers.csv"));
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<dynamic>().ToList();
                
                int counter = 1;
                foreach (var record in records)
                {
                    int custId = int.Parse(record.customer_id);
                    context.Customers.Add(new Customer
                    {
                        Id = custId,
                        Country = record.country,
                        AgeRange = record.age_range,
                        SignupDate = DateTime.Parse(record.signup_date),
                        FullName = $"Customer {custId}",
                        Email = $"customer{custId}@owe.com",
                        Password = "123", // user requested no hashing
                        Role = custId == 1 ? "Admin" : "Customer" // first user is admin
                    });
                    counter++;
                }
                context.SaveChanges();
            }

            // 6. Seed Sales
            if (!context.Sales.Any())
            {
                using var reader = new StreamReader(Path.Combine(baseDir, "dataset_fashion_store_sales.csv"));
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<dynamic>().ToList();
                foreach (var record in records)
                {
                    context.Sales.Add(new Sale
                    {
                        Id = int.Parse(record.sale_id),
                        Channel = record.channel,
                        IsDiscounted = int.Parse(record.discounted),
                        TotalAmount = decimal.Parse(record.total_amount, CultureInfo.InvariantCulture),
                        SaleDate = DateTime.Parse(record.sale_date),
                        CustomerId = int.Parse(record.customer_id),
                        Country = record.country
                    });
                }
                context.SaveChanges();
            }

            // 7. Seed SaleItems
            if (!context.SaleItems.Any())
            {
                using var reader = new StreamReader(Path.Combine(baseDir, "dataset_fashion_store_salesitems.csv"));
                using var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<dynamic>().ToList();
                foreach (var record in records)
                {
                    context.SaleItems.Add(new SaleItem
                    {
                        Id = int.Parse(record.item_id),
                        SaleId = int.Parse(record.sale_id),
                        ProductId = int.Parse(record.product_id),
                        Quantity = int.Parse(record.quantity),
                        OriginalPrice = decimal.Parse(record.original_price, CultureInfo.InvariantCulture),
                        UnitPrice = decimal.Parse(record.unit_price, CultureInfo.InvariantCulture),
                        DiscountApplied = decimal.Parse(record.discount_applied, CultureInfo.InvariantCulture),
                        DiscountPercent = record.discount_percent,
                        IsDiscounted = int.Parse(record.discounted),
                        ItemTotal = decimal.Parse(record.item_total, CultureInfo.InvariantCulture),
                        SaleDate = DateTime.Parse(record.sale_date),
                        Channel = record.channel,
                        ChannelCampaigns = record.channel_campaigns
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
