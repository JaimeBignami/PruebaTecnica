using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PruebaTecnica.Application;
using PruebaTecnica.Infrastructure;
using PruebaTecnica.Infrastructure.Persistence;

namespace PruebaTecnica.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            // Configuración DB
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Controladores
            services.AddControllers();

            services.AddHttpClient();

            // Swagger
            ConfigureSwagger(services);

            // Otros servicios que puedes agregar más adelante:
            services.AddApplication();
            services.AddInfrastructure();
            
            // - servicios.AddDbContext<AppDbContext>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Activar Swagger en entorno dev
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prueba Tecnica API v1");
                    c.RoutePrefix = string.Empty; // Esto expone Swagger en la raíz "/"
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Prueba Técnica API",
                    Version = "v1",
                    Description = "API para prueba técnica L3 - Procesamiento de transacciones"
                });

                // Aquí podrías agregar autenticación JWT o anotaciones si lo necesitas
                // c.EnableAnnotations();
            });
        }
    }
}