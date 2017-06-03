using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Musician.Data.Parameters
{
    public class MusicianParameter
    {
        public ICollection<Styles> Styles { get; set; }
    }
}
