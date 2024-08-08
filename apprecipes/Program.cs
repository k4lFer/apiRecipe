using System.Text.Json.Serialization;
using apprecipes.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace apprecipes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppSettings.Init();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();

            builder.Services.Configure<ApiBehaviorOptions>( options => options.SuppressModelStateInvalidFilter = true );

            builder.Services.AddCors( options =>
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(AppSettings.GetOriginRequest().Split(','))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
                })
            );

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API de recetas culinarias.",
                    Description = "Esta API permite gestionar y acceder a recetas de comida culinaria. Los usuarios pueden crear, leer, actualizar y eliminar recetas, así como buscar recetas por diferentes criterios.",
                    TermsOfService = new Uri("https://github.com/Lionnos"),
                    Contact = new OpenApiContact
                    {
                        Name = "Collaborators",
                        Url = new Uri("https://github.com/k4lFer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licencse",
                        Url = new Uri("http://localhost:5901")
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                {
                    options.SerializeAsV2 = true;
                });
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.UseCors("default");
            app.UseAuthentication();

            app.Run();
        }
    }
}
