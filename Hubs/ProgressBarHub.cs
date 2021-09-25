using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Tweetly_MVC.Hubs
{
    public class ProgressBarHub : Hub
    {
        public async Task ProgressBar(int currentValue)
        {
           await Clients.All.SendAsync("progressCalistir", currentValue);
        }
    }
}
