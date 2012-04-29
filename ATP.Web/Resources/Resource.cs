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
    }
}