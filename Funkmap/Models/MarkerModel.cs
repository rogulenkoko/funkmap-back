using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;

namespace Funkmap.Models
{
    public class MarkerModel
    {
        public string Login { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public EntityType ModelType { get; set; }


        public InstrumentType Instrument;
    }
}
