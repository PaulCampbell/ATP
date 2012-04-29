namespace ATP.Web.Resources
{
    public class User : Resource
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
    }
}