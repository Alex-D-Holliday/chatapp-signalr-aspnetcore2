using System;
using System.Collections.Generic;

namespace ChatApp.Models
{
    public class ChatEntity
    {
        public ChatEntity()
        {
            Messages = new List<MessageEntity>();
            Users = new List<ChatUser>();
        }

        public Int32 Id { get; set; }
        public String Name { get; set; }
        public ChatType Type { get; set; }
        public ICollection<MessageEntity> Messages { get; set; }
        public ICollection<ChatUser> Users { get; set; }
    }
}