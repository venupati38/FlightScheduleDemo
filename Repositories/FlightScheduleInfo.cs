using ConsoleApp1.Models;
using System;
using System.Collections.Generic;

namespace DeliveryFromFlight.Repositories
{
    public class FlightScheduleInfo : IFlightScheduleInfo
    {
        IFilesLoader _filesLoader;
        string _flightSchedulefilepath;
        public FlightScheduleInfo(IFilesLoader filesLoader, string flightSchedulefilepath)
        {
            _filesLoader = filesLoader;
            _flightSchedulefilepath = flightSchedulefilepath;
        }
        public void DisplayFlightsScheduleInfo()
        {
            List<Flight> flights = null;
            try
            {
                flights = _filesLoader.ExtractFlightsInfo(_flightSchedulefilepath);

                if (flights != null && flights.Count > 0)
                {
                    foreach (var flight in flights)
                    {
                        Console.WriteLine($"Flight: {flight.FlightNumber}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
