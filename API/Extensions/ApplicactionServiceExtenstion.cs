using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicactionServiceExtenstion 
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenServiceProvider, TokenServiceProvider>();
            services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));  
                });
            return services;
        }
    }
}