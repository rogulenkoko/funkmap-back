using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Musician.Data;
using Funkmap.Module.Musician.Models;

namespace Funkmap.Module.Musician.Mappers
{
    public static class SearchModelMapper
    {
        public static SearchModel ToSearchModel(this MusicianEntity musician)
        {
            if (musician == null) return null;
            return new MusicianSearchModel
            {
                Name = musician.Name,
                Latitude = musician.Latitude,
                Longitude = musician.Longitude,
                Id = musician.Id,
                ModelType = ModelType.Musician
            };
        }
    }
}
