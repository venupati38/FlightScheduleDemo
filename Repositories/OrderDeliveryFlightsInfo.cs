using ConsoleApp1.Models;
using DeliveryFromFlight.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryFromFlight.Repositories
{
    public class OrderDeliveryFlightsInfo : IOrderDeliveryFlightsInfo
    {
        IFilesLoader _filesLoader;
        string _flightSchedulefilepath;
        string _oderfilepath;

        public OrderDeliveryFlightsInfo(IFilesLoader filesLoader, string flightSchedulefilepath, string oderfilepath)
        {
            _filesLoader = filesLoader;
            _oderfilepath = oderfilepath;
            _flightSchedulefilepath = flightSchedulefilepath;
        }
        public void DisplayOrderDeliveryFlightsInfo()
        {
            List<Flight> flights = null;
            List<Order> orders = null;
            try
            {
                flights = _filesLoader.ExtractFlightsInfo(_flightSchedulefilepath);
                if (flights != null && flights.Count > 0)
                {
                    orders = _filesLoader.ExtractOrdersInfo(_oderfilepath);
                    if (orders != null && orders.Count > 0)
                    {
                        string prevorderId = "";
                        foreach (var item in orders)
                        {
                            var listFlights = from selitem in flights
                                              where selitem.Departure == item.Departure
                                              select selitem;

                            if (listFlights != null && listFlights.Any())
                            {
                                prevorderId = "";
                                foreach (var itemflight in listFlights)
                                {
                                    Console.WriteLine($"order: {item.OrderId}, flightNumber: {itemflight.FlightNumber}, departure: {itemflight.Departure}, arrival: {itemflight.Arrival}, day: {itemflight.Day}");
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(prevorderId))
                                {
                                    Console.WriteLine($"order: {item.OrderId}, flightNumber: not scheduled");
                                }
                                prevorderId = item.OrderId;
                            }
                        }
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
