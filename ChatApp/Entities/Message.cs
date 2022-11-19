﻿using System.ComponentModel.DataAnnotations;

namespace ChatApp.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
