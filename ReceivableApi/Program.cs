using ReceivableApi.Data;
using Microsoft.EntityFrameworkCore;
using ReceivableApi.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace ReceivableApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ReceivableApiContext>(options =>
                options.UseSqlite("Data Source=ReceivablesDatabase.db"));

            // Add services to the container.
            builder.Services.AddScoped<IJsonFileLoader, JsonFileLoader>();
            builder.Services.AddScoped<CountryLoader>();
            builder.Services.AddScoped<CurrencyLoader>();
            builder.Services.AddScoped<AddReceivableValidator>();
            builder.Services.AddScoped<ReceivableManager>();
            builder.Services.AddScoped<CurrencyConverter>();
            builder.Services.AddScoped<Func<DateTime>>(s => () => DateTime.Now);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Ensure database is created and up to date
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ReceivableApiContext>();
                context.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}