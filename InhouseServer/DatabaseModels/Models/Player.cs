namespace DatabaseModels
{
    public class Player(string nickname, string password) : DatabaseModel
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = nickname;
        public string Password { get; set; } = password;
        public int Rank { get; set; } = 0;
        public int Top { get; set; } = 3;
        public int Jng { get; set; } = 3;
        public int Mid { get; set; } = 3;
        public int Bot { get; set; } = 3;
        public int Supp { get; set; } = 3;
        public int Role { get; set; } = 0;
        public string ImageUrl { get; set; } =
            "https://uxwing.com/wp-content/themes/uxwing/download/peoples-avatars/male-icon.png";
    }
}
