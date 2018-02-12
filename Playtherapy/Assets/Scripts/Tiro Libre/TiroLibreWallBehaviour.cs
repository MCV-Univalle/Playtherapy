using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroLibreWallBehaviour : MonoBehaviour
{
    public AudioSource hitSound;
    public ScoreFeedbackBehaviour scoreFeedback;
    public Kick kickScript;

    public void OnTriggerEnter(Collider other)
    {
        if (kickScript.kicked && other.gameObject.tag == "Ball")
        {
            hitSound.Play();
            ShowFeedback(other.transform.position);
            GameManagerTiroLibre.gm.BallHit(0);           
        }
    }

    public void ShowFeedback(Vector3 startPosition)
    {
        scoreFeedback.Show(startPosition);
    }
}
