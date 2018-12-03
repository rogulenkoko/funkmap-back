using Microsoft.AspNetCore.Builder;

namespace Funkmap.Common.Core.Tools
{
    public static class CommonApplicationConfigurator
    {
        public static IApplicationBuilder UseFunkmap(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Funkmap feedback API");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}