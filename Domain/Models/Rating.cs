namespace Domain.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public double Value { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public bool AsSeller { get; set; }
    }
}