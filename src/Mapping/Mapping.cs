using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Mapping
{
    public class Mapping
    {
        public static ObservableCollection<LayerGroup> GetAvailableLayers()
        {
            //TODO: Bind to API
            return new ObservableCollection<LayerGroup>()
            {
                new LayerGroup()
                {
                    Name = "Geotechnical",
                    Layers = new List<Layer>()
                    {
                        new Layer()
                        {
                            Name = "Radon",
                            Enabled = false,
                            URL = "http://109.170.166.246/radon/{z}/{x}/{y}.png"
                        },
                        new Layer()
                        {
                            Name = "Mining Hazard",
                            Enabled = false,
                            URL = "http://109.170.166.246/mininghazard/{z}/{x}/{y}.png"
                        },
                    }
                },
                new LayerGroup()
                {
                    Name = "Pre-Planning",
                    Layers = new List<Layer>()
                    {
                        new Layer()
                        {
                            Name = "Bus Stops",
                            Enabled = false,
                            URL = "local"
                        },
                        new Layer()
                        {
                            Name = "Flood Zone 2",
                            Enabled = false,
                            URL = "http://109.170.166.246/floodzone2/{z}/{x}/{y}.png"
                        }
                    }
                },
                new LayerGroup()
                {
                    Name = "Historic Maps",
                    Layers = new List<Layer>()
                    {
                        new Layer()
                        {
                            Name = "1919-1947",
                            Enabled = false,
                            URL = "http://nls.tileserver.com/nls/{z}/{x}/{y}.jpg",
                            Opacity = 0.5d
                        }
                    }
                }
            };
        }

        public static async Task<string> GetIsochrone(double latitude, double longitude, double maxTime, double timeIntervals)
        {
            var baseAddress = new Uri("https://api.openrouteservice.org");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "5b3ce3597851110001cf624844f9a567079f465f93568e7ed6060c3f");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "5b3ce3597851110001cf624844f9a567079f465f93568e7ed6060c3f");

                using (var content = new StringContent($"{{\"locations\":[[{latitude},{longitude}]],\"range\":[{maxTime}],\"interval\":{timeIntervals},\"range_type\":\"time\",\"units\":\"m\"}}", Encoding.ASCII, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("/v2/isochrones/driving-car", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
            }
        }

        public static ObservableCollection<List<Coordinate>> Isochrones { get; set; } = new ObservableCollection<List<Coordinate>>();
    }

    public class Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public enum IsochroneMode
    {
        Car,
        Cycling,
        Walking
    }
}
