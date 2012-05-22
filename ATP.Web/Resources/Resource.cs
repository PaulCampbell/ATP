using System.Collections.Generic;

namespace ATP.Web.Resources
{
    public class Resource
    {
        public string ResourceBaseUrl { get; private set; }

        public int Id { get; set; }
        public string Uri
        {
            get
            {
                return "/resource-type/" + Id;
            }
        }

        public List<ResourceLink> Actions { get; set; }

        public Resource(string resourceBaseUrl)
        {
            ResourceBaseUrl = resourceBaseUrl;
            Actions = new List<ResourceLink>();
        }
    }
}