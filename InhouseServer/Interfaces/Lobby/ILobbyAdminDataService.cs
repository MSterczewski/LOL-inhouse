using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDTOs;
using WebDTOs.LobbyAdmin;

namespace Interfaces
{
    public interface ILobbyAdminDataService
    {
        Task ResetLobby();
        Task<bool> RemovePlayer(RemovePlayerDTO model);
        Task FillWithBots();
        Task ReviewLobby();
    }
}
