using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Resources
{
    public class List : Resource
    {
        public string User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PagableSortableList<Place> Places { get; set; }
        public int NumberOfPlaces { get; protected set; }

        public List():base("/lists")
        {
            
        } 
    }
}