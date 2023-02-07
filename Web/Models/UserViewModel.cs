namespace Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsBannedAsBuyer { get; set; }
        public bool IsBannedAsSeller { get; set; }
        public bool IsAdmin { get; set; }
    }
}
