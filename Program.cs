using DeliveryFromFlight.Repositories;
using System;
using System.IO;
using System.Reflection;

namespace DeliveryFromFlight
{
    public class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FilesLoader loader = new FilesLoader();
            string _flightscheduleinfoFilePath = Path.Combine(path, "FlightsSchedule.txt");
            string _orderinfoFilePath = Path.Combine(path, "coding-assigment-orders.json");
            try
            {
                Console.WriteLine("-------Flight schedule information----------");
                IFlightScheduleInfo flightScheduleInfo = new FlightScheduleInfo(loader, _flightscheduleinfoFilePath);
                flightScheduleInfo.DisplayFlightsScheduleInfo();

                Console.WriteLine("-------Flight schedule batch orders----------");
                IOrderDeliveryFlightsInfo orderDeliveryFlightsInfo = new OrderDeliveryFlightsInfo(loader, _flightscheduleinfoFilePath, _orderinfoFilePath);
                orderDeliveryFlightsInfo.DisplayOrderDeliveryFlightsInfo();

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
