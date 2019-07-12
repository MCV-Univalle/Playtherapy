using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

using DigitalRuby.Tween;

public class TrapCamera : MonoBehaviour {





    GameObject manager;
    BodySourceManager bodyManager;
    Body[] bodies;
    public Body onePlayerBody;
    Text comment_text;
    GameObject panel_camera;
    KinectSensor sensor;

    //public List<MonoBehaviour> array_scrips_to_disabled;
    // Use this for initialization
    void Start()
    {

        if (gameObject.activeSelf == true)
        {
            if (manager == null)
            {
                manager = GameObject.Find("ManagerInitialPosition");
                comment_text = GameObject.Find("comment_text").GetComponent<Text>();
                panel_camera = GameObject.Find("PanelInitialPosition");
            }
            bodyManager = manager.GetComponent<BodySourceManager>();
            panel_camera.transform.localScale = Vector3.zero;


            sensor = KinectSensor.GetDefault();
            if (sensor != null)
            {
                sensor.Open();
                if (sensor.IsOpen)
                {
                    lostBody();
                }
            }
        }



    }
    void foundBody()
    {
        comment_text.text = "Listo, espera un momento...";
        Wait3Seconds();
        //print ("a body was found");
    }
    void lostBody()
    {
        comment_text.text = "Por favor, Colocarse enfrente de la camara donde se pueda ver todo el cuerpo";
        ShowPanel();
        //print ("a body has lost");

    }
    void searchBody()
    {



        if (bodyManager != null)
        {
            if (onePlayerBody != null)
            {
                if (onePlayerBody.IsTracked == false)
                {
                    onePlayerBody = null;
                    lostBody();
                }
            }

            bodies = bodyManager.GetBodyData();

            if (bodies != null)
            {


                foreach (var body in bodies)
                {
                    if (body != null)
                    {
                        if (body.IsTracked)
                        {
                            if (onePlayerBody == null)
                            {
                                onePlayerBody = body;
                                foundBody();

                            }

                        }
                    }
                }
            }
        }



    }
    private void ShowPanel()
    {
        //		panel_camera.SetActive (true);
        panel_camera.transform.localScale = Vector3.one;
        Time.timeScale = 0;
        //		Vector3 currentScale = panel_camera.transform.localScale;
        //		Vector3 startPos = Vector3.zero;
        //		Vector3 endPos = Vector3.one;
        //		panel_camera.gameObject.Tween("ShowPanel", startPos, endPos, 0.75f, TweenScaleFunctions.CubicEaseIn, (t) =>
        //			{
        //				// progress
        //				panel_camera.transform.localScale = t.CurrentValue;
        //			}, (t) =>
        //			{
        //				// completion
        //
        //			});
    }
    private void Wait3Seconds()
    {
        //		panel_camera.gameObject.Tween("Wait", 0, 0, 3f, TweenScaleFunctions.CubicEaseIn, (t) =>
        //			{
        //				
        //			}, (t) =>
        //			{
        //				HidePanel();
        //
        //			});

        HidePanel();
    }
    private void HidePanel()
    {
        //		panel_camera.SetActive (false);
        panel_camera.transform.localScale = Vector3.zero;
        Time.timeScale = 1;
        //		Vector3 currentScale = panel_camera.transform.localScale;
        //		Vector3 startPos = Vector3.one;
        //		Vector3 endPos = Vector3.zero;
        //		panel_camera.gameObject.Tween("HidePanel", startPos, endPos, 0.75f, TweenScaleFunctions.CubicEaseIn, (t) =>
        //			{
        //				// progress
        //				panel_camera.transform.localScale = t.CurrentValue;
        //			}, (t) =>
        //			{
        //				// completion
        //				//Time.timeScale=1;
        //			});
    }
    // Update is called once per frame
    void Update()
    {
        //		if (sensor!=null) 
        //		{
        if (sensor.IsOpen)
        {
            searchBody();
        }
        //		}


    }
}
