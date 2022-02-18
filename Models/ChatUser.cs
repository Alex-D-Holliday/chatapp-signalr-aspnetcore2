using System;

namespace ChatApp.Models
{
    public class ChatUser
    {
        public String UserId { get; set; }
        public User User { get; set; }
        public Int32 ChatId { get; set; }
        public ChatEntity Chat { get; set; }
        public UserRole Role { get; set; }
    }
}