using System;
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

    Transform scrollContent; // ScrollViewのコンテンツ領域のTransform
    TopSceneViewModel viewModel;

    void Awake () {
        viewModel = new TopSceneViewModel ();

        // リストデータの変更をビューに反映する
        viewModel.ListItems.ObserveAdd ().Subscribe (item => {
            var itemObj = InstantiateWeatherListItem (item.Value.Value.city.nameJP);
            item.Value.SkipLatestValueOnSubscribe ().Subscribe (data => {
                var view = itemObj.GetComponent<WeatherListItemView> ();
                view.WeatherDescription = data.weatherData.GetDescription ();
                view.WeatherIcon = GetWeatherIcon (data.weatherData.GetIconID ());
            });
        });
    }

    void Start () {
        this.scrollContent = scrollView.transform.Find ("Viewport/Content");

        viewModel.CreateListItems ();
        viewModel.GetWeather ();
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