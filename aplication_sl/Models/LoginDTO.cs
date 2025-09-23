namespace api_sl.Models
{
    public class LoginDTO
    {
        public string CompanyDB { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int Language { get; set; }
        public string Url { get; set; }
        public LoginDTO(string _CompanyDB, string _Password, string _UserName)
        {
            CompanyDB = _CompanyDB;
            Password = _Password;
            UserName = _UserName;
        }
        public LoginDTO()
        { }
    }
}
