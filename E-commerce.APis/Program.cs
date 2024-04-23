using CoreLayer.Entites;
using CoreLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer;
using RepositoryLayer.Data;

namespace E_commerce.APis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            #region Update-Database
            //StoreContext dbcontext = new StoreContext();
            //await dbcontext.Database.MigrateAsync();

            using var Scope = app.Services.CreateScope();
            // Group Of Services LifeTime Scooped
            var Services = Scope.ServiceProvider;
            // Services It Self
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {

                var DbContext = Services.GetRequiredService<StoreContext>();
                // Ask CLR For Creating Object From DbContext Explicitly
                await DbContext.Database.MigrateAsync(); // Update-Database
                await StoreContextSeed.SeedAsync(DbContext); // data seeding
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Appling The Migration");
            }


            #endregion



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
