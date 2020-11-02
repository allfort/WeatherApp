using System.Linq;
using UniRx;

public class TopSceneViewModel {

    public ReactiveCollection<ReactiveProperty<ListItemData>> ListItems = new ReactiveCollection<ReactiveProperty<ListItemData>> ();

    public class ListItemData {
        public City city;
        public OpenWeatherMapApiClient.WeatherData weatherData;

        public ListItemData (City city) {
            this.city = city;
        }
    }

    readonly City[] Cities = {
        new City ("北海道", "Hokkaido"),
        new City ("東京", "Tokyo"),
        new City ("名古屋", "Nagoya"),
        new City ("大阪", "Osaka"),
        new City ("神戸", "Kobe"),
        new City ("広島", "Hiroshima"),
        new City ("福岡", "Fukuoka"),
    };

    public class City {
        public string nameJP;
        public string nameEng;

        public City (string jp, string eng) {
            this.nameJP = jp;
            this.nameEng = eng;
        }
    }

    public void CreateListItems () {
        Cities.ToList ().ForEach (city => {
            ListItems.Add (new ReactiveProperty<ListItemData> (new ListItemData (city)));
        });
    }

    public void GetWeather () {
        // 各都市の天気を取得する
        var apiClient = new OpenWeatherMapApiClient ();
        ListItems.ToList ().ForEach (item => {
            apiClient
                .GetCurrentWeather (item.Value.city.nameEng)
                .SubscribeOn (Scheduler.ThreadPool)
                .ObserveOn (Scheduler.MainThread)
                .Subscribe (data => {
                    var newItem = new ListItemData (item.Value.city);
                    newItem.weatherData = data;
                    item.Value = newItem;
                });
        });
    }
}