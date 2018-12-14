using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabBehaviour : MonoBehaviour
{
    public bool isLeftHand;
    public float noGrabValue;
    public int countTotalGrabR;
    public int countTotalGrabL;

    public Text debugText;

    private LeapServiceProvider leap;

    private bool isGrabbing;

    // Use this for initialization
    void Start()
    {
        leap = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
    }

    void Update()
    {
        if (GameManagerMoles.gm != null && GameManagerMoles.gm.isPlaying && leap.IsConnected()
            && GameManagerMoles.gm.gameMode == GameManagerMoles.GameModeMoles.Grab)
        {
            foreach (Hand hand in leap.CurrentFrame.Hands)
            {
                if (hand.IsLeft && isLeftHand)
                {
                    if (isGrabbing && hand.GrabStrength < noGrabValue)
                    {
                        isGrabbing = false;

                    }
                }
                else
                {
                    if (isGrabbing && hand.GrabStrength < noGrabValue)
                    {
                        isGrabbing = false;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameManagerMoles.gm != null && GameManagerMoles.gm.isPlaying && leap.IsConnected()
            && GameManagerMoles.gm.gameMode == GameManagerMoles.GameModeMoles.Grab)
        {
            foreach (Hand hand in leap.CurrentFrame.Hands)
            {
                if (hand.IsLeft && isLeftHand)
                {
                    if (!isGrabbing && hand.GrabStrength >= GameManagerMoles.gm.minGrabStrenght)
                    {
                        isGrabbing = true;
                        other.gameObject.GetComponent<MoleBodyBehaviour>().Collision();
                        this.AllGrabLeft = 1;
                        Debug.Log(this.AllGrabLeft);
                        GameManagerMoles.gm.AllGrabLeft = this.AllGrabLeft;

                    }
                }
                else
                {
                    if (!isGrabbing && hand.GrabStrength >= GameManagerMoles.gm.minGrabStrenght)
                    {
                        isGrabbing = true;
                        other.gameObject.GetComponent<MoleBodyBehaviour>().Collision();
                        this.AllGrabRigth = 1;
                        Debug.Log(this.AllGrabRigth);
                        GameManagerMoles.gm.AllGrabRigth = this.AllGrabRigth;
                    }
                }
            }
        }
    }
    public int AllGrabRigth
    {
        get
        {
            return countTotalGrabR;
        }

        set
        {
            countTotalGrabR = countTotalGrabR + value;
        }
    }
    public int AllGrabLeft
    {
        get
        {
            return countTotalGrabL;
        }

        set
        {
            countTotalGrabL = countTotalGrabL + value;
        }
    }
}
