using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Search;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Module.Musician.Models
{
    public class MusicianSearchModel : SearchModel
    {
        public InstrumentType Instrument { get; set; }
    }
}
