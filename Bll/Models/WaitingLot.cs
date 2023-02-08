namespace Bll.Models
{
    public class WaitingLot
    {
        public WaitingLot(int id, DateTime closeTime)
        {
            Id = id;
            CloseTime = closeTime;
        }

        public int Id { get; set; }
        public DateTime CloseTime { get; set; }
    }
}