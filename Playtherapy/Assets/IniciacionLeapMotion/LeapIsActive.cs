using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeapAPI;
public class LeapIsActive : MonoBehaviour {

	public GameObject leap_panel;
	// Use this for initialization
	void Awake () {
        LeapService.CreateController();
	} 
	
	// Update is called once per frame
	void Update () {
		if (LeapService.IsConected ())
		{
			

			if (leap_panel.activeSelf) {
				leap_panel.SetActive (false);
				Time.timeScale = 1;
			}

		}
		else if (!leap_panel.activeSelf) {
			leap_panel.SetActive (true);
			Time.timeScale = 0;
		}
			
	}
}
