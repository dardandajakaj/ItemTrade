using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Text.Json;
using API.Entity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {    
        public static async Task Seeder(IApplicationBuilder builder){
            using (var scope = builder.ApplicationServices.CreateScope()){
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.EnsureCreated();

                if(!await context.Users.AnyAsync()){
                    var usersJSON = await File.ReadAllTextAsync("Data/UserSeed.json");
                    var users = JsonSerializer.Deserialize<List<User>>(usersJSON);
                    Console.WriteLine(users);
                    foreach (var user in users)
                    {
                        using var hmac = new HMACSHA512();
                        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("12345678"));
                        user.PasswordSalt = hmac.Key;
                        context.Users.Add(user);
                    }
                    await context.SaveChangesAsync();
                }

                if(!await context.Categories.AnyAsync()){
                    var categoriesJSON = await File.ReadAllTextAsync("Data/CategorySeed.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesJSON);
                    foreach (var category in categories)
                    {
                        context.Categories.Add(category);
                    }
                    await context.SaveChangesAsync();
                }
               

                if(!await context.Products.AnyAsync()){
                    var productsJSON = await File.ReadAllTextAsync("Data/ProductSeed.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsJSON);
                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }
                    await context.SaveChangesAsync();
                }                
            }
        }
    }
}