using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuerraMedieval
{
    public class Point : MonoBehaviour {
        public float totalTime = 5f;
        private float currentTime;
        private TextMesh text;

	    // Use this for initialization
	    void Start () {
            currentTime = Time.time;
            text = gameObject.GetComponent<TextMesh>();
	    }
	
	    // Update is called once per frame
	    void Update () {
            transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            if(Time.time - currentTime > totalTime)
            {
                Destroy(this.gameObject);
            }
            text.color = new Color(text.color.r,
                text.color.g,
                text.color.b,
                Mathf.Lerp(totalTime, 0, Time.time - currentTime));
	    }
    }
}
