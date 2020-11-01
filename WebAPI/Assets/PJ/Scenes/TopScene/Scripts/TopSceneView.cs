using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class TopSceneView : MonoBehaviour {

    [SerializeField, Tooltip ("ScrollView")]
    GameObject scrollView;

    [SerializeField, Tooltip ("天気リスト項目プレハブ")]
    GameObject weatherListItemPrefab;

    [SerializeField, Tooltip ("天気アイコン画像")]
    Sprite[] WeatherIcons;

    readonly City[] Cities = {
        new City ("北海道", "Hokkaido"),
        new City ("東京", "Tokyo"),
        new City ("名古屋", "Nagoya"),
        new City ("大阪", "Osaka"),
        new City ("神戸", "Kobe"),
        new City ("広島", "Hiroshima"),
        new City ("福岡", "Fukuoka"),
    };

    class City {
        public string nameJP;
        public string nameEng;

        public City (string jp, string eng) {
            this.nameJP = jp;
            this.nameEng = eng;
        }
    }

    Transform scrollContent; // ScrollViewのコンテンツ領域のTransform

    void Start () {
        this.scrollContent = scrollView.transform.Find ("Viewport/Content");

        // 各都市の天気を取得する
        var apiClient = new OpenWeatherMapApiClient ();
        Cities.ToList ().ForEach (city => {
            var itemObj = InstantiateWeatherListItem (city.nameJP);
            apiClient
                .GetCurrentWeather (city.nameEng)
                .SubscribeOn (Scheduler.ThreadPool)
                .ObserveOn (Scheduler.MainThread)
                .Subscribe (data => {
                    var view = itemObj.GetComponent<WeatherListItemView> ();
                    view.WeatherDescription = data.GetDescription ();
                    view.WeatherIcon = GetWeatherIcon (data.GetIconID ());
                });
        });
    }

    // ScrollViewにリスト項目を追加する
    GameObject InstantiateWeatherListItem (string cityName) {
        // プレハブをコンテンツ領域に生成
        var item = Instantiate (weatherListItemPrefab, Vector3.zero, Quaternion.identity);
        item.transform.SetParent (this.scrollContent);
        item.transform.localScale = Vector3.one;
        // ビュー初期化
        var view = item.GetComponent<WeatherListItemView> ();
        view.CityName = cityName;
        return item;
    }

    // IconIDから天気アイコン画像を取得する
    Sprite GetWeatherIcon (string iconID) {
        return WeatherIcons.Where (s => s.name.StartsWith (iconID)).First ();
    }
}