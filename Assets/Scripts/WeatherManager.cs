using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class WeatherManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField cityInputField;
    public TMP_Text statusText;
    public Image weatherIcon;

    [Header("Weather Icons")]
    public Sprite sunny;
    public Sprite partlySunny;
    public Sprite cloudy;
    public Sprite rain;
    public Sprite snow;
    public Sprite thunder;
    public Sprite thunderRain;
    public Sprite wind;
    public Sprite night;


    [Header("Weather API")]
    public string apiKey = "ae1c0db8d5b74cb88a9164909261601";

    private const string baseUrl = "https://api.weatherapi.com/v1/current.json";

    private Dictionary<string, Sprite> conditionKeywordMap;

    private void Awake()
    {
        weatherIcon.gameObject.SetActive(false);
        conditionKeywordMap = new Dictionary<string, Sprite>()
        {
            { "thunderstorm", thunderRain },
            { "thunder", thunder },
            { "rain", rain },
            { "snow", snow },
            { "sleet", snow },
            { "blizzard", snow },
            { "partly", partlySunny },
            { "cloud", cloudy },
            { "overcast", cloudy },
            { "mist", wind },
            { "fog", wind },
            { "wind", wind },
            { "sunny", sunny },
            { "clear", sunny }
        };
    }

    
    public void OnCheckWeatherPressed()
    {
        string city = cityInputField.text;

        if (string.IsNullOrWhiteSpace(city))
        {
            SetStatusDefault("Please enter a city name.");
            return;
        }

        StartCoroutine(GetWeather(city));
    }

    private IEnumerator GetWeather(string city)
    {
        SetStatusDefault("Loading weather...");

        string url = $"{baseUrl}?key={apiKey}&q={city}&aqi=no";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                statusText.text = "Failed to fetch weather.";
                Debug.LogError(request.error);
                yield break;
            }

            ProcessWeatherData(request.downloadHandler.text);
        }
    }

    private void ProcessWeatherData(string json)
    {
        WeatherResponse data = JsonUtility.FromJson<WeatherResponse>(json);

        if (data == null || data.location == null || data.current == null)
        {
            SetStatusDefault("Invalid weather data.");
            return;
        }

        SetStatusSuccess($"{data.location.name}, {data.location.country}\n" +
                         $"Local time: {data.location.localtime}\n" +
                         $"Weather: {data.current.condition.text}\n" +
                         $"Temp: {data.current.temp_c} °C");
        UpdateWeatherIcon(data.current.condition.text,
                          data.location.localtime);
    }

    private void UpdateWeatherIcon(string conditionText, string localTime)
    {
        string lower = conditionText.ToLower();

        int hour = int.Parse(localTime.Substring(11, 2));
        bool isNight = hour >= 21 || hour < 5;

        foreach (var pair in conditionKeywordMap)
        {
            if (lower.Contains(pair.Key))
            {
                weatherIcon.sprite = pair.Value;
                return;
            }
        }

        if (isNight)
        {
            weatherIcon.sprite = night;
            return;
        }
        
        weatherIcon.sprite = cloudy;
    }
    
    private void SetStatusDefault(string message)
    {
        weatherIcon.gameObject.SetActive(false);
        statusText.text = message;
        statusText.verticalAlignment = VerticalAlignmentOptions.Middle;
        statusText.alignment = TextAlignmentOptions.Center;
        statusText.fontStyle = FontStyles.Italic;
    }

    private void SetStatusSuccess(string message)
    {
        weatherIcon.gameObject.SetActive(true);
        statusText.text = message;
        statusText.verticalAlignment = VerticalAlignmentOptions.Top;
        statusText.alignment = TextAlignmentOptions.Left;
        statusText.fontStyle = FontStyles.Normal;
    }
}
