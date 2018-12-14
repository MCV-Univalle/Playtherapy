using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// for your own scripts make sure to add the following line:
using DigitalRuby.Tween;
public class ScoreHandlerVecinosInvasores : MonoBehaviour {


	public int ships_caught;
	public int score_obtain;
	public int score_max;
	public Text txt_score;
    public int touchPositive;


    Text desempeno;
	// Use this for initialization
	void Start () {

        txt_score = GameObject.Find("txt_score").GetComponent<Text>();
		reset ();
	}
	public void reset()
	{
		ships_caught = 0;
		score_obtain = 0;
		score_max = 0;
		setScore ();
	}
	public void sum_score(int pts=0)
	{
		if (pts<0)
		{
			if (score_obtain+pts<0)
			{
				score_obtain = 0;
			}
			else
			{
				score_obtain += pts;
				TweenColorIncorrecto ();
			}
		}
		else
		{
			TweenColorCorrecto ();
			score_max += pts;
			score_obtain += pts;
            TouchPositive = 1;
            Debug.Log("Se encontro");
            
        }
		setScore ();
	}
	void setScore()
	{
		txt_score.text =""+ score_obtain;// + "/" + score_max;


		if (score_max>0) {


			string p_string = "000";

			float percent =((float)score_obtain / (float)score_max) * 100;
			//print ("percent:"+percent);
			if (percent == 100f) {
				p_string =""+ percent;
			} else if (percent < 10f) 
			{

				p_string = "00" + percent;
			}
			else 
			{
				p_string = "0" + percent;
			}

			p_string=p_string.Substring (0, 3);

			//desempeno.text = "desempeño: " +p_string +"%";
			//desempeno.text = ""+score_obtain+"/" +score_max +" ->"+ percent+"%";
		}
	}


	private void TweenColorIncorrecto()
	{
		Color endColor = Color.red;
		txt_score.gameObject.Tween("ColorCircle", txt_score.color, endColor, 0.25f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				txt_score.color = t.CurrentValue;
			}, (t) =>
			{
				// completion
				endColor= Color.black;
				txt_score.gameObject.Tween("ColorCircle", txt_score.color, endColor, 0.25f, TweenScaleFunctions.QuadraticEaseOut, (t2) =>
					{
						// progress
						txt_score.color = t2.CurrentValue;
					}, (t2) =>
					{
						// completion
					});
			});
	}
	private void TweenColorCorrecto()
	{
		Color endColor = Color.green;
		txt_score.gameObject.Tween("ColorCircle", txt_score.color, endColor, 0.25f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
			{
				// progress
				txt_score.color = t.CurrentValue;
			}, (t) =>
			{
				// completion
				endColor= Color.black;
				txt_score.gameObject.Tween("ColorCircle", txt_score.color, endColor, 0.25f, TweenScaleFunctions.QuadraticEaseOut, (t2) =>
					{
						// progress
						txt_score.color = t2.CurrentValue;
					}, (t2) =>
					{
						// completion
					});
			});
	}
    public int TouchPositive
    {
        get
        {
            return touchPositive;
        }

        set
        {
            touchPositive = touchPositive + value;
        }
    }
}
