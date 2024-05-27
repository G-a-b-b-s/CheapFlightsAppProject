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

    public bool createUser(string login, string pass) {
        try {
            connection.Execute($"INSERT INTO users VALUES('{login}', '{pass}', 0)");
            return true;
        }
        catch (SqliteException e) {
            Console.WriteLine(e.Message + " " + $"INSERT INTO users VALUES('{login}', '{pass}', 0)");
            return false;
        }
    }

    public List<Flight> searchFlights(string dep, string dest, string date) {
        // Console.WriteLine($"SELECT * FROM flights " +
        //                   $"WHERE departure='{dep}'" +
        //                   $"AND destination='{dest}'" +
        //                   $"AND date like '{date}%'"   
        // );        
        Console.WriteLine(date);
        List <Flight> flight =
            connection.Query<Flight>($"SELECT * FROM flights " +
                                     $"WHERE departure like '{dep}%'" +
                                     $"AND destination like '{dest}%'"+
                                     $"AND date like '{date}%'"
            ).ToList();
        return flight;
    }
}