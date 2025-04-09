using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class ProductHub : Hub
    {
        public async Task SendProductUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveProductUpdate", message);
        }
    }
}
