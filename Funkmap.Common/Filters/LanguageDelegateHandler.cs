using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Funkmap.Common.Filters
{
    public class LanguageDelegateHandler : DelegatingHandler
    {
        private readonly Dictionary<string, string> _locales;
        private readonly string _defaultLanguage;

        public LanguageDelegateHandler()
        {
            _defaultLanguage = "en";

            _locales = new Dictionary<string, string>
            {
                { "ru", "ru" },
                { "en", "en" }
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var langaugeHeader = request.Headers.AcceptLanguage.FirstOrDefault();

            var lang = langaugeHeader?.Value == null || !_locales.ContainsKey(langaugeHeader.Value) ? _defaultLanguage : langaugeHeader.Value;

            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            return base.SendAsync(request, cancellationToken);
        }
    }
}
