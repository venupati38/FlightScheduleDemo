using ConsoleApp1.Models;
using DeliveryFromFlight.Models;
using System.Collections.Generic;

namespace DeliveryFromFlight.Repositories
{
    public interface IFilesLoader
    {
        List<Flight> ExtractFlightsInfo(string filePath);
        List<Order> ExtractOrdersInfo(string filePath);
    }
}
