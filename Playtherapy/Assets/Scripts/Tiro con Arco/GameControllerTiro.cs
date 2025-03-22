using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerTiro : MonoBehaviour
{
    public GameObject timer;
    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    public float currentTime = 600f;
    private float totalGameTime;
    public float timeMillis = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        totalGameTime = currentTime;

    }

    // Update is called once per frame
    void Update()
    {
            currentTime -= Time.deltaTime;
            if (currentTime > 0)
            {
                timeMillis -= Time.deltaTime * 1000;
                if (timeMillis < 0)
                    timeMillis = 1000f;

                textCurrentTime.text = string.Format("{0:00}:{1:00}:{2:00}",
                    Mathf.FloorToInt(currentTime / 60),
                    Mathf.FloorToInt(currentTime % 60),
                    Mathf.FloorToInt(timeMillis * 60 / 1000));

                sliderCurrentTime.value = currentTime * 100 / 60f;

            }
       
    }
}
