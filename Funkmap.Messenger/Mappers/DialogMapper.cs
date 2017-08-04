using System.Collections.Generic;
using System.Linq;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Mappers
{
    public static class DialogMapper
    {
        public static Dialog ToModel(this DialogEntity source, string userLogin)
        {
            if (source == null) return null;
            return new Dialog()
            {
                DialogId = source.Id.ToString(),
                Name = source.Participants.FirstOrDefault(x => x != userLogin)
            };
        }
    }
}
