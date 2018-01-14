using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shop.Data;

namespace Shop
{
    public static class ProgramExtensions
    {
        public static IWebHost PopulateData(this IWebHost webhost)
        {
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                using (var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    ProductsInitializer.SeedProducts(dbContext);
                }
            }
            return webhost;
        }
    }
}
