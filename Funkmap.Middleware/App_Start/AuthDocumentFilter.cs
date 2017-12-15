using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Funkmap.Middleware.App_Start
{
    public class AuthDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {

            var pathItem = new PathItem()
            {
                post = new Operation()
                {
                    tags = new List<string> { "Auth" },
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    parameters = new List<Parameter>()
                    {
                        new Parameter() {type = nameof(String), name = "grant_type", required = true, @in = "formData", @default = "password"},
                        new Parameter() {type = nameof(String), name = "username", required = true, @in = "formData"},
                        new Parameter() {type = nameof(String), name = "password", required = true, @in = "formData"}
                    }
                }
            };

            swaggerDoc.paths.Add("/api/token", pathItem);
        }
    }
}
