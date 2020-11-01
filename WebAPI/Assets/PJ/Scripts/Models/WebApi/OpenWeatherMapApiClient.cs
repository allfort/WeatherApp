using System;
using Retrofit.Methods;
using Retrofit.Parameters;
using UniRx;

public class OpenWeatherMapApiClient : WebApiClientBase {

    protected override string Endpoint => "http://api.openweathermap.org";
    readonly string ApiKey = "38af10229a40eb1b9c0bc8d75d5a785d";

    public interface ApiInterface {

        [Get ("/data/2.5/weather")]
        IObservable<WeatherData> GetCurrentWeather ([Query ("q")] string cityName, [Query ("appid")] string appID, [Query ("lang")] string lang);
    }

    public class WeatherData {
        public Coord coord;
        public Weather[] weather;

        public string GetDescription () {
            return weather[0].description;
        }

        public string GetIconID () {
            return weather[0].icon;
        }
    }

    public class Coord {
        public double lon;
        public double lat;
    }

    public class Weather {
        public string id;
        public string main;
        public string description;
        public string icon;
    }

    public IObservable<WeatherData> GetCurrentWeather (string cityName) {
        return GetService<ApiInterface> ().GetCurrentWeather (cityName, ApiKey, "ja");
    }
}