using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ATP.Web.Resources
{
    [DataContract]
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

        [DataMember]
        public List<ResourceLink> Actions { get; set; }

        public Resource(string resourceBaseUrl)
        {
            ResourceBaseUrl = resourceBaseUrl;
            Actions = new List<ResourceLink>();
        }
    }
}