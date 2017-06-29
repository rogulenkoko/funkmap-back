using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;

namespace Funkmap.Data.Domain
{
    public abstract class FullBaseModel
    {
        public string Login { get; set; }
        public string Title { get; set; }
        public byte[] Avatar { get; set; }


        //для музыканта
        public InstrumentType Instrument { get; set; }
        public int Expirience { get; set; }
    }
}
