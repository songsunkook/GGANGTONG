using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript4 : MonoBehaviour
{
    WeatherManager theWeather;
    public bool rain;

    private void Start()
    {
        theWeather = FindObjectOfType<WeatherManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rain)
        {
            theWeather.Rain();
        }
        else
        {
            theWeather.RainStop();
        }
    }
}
