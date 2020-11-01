using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 天気リスト項目 */
public class WeatherListItemView : MonoBehaviour {

    [SerializeField, Tooltip ("都市名")]
    Text cityNameText;

    [SerializeField, Tooltip ("天気詳細")]
    Text weatherDescrioptionText;

    [SerializeField, Tooltip ("天気アイコン")]
    Image weatherIcon;

    /// <summary>
    /// 都市名
    /// </summary>
    public string CityName {
        get => cityNameText.text;
        set => cityNameText.text = value;
    }

    /// <summary>
    /// 天気詳細
    /// </summary>
    public string WeatherDescription {
        get => weatherDescrioptionText.text;
        set => weatherDescrioptionText.text = value;
    }

    /// <summary>
    /// 天気アイコン
    /// </summary>
    public Sprite WeatherIcon {
        get => weatherIcon.sprite;
        set => weatherIcon.sprite = value;
    }
}