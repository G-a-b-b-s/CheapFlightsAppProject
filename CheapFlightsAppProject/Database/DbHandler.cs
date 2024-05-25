namespace CheapFlightsAppProject.Database;
using Dapper;
using Microsoft.Data.Sqlite;

public class DbHandler {
    public SqliteConnection connection;
    public DbHandler() {
        connection = new SqliteConnection("Data Source=/home/bartosz/Desktop/pz2/projekt/CheapFlightsAppProject/Database/data.db");
    }

    public bool checkCredentials(string login, string pass) {
        try {
            User u = connection.Query<User>($"SELECT * FROM users WHERE username='{login}'").SingleOrDefault();
            return u.password == pass;
        }
        catch (NullReferenceException){
            return false;
        }
    }
}