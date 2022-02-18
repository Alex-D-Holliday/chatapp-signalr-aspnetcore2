using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp.Models;

namespace ChatApp.Infrastructure.Respository
{
    public interface IChatService
    {
        ChatEntity GetChat(Int32 id);
        Task CreateRoom(String name, String userId);
        Task JoinRoom(Int32 chatId, String userId);
        IEnumerable<ChatEntity> GetChats(String userId);
        Task<Int32> CreatePrivateRoom(String rootId, String targetId);
        IEnumerable<ChatEntity> GetPrivateChats(String userId);

        Task<MessageEntity> CreateMessage(Int32 chatId, String message, String userId);
    }
}