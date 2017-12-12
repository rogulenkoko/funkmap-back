using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Funkmap.Middleware
{
    public class SwaggerConfig
    {
        public static void SetCommentsPath(SwaggerDocsConfig swaggerDocsConfig)
        {
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(executablePath)) throw new InvalidOperationException("Executable path is not set.");

            List<Assembly> list = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap"))
                .ToList();

            foreach (var assembly in list)
            {
                string filePath = $"{executablePath}\\{assembly.GetName().Name}.XML";
                if (!File.Exists(filePath)) continue;

                swaggerDocsConfig.IncludeXmlComments(filePath);
            }
        }
    }
}
