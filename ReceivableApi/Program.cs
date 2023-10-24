using ReceivableApi.Data;
using Microsoft.EntityFrameworkCore;
using ReceivableApi.Validators;

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
            builder.Services.AddScoped<Func<DateTime>>(s => () => DateTime.Now);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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