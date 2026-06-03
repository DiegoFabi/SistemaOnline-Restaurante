namespace SistemaOnline.ViewModels
{
    public class UserVM
    {
        public int idUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public int idRol { get; set; }
    }
}
