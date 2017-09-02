using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Settings;

namespace Funkmap.Middleware.Settings
{
    public class MonolithSettingsService : ISettingsService
    {
        
        public ISettings GetSettings()
        {
            return new MonolithSettings();
        }
    }
}
