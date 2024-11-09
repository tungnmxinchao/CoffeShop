using System;
using System.Collections.Generic;

namespace CoffeShop.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int TableId { get; set; }
        public int UserId { get; set; }
        public string FeedbackOption { get; set; } = null!;
        public string? FeedbackContent { get; set; }
        public string Status { get; set; } = null!;
        public string Cause { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Table Table { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
