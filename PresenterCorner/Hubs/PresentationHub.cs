using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PresenterCorner.Data;
using PresenterCorner.Models;
using System;

namespace PresenterCorner.Hubs
{
   public class PresentationHub : Hub
   {
        private readonly ApplicationDbContext _context;

        public PresentationHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = Context.GetHttpContext().Request.Query["userId"];

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user != null)
            {
                user.ConnectionId = connectionId;
                await _context.SaveChangesAsync();
            }
            // Optionally handle user connection logic
            await base.OnConnectedAsync();
        }
        // Notify all clients in a presentation about an update
        public async Task SendUpdate(string presentationId, object update, string role)
        {
            if (role == "Viewer")
            {
                throw new HubException("Viewers cannot send updates.");
            }
            await Clients.Group(presentationId).SendAsync("ReceiveUpdate", update);

        }
        // Add a user to a presentation group
        public async Task JoinPresentation(string presentationId, string nickname)
        {
            var user = _context.Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user == null)
            {
                user = new User
                {
                    ConnectionId = Context.ConnectionId,
                    Nickname = nickname,
                    Role = "Viewer" // Default role
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, presentationId);
            await Clients.Group(presentationId).SendAsync("UserJoined", user);

        }
        // Remove a user from a presentation group
        public async Task LeavePresentation(string presentationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, presentationId);
        }

        public async Task AssignRole(string connectionId, string role)
        {
            var user = _context.Users.FirstOrDefault(u => u.ConnectionId == connectionId);
            if (user != null)
            {
                user.Role = role;
                await _context.SaveChangesAsync();
            }
            await Clients.Client(connectionId).SendAsync("RoleAssigned", role);
        }
    }

}
