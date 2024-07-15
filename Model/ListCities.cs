using BarLight.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarLight.Model
{
    internal class ListCities : List<String>
    {
        public ListCities()
        {
            List<City> cities = Manager.Context.Cities.ToList();
            foreach (City city in cities)
                Add(city.Name);
        }
    }
}
