﻿namespace Bll.Models
{
    public class CreateLotInput
    {
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public double Price { get; set; }

        public int MinimalBid { get; set; }
        public DateTime CloseTime { get; set; }

        public string UserId { get; set; }
        public int? CategoryId { get; set; }

        public string? NewCategoryName { get; set; }
    }
}