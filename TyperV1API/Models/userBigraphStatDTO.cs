namespace TyperV1API.Models
{
    public class userBigraphStatDTO
    {
        public int? UserId { get; set; }
        public string Bigraph { get; set; } = null!;
        public decimal? Speed { get; set; }
        public int corrCount { get; set; }
        public decimal? Accuracy { get; set; }
        public int? Quantity { get; set; }
    }
}
