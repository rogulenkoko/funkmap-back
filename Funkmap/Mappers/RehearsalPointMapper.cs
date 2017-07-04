using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class RehearsalPointMapper
    {
        public static RehearsalPointPreviewModel ToPreviewModel(this RehearsalPointEntity source)
        {
            if (source == null) return null;
            return new RehearsalPointPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                Avatar = source.Photo.AsByteArray,
                VkLink = source.VkLink,
                YouTubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription
            };
        }
    }
}
