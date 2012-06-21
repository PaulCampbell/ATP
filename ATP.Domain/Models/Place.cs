using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models
{
    public class Place : Entity
    {
        public string Name { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Description { get; set; }
        public List<String> Pictures { get; set; }
        public string List { get; set; }
    }
}
