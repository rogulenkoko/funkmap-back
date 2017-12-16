using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Models.Requests;
using Funkmap.Tools.Abstract;

namespace Funkmap.Tools
{
    public class ParameterFactory : IParameterFactory
    {
        public IFilterParameter CreateParameter(FilteredRequest request)
        {
            switch (request.EntityType)
            {
                case EntityType.Musician:
                    return new MusicianFilterParameter()
                    {
                        Styles = request.Styles,
                        Expirience = request.Expirience,
                        Instruments = request.Instruments
                    };

                case EntityType.Band:
                    return new BandFilterParameter()
                    {
                        Styles = request.Styles
                    };
                default: return null;
            }
        }
    }
}
