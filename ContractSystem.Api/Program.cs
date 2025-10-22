
using ContractSystem.Core;
using ContractSystem.Core.IRepositories;
using ContractSystem.Repositories;
using ContractSystem.Service;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace ContractSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Для избежания циклического преобразования в json
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //});
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Авторизация
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "SecrerIssuer",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretIssuerKeySecretIssuerKeySecretIssuerKey")),
                        ValidAudience = "App"                                           
                    };
                });


            TypeAdapterConfig.GlobalSettings.Apply(new MapsterConfig());
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
