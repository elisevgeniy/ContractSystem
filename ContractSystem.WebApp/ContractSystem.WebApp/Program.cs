using Blazored.Toast;
using ContractSystem.Core;
using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models;
using ContractSystem.Repositories;
using ContractSystem.Service;
using ContractSystem.WebApp.Client.Pages;
using ContractSystem.WebApp.Components;
using ContractSystem.WebApp.Components.Models;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ContractSystem.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    options =>
                    {
                        options.Cookie.Name = "auth_token";
                        options.LoginPath = "/login";
                        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                    });

            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();



            TypeAdapterConfig.GlobalSettings.Apply(new MapsterConfig());
            TypeAdapterConfig.GlobalSettings.NewConfig<ClaimsPrincipal, AuthedUser>()
                .Map(au => au.Id, cp => cp.Claims.Where(cl => cl.Type.Equals(ClaimTypes.Sid)).First().Value)
                .Map(au => au.Name, cp => cp.Claims.Where(cl => cl.Type.Equals(ClaimTypes.Name)).First().Value)
                .Map(au => au.Role, cp => Enum.Parse<Role>(cp.Claims.Where(cl => cl.Type.Equals(ClaimTypes.Role)).First().Value))
                ;
            builder.Services.AddMapster();

            builder.Services.AddDbContext<DataContext>();
            
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
            builder.Services.AddScoped<IApprovalRepository, ApprovalRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<DocumentService>();
            builder.Services.AddScoped<ApprovalService>();




           var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
