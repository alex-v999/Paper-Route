namespace Paper_Route.User
{
    public class UserProfile
    {
        public int Id { get; set; } // <- primary key
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

}