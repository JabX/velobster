using System;
using System.Linq;
using System.Threading.Tasks;
using Vélobster.Model;
using Windows.Data.Json;
using Windows.Web.Http;

namespace Vélobster.Provider
{
    public static class StationProvider
    {
        async public static Task<AllStationList> GetStationData()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(
                new Uri("https://api.jcdecaux.com/vls/v1/stations?contract=paris&apiKey=2ea0e103b64bb80ce93b2cc73089d8de7dd7481d")
            );

            if (response.IsSuccessStatusCode)
            {
                var stationsJson = JsonArray.Parse(await response.Content.ReadAsStringAsync());
                return new AllStationList(stationsJson.Select(station =>
                {
                    var number = (int)station.GetObject()["number"].GetNumber();
                    var name = station.GetObject()["name"].GetString();
                    var address = station.GetObject()["address"].GetString();
                    var latitude = station.GetObject()["position"].GetObject()["lat"].GetNumber();
                    var longitude = station.GetObject()["position"].GetObject()["lng"].GetNumber();
                    var banking = station.GetObject()["banking"].GetBoolean();
                    var bonus = station.GetObject()["bonus"].GetBoolean();
                    var status = station.GetObject()["status"].GetString();
                    var totalStands = (int)station.GetObject()["bike_stands"].GetNumber();
                    var availableStands = (int)station.GetObject()["available_bike_stands"].GetNumber();
                    var availableBikes = (int)station.GetObject()["available_bikes"].GetNumber();
                    var lastUpdate = (long)station.GetObject()["last_update"].GetNumber();

                    return new Station(number, name, address, latitude, longitude, banking, bonus, status, totalStands, availableBikes, availableStands, lastUpdate);
                }));                
            }
            else
                return null;
        }
    }
}
