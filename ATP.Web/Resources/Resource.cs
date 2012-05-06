using System.Collections.Generic;

namespace ATP.Web.Resources
{
    public class Resource
    {
        public int Id { get; set; }
        public string Uri
        {
            get
            {
                return "/users/" + Id;
            }
        }

        public List<ResourceLink> Actions { get; set; }

        public Resource()
        {
            Actions = new List<ResourceLink>();
        }
    }
}