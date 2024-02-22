using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoNameBikes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNameBikes.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AdventureWorksLT2017Context(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AdventureWorksLT2017Context>>()))
            {
                // Look for any products.
                if (context.Product.Any())
                {
                    return;   // DB has been seeded
                }

         
            }
        }
    }
}

