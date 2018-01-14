using ShopModels;

namespace Shop.Data
{
    public static class ProductsInitializer
    {
        public static void SeedProducts(DataContext dataContext)
        {
            dataContext.Products.Add(new Product() { Name = "car", Description = "good car", Price = 100.2m });
            dataContext.Products.Add(new Product() { Name = "car2", Description = "better car", Price = 9.8m });
            dataContext.Products.Add(new Product() { Name = "car3", Description = "the best car", Price = 1m });
            dataContext.Products.Add(new Product() { Name = "bike", Description = "good bike", Price = 15m });
            dataContext.Products.Add(new Product() { Name = "bus", Description = "good bus", Price = 7.3m });
            dataContext.Products.Add(new Product() { Name = "train", Description = "good train", Price = 10m });
            dataContext.SaveChanges();
        }
    }
}
