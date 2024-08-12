using System.Text;
using System.Text.Json.Serialization;
using apprecipes.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace apprecipes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string myAllowSpecificOrigins = "AllowOnlyDefaults";
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            #region appsettings
            AppSettings.Init();
            #endregion
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            #region CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(myAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins(AppSettings.GetOriginRequest().Split(','))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowAnyOrigin()
                            .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });
            #endregion
            
            #region JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppSettings.GetOriginIssuer(),
                    ValidAudience = AppSettings.GetOriginAudience(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.GetAccessJwtSecret())),
                    ClockSkew = TimeSpan.Zero,
                };
            });
            builder.Services.AddAuthorization();
            #endregion
            
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            #region authentication to Swagger UI
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
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
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            #endregion

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
            app.UseCors(myAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
