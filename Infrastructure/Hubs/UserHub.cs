using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendUserUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveUserUpdate", message);
        }
    }
}
