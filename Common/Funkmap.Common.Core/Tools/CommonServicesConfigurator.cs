using System;
using System.IO;
using System.Linq;
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
        public static void AddFunkmap(this IServiceCollection services, IConfiguration config, params string[] assemblies)
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

                if (assemblies.Any())
                {
                    foreach (var assembly in assemblies)
                    {
                        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly}.xml");
                        options.IncludeXmlComments(xmlPath);
                    }
                }
                else
                {
                    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name.Replace(".Web", "")}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                }
                
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