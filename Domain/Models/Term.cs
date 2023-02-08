namespace Domain.Models
{
    public class Term
    {
        public VALUE Terms { get; set; }

        public enum VALUE
        {
            One = 1, Three = 3, Seven = 7
        }
    }
}