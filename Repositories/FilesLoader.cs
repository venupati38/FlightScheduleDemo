using ConsoleApp1.Models;
using DeliveryFromFlight.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DeliveryFromFlight.Repositories
{
    public class FilesLoader : IFilesLoader
    {
        public List<Flight> ExtractFlightsInfo(string filePath)
        {
            List<Flight> listFlight = null;
            List<string> listlines = null;
            string line = "";
            Regex objRegexDayMatch = null;
            Regex objFlightMatch = null;
            Int32 numofcurrentflightcount = 0;
            try
            {
                if (CheckFilePathExists(filePath))
                {
                    listlines = new List<string>();
                    using (StreamReader sr = new StreamReader(@filePath))
                    {
                        line = sr.ReadLine();
                        while (line != null)
                        {

                            listlines.Add(line);
                            line = sr.ReadLine();
                        }
                        sr.Close();
                    }

                    if (listlines.Count > 0)
                    {
                        listFlight = new List<Flight>();
                        objRegexDayMatch = new Regex(@"(?:Day)[\s]([\d])");
                        objFlightMatch = new Regex(@"(?:Flight)[\s]([\d])");

                        for (Int32 i = 0; i < listlines.Count;)
                        {

                            if (objRegexDayMatch.IsMatch(listlines[i]))
                            {
                                Flight objFlight = null;

                                numofcurrentflightcount = i + 1;
                                while (numofcurrentflightcount < listlines.Count && objFlightMatch.IsMatch(listlines[numofcurrentflightcount]))
                                {
                                    objFlight = new Flight();
                                    Flight flight = extractFlightInfo(listlines[numofcurrentflightcount]);
                                    if (flight != null)
                                    {
                                        objFlight.Day = Convert.ToInt32(objRegexDayMatch.Match(listlines[i]).Groups[1].Value);
                                        objFlight.Arrival = flight.Arrival;
                                        objFlight.Departure = flight.Departure;
                                        objFlight.FlightNumber = flight.FlightNumber;
                                    }
                                    listFlight.Add(objFlight);
                                    numofcurrentflightcount++;
                                }
                                if (objFlight == null)
                                {
                                    objFlight = new Flight();
                                    objFlight.Day = Convert.ToInt32(objRegexDayMatch.Match(listlines[i]).Groups[1].Value);
                                }
                                i = numofcurrentflightcount;
                            }
                        }

                    }
                }
                else
                {
                    throw new Exception("Invalid file path");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listFlight;
        }

        public List<Order> ExtractOrdersInfo(string filePath)
        {
            List<Order> listOrder = null;
            Order objorder = null;
            try
            {
                if (CheckFilePathExists(filePath))
                {
                    var json = File.ReadAllText(@filePath);
                    var jsonReturn = JsonConvert.DeserializeObject<dynamic>(json);
                    listOrder = new List<Order>();
                    foreach (var item in jsonReturn)
                    {
                        objorder = new Order();
                        objorder.OrderId = item.Name;
                        objorder.Departure = item.Value["destination"].ToString();
                        listOrder.Add(objorder);
                    }
                }
                else
                {
                    throw new Exception("Invalid file path");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listOrder;
        }

        private Boolean CheckFilePathExists(string filePath)
        {
            Boolean IsFound = false;
            try
            {
                if (File.Exists(filePath))
                {
                    IsFound = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsFound;
        }

        private Flight extractFlightInfo(string inputData)
        {
            Regex regexFlightName = new Regex(@"([\d])");
            Flight flight = null;
            try
            {
                Regex regexArrival = new Regex(@"(?<=Flight[\s][\d]:)(.*?)(?:to)");
                if (Convert.ToString(inputData).Contains(":") && Convert.ToString(inputData).Contains("to"))
                {
                    string _arrival = GetCityName(regexArrival.Match(inputData).Groups[1].Value);
                    string _depature = GetCityName(inputData.Substring(inputData.IndexOf("to") + 2));
                    int _flightNumber = 0;
                    if (int.TryParse(regexFlightName.Match(inputData.Substring(0, inputData.IndexOf(":"))).Value, out _flightNumber))
                    {
                        flight = new Flight { FlightNumber = _flightNumber, Arrival = _arrival, Departure = _depature };
                    }
                    else
                    {
                        flight = new Flight { FlightNumber = 0, Arrival = _arrival, Departure = _depature };
                    }
                }
                return flight;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetCityName(string cityname)
        {
            string name;
            try
            {
                name = cityname.Substring(cityname.IndexOf('('));
                name = name.Substring(1, name.IndexOf(")") - 1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return name;
        }
    }
}
