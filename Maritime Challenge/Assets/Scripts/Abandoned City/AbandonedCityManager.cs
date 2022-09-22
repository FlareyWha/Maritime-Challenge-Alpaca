using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonedCityManager : MonoBehaviourSingleton<AbandonedCityManager>
{
    private List<BaseAbandonedCity> abandonedCities = new List<BaseAbandonedCity>();

    // Start is called before the first frame update
    void Start()
    {
        FindAbandonedCities();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindAbandonedCities()
    {
        //Get the abandoned cities from database???
        abandonedCities.Clear();
        abandonedCities.AddRange(FindObjectsOfType<BaseAbandonedCity>());
    }
}
