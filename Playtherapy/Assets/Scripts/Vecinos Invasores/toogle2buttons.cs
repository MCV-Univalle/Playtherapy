﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
public class toogle2buttons : MonoBehaviour {


	public Finger.FingerType type;
	public void changeToogle(Toggle button)
	{
		button.isOn =gameObject.GetComponent<Toggle>().isOn ;
	}
}
