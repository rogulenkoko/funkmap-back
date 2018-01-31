using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Funkmap.Common.Filters
{
    public class LanguageFilterAttribute : ActionFilterAttribute
    {

        private readonly Dictionary<string, string> _locales;
        private readonly string _defaultLanguage;

        public LanguageFilterAttribute()
        {
            _defaultLanguage = "en";

            _locales = new Dictionary<string, string>()
            {
                { "ru", "ru" },
                { "en", "en" }
            };
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var langaugeHeader = actionContext.Request.Headers.AcceptLanguage.FirstOrDefault();

            var lang = langaugeHeader?.Value ?? _defaultLanguage;

            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

        }
    }
}
