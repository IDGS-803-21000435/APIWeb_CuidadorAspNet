using Microsoft.AspNetCore.SignalR;

public class HubHelper : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
