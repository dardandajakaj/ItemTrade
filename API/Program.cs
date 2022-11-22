using API.Extensions;
using API.Data;
using API.Middlewares;
using System.Text.Json.Serialization;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        // Add services to the container.
        builder.Services.AddApplicationService(builder.Configuration);
        builder.Services.AddControllers().
                AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddCors();
       
        builder.Services.AddIdentityServices(builder.Configuration);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        await Seed.Seeder(app);
        app.Run();
    }
}