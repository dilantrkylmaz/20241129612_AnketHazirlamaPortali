using Int2Uyg.API.Models;
using Int2Uyg.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Int2Uyg.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Int2Uyg.API (Anket Portali)", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Token formatı: Bearer [boşluk] tokeniniz",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
                });
            });

            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddScoped(typeof(GenericRepository<>));
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<SurveyRepository>();
            builder.Services.AddScoped<AnswerRepository>();
            builder.Services.AddScoped<QuestionRepository>();
            builder.Services.AddScoped<QuestionOptionRepository>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    roleManager.CreateAsync(new AppRole { Name = "Admin" }).Wait();
                }
                if (!roleManager.RoleExistsAsync("Uye").Result)
                {
                    roleManager.CreateAsync(new AppRole { Name = "Uye" }).Wait();
                }
                if (userManager.FindByNameAsync("admin").Result == null)
                {
                    var adminUser = new AppUser
                    {
                        UserName = "admin",
                        Email = "admin@sistem.com",
                        FullName = "Sistem Yöneticisi",
                        PhoneNumber = "05550000000",
                        PhotoUrl = "profil.jpg"
                    };

                    var result = userManager.CreateAsync(adminUser, "Admin123!").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                    }
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}