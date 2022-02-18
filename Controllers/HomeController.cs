using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp.Database;
using ChatApp.Hubs;
using ChatApp.Infrastructure;
using ChatApp.Infrastructure.Respository;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IChatService _repo;
        public HomeController(IChatService repo) => _repo = repo;
        public IActionResult Index()
        {
            IEnumerable<ChatEntity> chats = _repo.GetChats(GetUserId());

            return View(chats);
        }

        public IActionResult Find([FromServices] AppDbContext ctx)
        {
            List<User> users = ctx.Users
                .Where(x => x.Id != User.GetUserId())
                .ToList();

            return View(users);
        }

        public IActionResult Private()
        {
            IEnumerable<ChatEntity> chats = _repo.GetPrivateChats(GetUserId());

            return View(chats);
        }

        public async Task<IActionResult> CreatePrivateRoom(String userId)
        {
            Int32 id = await _repo.CreatePrivateRoom(GetUserId(), userId);

            return RedirectToAction("Chat", new { id });
        }

        [HttpGet("{id}")]
        public IActionResult Chat(Int32 id)
        {
            return View(_repo.GetChat(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(String name)
        {
            await _repo.CreateRoom(name, GetUserId());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(Int32 id)
        {
            await _repo.JoinRoom(id, GetUserId());

            return RedirectToAction("Chat", "Home", new { id = id });
        }

        public async Task<IActionResult> SendMessage(
            Int32 roomId,
            String message,
            [FromServices] IHubContext<ChatHub> chat)
        {
            MessageEntity Message = await _repo.CreateMessage(roomId, message, User.Identity.Name);

            await chat.Clients.Group(roomId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });

            return Ok();
        }
    }
}