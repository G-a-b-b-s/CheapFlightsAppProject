namespace CheapFlightsAppProject.Database;

public class User {
    public User(){}
    public User(string name, string surname, string login, string password, int role) {
        this.username = login;
        this.password = password;
        this.role = role;
        this.Name = name;
        this.Surname = surname;
    }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public int role { get; set; }
}