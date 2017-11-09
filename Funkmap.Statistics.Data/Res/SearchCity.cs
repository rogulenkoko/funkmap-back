using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Statistics.Data.Res
{
    public class SearchCity
    {
        public static string GetNameCity(GeoJsonPoint<GeoJson2DGeographicCoordinates> _geo)
        {
            foreach (var city in Cities)
            {
                if (city.isEquas(_geo))
                    return city._name;
            }
            return "Другое";
        }
        private static List<SearchCity> GetListCities()
        {
            List<SearchCity> cities = new List<SearchCity>();
            cities.Add(new SearchCity("Санкт Петербург",new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(59.945, 30.3717)), 0.16));
            cities.Add(new SearchCity("Минск", new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(53.905313, 27.557182)), 0.15));
            cities.Add(new SearchCity("Москва", new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(55.756519, 37.618660)), 0.24));
            cities.Add(new SearchCity("Киров", new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(58.574381, 49.557386)), 0.17));
            cities.Add(new SearchCity("Зажопинск", new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(53.906383, 30.338138)), 0.16));

            return cities;
        }

        private bool isEquas(GeoJsonPoint<GeoJson2DGeographicCoordinates> city)
        {
            if ((Math.Pow(this._location.Coordinates.Latitude - city.Coordinates.Latitude, 2.0)
                 + Math.Pow(this._location.Coordinates.Longitude - city.Coordinates.Longitude, 2.0)) < this._radius)
                return true;
            return false;
        }
        public static List<SearchCity>Cities = GetListCities();
        
        private string _name;
        private GeoJsonPoint<GeoJson2DGeographicCoordinates> _location;
        private double _radius;
        private SearchCity(string name, GeoJsonPoint<GeoJson2DGeographicCoordinates> location,double radius)
        {
            _name = name;
            _location = location;
            _radius = radius;
        }

        


    }
}
