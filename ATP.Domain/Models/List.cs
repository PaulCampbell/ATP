using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models
{
    public class List : Entity
    {
        public string UserId { get; set; }
        public string ListName { get; set; }
        public List<Place> Places { get; set; }
    }
}
