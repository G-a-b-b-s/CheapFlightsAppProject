namespace CheapFlightsAppProject.Database;

public class User {
    public User(){}
    public User(string login, string password, int role) {
        this.login = login;
        this.password = password;
        this.role = role;
    }
    public string login { get; set; }
    public string password { get; set; }
    public int role { get; set; }
}