namespace Domain.Models
{
  public class Lot
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public ICollection<LotImage> Images { get; set; }
  }
}