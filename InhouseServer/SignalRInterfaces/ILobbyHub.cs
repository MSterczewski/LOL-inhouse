namespace SignalRInterfaces
{
    public interface ILobbyHub
    {
        Task RefreshLobby();
        Task MatchReady();
    }
}
