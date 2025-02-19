using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    void Start()
    {
        RenderSettings.fog = true;                    
        RenderSettings.fogColor = Color.gray;        
        RenderSettings.fogMode = FogMode.Linear;     
        RenderSettings.fogStartDistance = 15f;        
        RenderSettings.fogEndDistance = 50f;          
    }
}
