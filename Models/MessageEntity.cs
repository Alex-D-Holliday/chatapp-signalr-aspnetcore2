using System;

namespace ChatApp.Models
{
    public class MessageEntity
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Text { get; set; }
        public DateTime Timestamp { get; set; }

        public Int32 ChatId { get; set; }
        public ChatEntity Chat { get; set; }
    }
}