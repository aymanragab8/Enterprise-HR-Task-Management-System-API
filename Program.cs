using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication2.Models.Data;
using WebApplication2.Models.Entities;
using WebApplication2.Roles;
using WebApplication2.Services;
using WebApplication2.Services.Interfaces;
using WebApplication2.Services.Repositories;


namespace WebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);       // Create a new instance of the WebApplicationBuilder class, which is used to configure and build the web application

            // Add services to the container.

            builder.Services.AddControllers();                      // Add services for controllers to the application, which allows for handling HTTP requests and returning responses in a structured manner
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddDbContext<ApplicationDbContext>(option =>               // Register the ApplicationDbContext with the dependency injection container
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("cs"));       // Configure the ApplicationDbContext to use SQL Server with the connection string specified in the configuration

            }); 

            builder.Services.AddScoped<ApplicationDbContext>();     // Register the ApplicationDbContext as a scoped service, meaning a new instance will be created for each HTTP request

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()     // Add identity services to the application, specifying the ApplicationUser and IdentityRole types for user and role management
                .AddEntityFrameworkStores<ApplicationDbContext>();  // Configure the identity system to use Entity Framework Core with the ApplicationDbContext for storing user and role information


            //*************************************************
            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;        // Set the default authentication scheme to JWT Bearer
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;           // Set the default challenge scheme to JWT Bearer
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;              // Set the default scheme to JWT Bearer
                }).AddJwtBearer(option =>
                {
                    option.SaveToken = true;                            // Save the token in the authentication properties after a successful authentication
                    option.RequireHttpsMetadata = false;                // Disable the requirement for HTTPS metadata, allowing the application to work in development environments without HTTPS
                    option.TokenValidationParameters = new TokenValidationParameters()   // Configure the token validation parameters for JWT authentication
                    {
                        ValidateIssuer = true,                      // Validate the issuer of the token
                        ValidateAudience = true,                    // Validate the audience of the token
                        ValidIssuer = builder.Configuration["JWT:IssuerUrl"],    // Specify the valid issuer for the token
                        ValidAudience = builder.Configuration["JWT:AudienceUrl"],  // Specify the valid audience for the token
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))   // Specify the signing key for validating the token, using a symmetric security key created from a secret string
                    };

                });
            //***********************************************

            builder.Services.AddCors(option =>          // Add CORS services to the application and configure a policy named "MyPolicy"
            {
                option.AddPolicy("MyPolicy",option =>       // Define the CORS policy named "MyPolicy" and specify the allowed origins, methods, and headers
                {
                    option.AllowAnyOrigin()                 // Allow requests from any origin
                          .AllowAnyMethod()                       // Allow any HTTP method (GET, POST, etc.)
                          .AllowAnyHeader();                      // Allow any headers in the request
                });
            });
            // AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            });


            // DI for Services
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();


            builder.Services.AddOpenApi();              // Add OpenAPI services to the application, which allows for generating API documentation and providing an interactive API explorer
            builder.Services.AddEndpointsApiExplorer();     // Add services for API endpoint exploration, which allows for discovering and documenting API endpoints in the application
            builder.Services.AddSwaggerGen();           // Add services for generating Swagger documentation, which provides a way to describe and document the API endpoints in a standardized format

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                IdentitySeeder.SeedRolesAsync(roleManager).GetAwaiter().GetResult();
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())        // Check if the application is running in the development environment
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();                  // Redirect HTTP requests to HTTPS

            app.UseCors("MyPolicy");                    // Apply the CORS policy to the application

            app.UseAuthorization();                     // Enable authorization middleware

            app.MapControllers();                       // Map controller routes to the application

            app.Run();
        }
    }
}
