using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class RoleHub : Hub
    {
        public async Task SendRoleUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveRoleUpdate", message);
        }
    }
}
