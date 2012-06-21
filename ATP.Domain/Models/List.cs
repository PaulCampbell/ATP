using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models
{
    public class List : Entity
    {
        public string User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Places { get; protected set; }
        public int NumberOfPlaces { get; protected set; }

        public List()
        {
            Places = new List<string>();
            NumberOfPlaces = 0;
        }

        public void AddPlace(Place place)
        {
            if (!Places.Any(p => p == "/places/" + place.Id))
            {
                Places.Add("/places/" + place.Id);
                NumberOfPlaces++;
            }
        }

    }
}
