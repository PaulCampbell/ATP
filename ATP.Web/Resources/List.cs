using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Resources
{
    public class List : Resource
    {
        public string User { get; set; }
        public string ListName { get; set; }
        public PagableSortableList<Place> Places { get; set; }  
    }
}