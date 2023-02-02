﻿namespace Domain.Models
{
    /// <summary>
    /// Favorites products of user (want to see later)
    /// </summary>
    public class Favorite
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int LotId { get; set; }
        public Lot Lot { get; set; }
    }
}