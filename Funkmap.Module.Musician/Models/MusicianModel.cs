using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Module.Musician.Models
{
    public class MusicianModel
    {
        public string Login { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public int Expirience { get; set; }
        public Styles[] Styles { get; set; }


    }
}
