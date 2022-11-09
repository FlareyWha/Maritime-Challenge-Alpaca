using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenDots : MonoBehaviour
{
    private Text LoadingText;

    private string originalText;
    private int numDots = 0;
    private int maxDots = 3;
    private float dotInterval = 0.3f;
    private float timer = 0.1f;


    private void Awake()
    {
        LoadingText = GetComponent<Text>();
        originalText = LoadingText.text;
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            UpdateLoadingText();
            timer = dotInterval;
        }

    }

    private void UpdateLoadingText()
    {
        if (numDots == maxDots)
            numDots = 0;
        else
            numDots++;

        LoadingText.text = originalText;
        for (int i = 0; i < numDots; i++)
        {
            LoadingText.text += ".";
        }
        
    }
}
