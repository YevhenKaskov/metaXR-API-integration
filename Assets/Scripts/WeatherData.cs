[System.Serializable]
public class WeatherResponse
{
    public Location location;
    public Current current;
}

[System.Serializable]
public class Location
{
    public string name;
    public string country;
    public string localtime;
}

[System.Serializable]
public class Current
{
    public float temp_c;
    public Condition condition;
}

[System.Serializable]
public class Condition
{
    public string text;
}
