namespace CheapFlightsAppProject.Database;

public class Flight {
    public string Departure { get; set; }
    public string Destination { get; set; }
    public string Date { get; set; }
    
    public string FlightDuration { get; set; }
    public int Price {get; set; }

    public string FlightNumber { get; set; }
    
    public string FlightOperator { get; set; }
}