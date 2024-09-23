using Microsoft.AspNetCore.SignalR;

public class UserHub : Hub
{
    public async Task SendUserUpdate(string userId)
    {
        await Clients.All.SendAsync("ReceiveUserUpdate", userId);
    }
    
    public async Task SendUserDelete(string userId)
    {
        await Clients.All.SendAsync("ReceiveUserDelete", userId);
    }
    
}
