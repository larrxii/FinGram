﻿namespace FinGram.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int? ToUserId { get; set; }  
        public string Text { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsExpertReply { get; set; }
    }
}
