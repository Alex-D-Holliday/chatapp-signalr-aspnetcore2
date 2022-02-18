using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Respository
{
    public class ChatService : IChatService
    {
        private AppDbContext _ctx;

        public ChatService(AppDbContext ctx) => _ctx = ctx;

        public async Task<MessageEntity> CreateMessage(Int32 chatId, String message, String userId)
        {
            MessageEntity Message = new MessageEntity
            {
                ChatId = chatId,
                Text = message,
                Name = userId,
                Timestamp = DateTime.Now
            };

            _ctx.Messages.Add(Message);
            await _ctx.SaveChangesAsync();

            return Message;
        }

        public async Task<Int32> CreatePrivateRoom(String rootId, String targetId)
        {
            ChatEntity chat = new ChatEntity
            {
                Type = ChatType.Private
            };

            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();

            return chat.Id;
        }

        public async Task CreateRoom(String name, String userId)
        {
            ChatEntity chat = new ChatEntity
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();
        }

        public ChatEntity GetChat(Int32 id)
        {
            return _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ChatEntity> GetChats(String userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }

        public IEnumerable<ChatEntity> GetPrivateChats(String userId)
        {
            return _ctx.Chats
                   .Include(x => x.Users)
                       .ThenInclude(x => x.User)
                   .Where(x => x.Type == ChatType.Private
                       && x.Users
                           .Any(y => y.UserId == userId))
                   .ToList();
        }

        public async Task JoinRoom(Int32 chatId, String userId)
        {
            ChatUser chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            _ctx.ChatUsers.Add(chatUser);

            await _ctx.SaveChangesAsync();
        }
    }
}