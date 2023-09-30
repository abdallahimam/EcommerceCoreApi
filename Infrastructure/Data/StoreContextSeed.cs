using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedingData(StoreContext context) {

            if (!context.ProductBrands.Any()) {
                var brandsData = await File.ReadAllTextAsync("../Core/DummyData/Brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                await context.ProductBrands.AddRangeAsync(brands);
            }

            if (!context.ProductTypes.Any()) {
                var typesData = await File.ReadAllTextAsync("../Core/DummyData/Types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                await context.ProductTypes.AddRangeAsync(types);
            }

            if (!context.Products.Any()) {
                var productsData = await File.ReadAllTextAsync("../Core/DummyData/Products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                await context.Products.AddRangeAsync(products);
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = await File.ReadAllTextAsync("../Core/DummyData/Delivery.json");
                var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                await context.DeliveryMethods.AddRangeAsync(deliveries);
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}