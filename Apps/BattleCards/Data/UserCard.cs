﻿using System.ComponentModel.DataAnnotations;

namespace BattleCards.Data
{
    public class UserCard
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int CardId { get; set; }
        public virtual Card Card { get; set; }
    }
}
