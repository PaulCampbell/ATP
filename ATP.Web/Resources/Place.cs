using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Resources
{
    public class Place : Resource
    {
        public string Name { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Description { get; set; }
        public List<String> Pictures { get; set; } 

        public Place():base("/places/")
        {
            
        }
    }
}