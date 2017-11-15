using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WheelOfFortune.Additionals
{
    public class UserBalanceUpdated : Hub
    {
        public async Task Send(/*string nick, string message*/)
        {
            await Clients.All.InvokeAsync("Send"/*, nick, message*/);
        }
    }
}
