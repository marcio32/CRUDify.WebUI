using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class ServiceHub : Hub
    {
        public async Task SendServiceUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveServiceUpdate", message);
        }
    }
}
