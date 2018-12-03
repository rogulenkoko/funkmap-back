using System;
using System.IO;
using System.Reflection;
using Funkmap.Common.Core.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Funkmap.Common.Core.Tools
{
    public static class CommonServicesConfigurator
    {
        public static void AddFunkmap(this IServiceCollection services, IConfiguration config)
        {
            var authOptions = new FunkmapJwtOptions(config);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidAudience = authOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(authOptions.Key))
                    };
                });
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Funkmap API", Version = "v1" });

                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name.Replace(".Web","")}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                
                options.AddSecurityDefinition("Bearer", new OAuth2Scheme
                {
                    Flow = "password",
                    TokenUrl = authOptions.TokenUrl,
                });

                options.DocumentFilter<OAuthDocumentFilter>();
            });
        }
    }
}