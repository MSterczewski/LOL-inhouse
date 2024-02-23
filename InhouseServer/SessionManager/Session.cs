using Models;

namespace SessionManager
{
    public static class Session
    {
        private static readonly Dictionary<int, string> Tokens = [];
        private static readonly Dictionary<int, ERole> Roles = [];

        public static bool IsValid(int playerId, string token)
        {
            return Tokens.TryGetValue(playerId, out var existingToken) && existingToken == token;
        }

        public static void Logout(int playerId)
        {
            Tokens.Remove(playerId);
        }

        public static string Login(int playerId, ERole role)
        {
            var token = Guid.NewGuid().ToString();
            Tokens.Remove(playerId);
            Roles.Remove(playerId);
            Tokens.Add(playerId, token);
            Roles.Add(playerId, role);
            return token;
        }

        public static bool IsAdmin(int playerId)
        {
            if (Roles.TryGetValue(playerId, out var role))
                return role == ERole.Admin;
            return false;
        }
    }
}
