using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDTOs;
using WebDTOs.Lobby;

namespace Interfaces
{
    public interface ILobbyDataService
    {
        Task<GetLobbyResponseDTO> GetLobby();
        Task<EJoinLobbyError?> JoinLobby(JoinLobbyRequestDTO model);
        Task<bool> LeaveLobby(LeaveLobbyRequestDTO model);
    }
}
