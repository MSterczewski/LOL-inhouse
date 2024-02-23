using Microsoft.AspNetCore.SignalR;
using SignalRInterfaces;

namespace InhouseServer.Hubs
{
    public interface ILobbyHubClient
    {
        Task RefreshLobby();
        Task MatchReady();
    }

    public class LobbyHub(IHubContext<LobbyHub, ILobbyHubClient> hubContext)
        : Hub<ILobbyHubClient>,
            ILobbyHub
    {
        private readonly IHubContext<LobbyHub, ILobbyHubClient> _hubContext = hubContext;

        public async Task RefreshLobby()
        {
            await _hubContext.Clients.All.RefreshLobby();
        }

        public async Task MatchReady()
        {
            await _hubContext.Clients.All.MatchReady();
        }
    }
}
