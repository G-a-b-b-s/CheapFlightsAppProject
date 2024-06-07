namespace CheapFlightsAppProject.Database;
using Dapper;
using Microsoft.Data.Sqlite;

public class DbHandler {
    public SqliteConnection Connection;
    public DbHandler() {
        Connection = new SqliteConnection("Data Source=../CheapFlightsAppProject/Database/data.db");
    }

    public bool CheckCredentials(string login, string pass) {
        try {
            User u = Connection.Query<User>($"SELECT * FROM users WHERE username='{login}'").SingleOrDefault();
            return u.password == pass;
        }
        catch (NullReferenceException){
            return false;
        }
    }

    
    public User GetUser(string login, string pass) {
        try {
            User u = Connection.Query<User>($"SELECT * FROM users WHERE username='{login}'").SingleOrDefault();
            return u;
        }
        catch (NullReferenceException){
            return null;
        }
    }
    
    public bool CreateUser(string login, string pass) {
        try {
            Connection.Execute($"INSERT INTO users VALUES('{login}', '{pass}', 0)");
            return true;
        }
        catch (SqliteException e) {
            Console.WriteLine(e.Message + " " + $"INSERT INTO users VALUES('{login}', '{pass}', 0)");
            return false;
        }
    }
    public bool IsAdmin(string login) {
        try {
            User u = Connection.Query<User>($"SELECT * FROM users WHERE username='{login}'").SingleOrDefault();
            return u.role == 1;
        }
        catch (NullReferenceException){
            return false;
        }
    }
    public bool AddAdmin(User user) {
        try {
            User u = Connection.Query<User>($"SELECT * FROM users WHERE username='{@user.username}'").SingleOrDefault();
            if (u != null) {
                u.role = 1;
                Connection.Execute($"UPDATE users SET role = {u.role} WHERE username='{@user.username}'");
                return true;
            }
            return false;
        }
        catch (NullReferenceException){
            return false;
        }
    }
    public List<Flight> SearchFlights(string dep, string dest, string date) {
        List <Flight> flight =
            Connection.Query<Flight>($"SELECT * FROM flights " +
                                     $"WHERE departure like '{dep}%'" +
                                     $"AND destination like '{dest}%'"+
                                     $"AND date like '{date}%'"
            ).ToList();
        return flight;
    }
    public bool AddFlight(Flight flight)
    {
        try
        {
            string sql = "INSERT INTO flights (Departure, Destination, Date, FlightDuration, " +
                         "Price, FlightNumber, FlightOperator) VALUES " +
                         "(@Departure, @Destination, @Date, @FlightDuration, @Price, @FlightNumber, @FlightOperator)";
            Connection.Execute(sql, flight);
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }
    
    public bool DeleteFlight(Flight flight)
    {
        try
        {
            string sql = "DELETE FROM flights WHERE FlightNumber = @FlightNumber";
            Connection.Execute(sql, flight);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    public bool EditFlight(Flight flight)
    {
        try
        {
            
            string sql = "UPDATE flights SET Departure = @Departure, Destination = @Destination, Date = @Date, " +
                         "FlightDuration = @FlightDuration, Price = @Price, FlightOperator = @FlightOperator " +
                         "WHERE FlightNumber = @FlightNumber";
            Connection.Execute(sql, flight);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    public bool SaveFlight(string username, string flightNumber) {
        try
        {
            
            string sql = $"INSERT INTO savedFlights VALUES('{username}', '{flightNumber}')";
            Connection.Execute(sql);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;
        }
    }

    public List<Flight> SearchSavedFlights(string username) {
        return Connection.Query<Flight>($"select Departure, Destination, Date, Price, FlightDuration, FlightNumber, FlightOperator from flights join savedFlights using(FlightNumber)" +
                     $"where savedFlights.username='{username}'").ToList();
    }
}