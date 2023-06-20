using DataProj.Api.Logs;
using DataProj.DAL.Repository.Implemintations;
using DataProj.DAL.Repository.Interfaces;
using DataProj.DAL.SqlServer;
using DataProj.Domain.Models.Abstractions.BaseUsers;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Services.Implementations;
using DataProj.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DataProj.API
{
    public static class Initializer
    {
        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            #region Base_Repositories 
            services.AddScoped(typeof(IBaseAsyncRepository<>), typeof(BaseAsyncRepository<>));
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            #endregion
            return services;
        }

        public static IServiceCollection InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager<Employee>, AuthManager<Employee>>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskItemService, TaskItemService>();

            return services;

        }

        public static IServiceCollection InitializeIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<RoleManager<IdentityRole>>();

            services.AddScoped<IAuthManager<Employee>>(provider =>
            {
                var userManager = provider.GetRequiredService<UserManager<Employee>>();
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                return new AuthManager<Employee>(userManager, roleManager, configuration);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppDbContext>()
                 .AddDefaultTokenProviders();

            services.AddScoped<IUserStore<Employee>, UserStore<Employee, IdentityRole, AppDbContext, string>>();
            services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

            return services;
        }

        public static async Task InitializeRoles(this IServiceCollection services)
        {
            var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
            await SeedRoles(roleManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>())
            {
                var roleName = role.ToString();

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async static Task SeedAdmins(this IServiceCollection services)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<Employee>>();
            const string adminId = "1fd2c92f-1722-4b69-87db-f877b2656307";
            const string adminName = "Ayub";

            var user = await userManager.FindByIdAsync(adminId);

            if (user != null) return;

            var admin = new Employee()
            {
                Id = adminId,
                FirstName = "Admin",
                LastName = "Admin",
                MiddleName = "Admin",             
                Email = "admin@admin.com",
                UserName = adminName,
                NormalizedUserName = "Ayub".Normalize(),
                NormalizedEmail = "admin@admin.com".Normalize(),
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "P@ssw0rd!");


            await userManager.CreateAsync(admin);
            await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
        }


        public static void IntialiseLogger(this ILoggingBuilder loggingBuilder, Action<DbLoggerOptions> configure)
        {
            loggingBuilder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
            loggingBuilder.Services.Configure(configure);
        }

    }
}
