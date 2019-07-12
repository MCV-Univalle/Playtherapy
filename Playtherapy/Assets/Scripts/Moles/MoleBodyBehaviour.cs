using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBodyBehaviour : MonoBehaviour
{
    [SerializeField] MoleBehaviour moleBehaviour;
    [SerializeField] GameObject fx;
    [SerializeField] AudioSource soundfx;
    public int touchIndexL;
    public int touchePinkL;
    public int touchRightL;
    public int touchMiddleL;
    public int toucheThumbL;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManagerMoles.gm != null && GameManagerMoles.gm.gameMode == GameManagerMoles.GameModeMoles.Touch
            && other.tag.Equals("FingerTip") && (moleBehaviour.isUp || (moleBehaviour.isMoving && !moleBehaviour.isUp)))
        {
            Collision();
            MoleTouched(other.GetComponent<FingerTipBehaviour>().isLeft, other.GetComponent<FingerTipBehaviour>().fingerType);
        }
    }

    public void Collision()
    {
        soundfx.Play();
        GameObject go = Instantiate(fx, transform) as GameObject;
        GameManagerMoles.gm.UpdateScore(1);
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
        if (GetComponent<SkinnedMeshRenderer>() != null)
            GetComponent<SkinnedMeshRenderer>().enabled = false;
        moleBehaviour.feedback.Good(moleBehaviour.gameObject.transform);
        moleBehaviour.ResetMole();
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = true;
        if (GetComponent<SkinnedMeshRenderer>() != null)
            GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    public void MoleTouched(bool isLeft, Finger.FingerType fingerType)
    {
        string debug = "";

        if (isLeft)
            debug += "left";
        else
            debug += "right";

        switch (fingerType)
        {
            case Finger.FingerType.TYPE_INDEX:
                {
                    debug += " index";
                    this.FpinchIndexL = 1;
                    Debug.Log(touchIndexL);
                    GameManagerMoles.gm.FpinchIndexL = this.FpinchIndexL;

                    break;
                }
            case Finger.FingerType.TYPE_MIDDLE:
                {
                    debug += " middle";
                    this.FpinchMiddleL = 1;
                    Debug.Log(touchMiddleL);
                    GameManagerMoles.gm.FpinchMiddleL = this.FpinchMiddleL;
                    break;
                }
            case Finger.FingerType.TYPE_RING:
                {
                    debug += " ring";
                    this.FpinchRingL = 1;
                    GameManagerMoles.gm.FpinchRingL = this.FpinchRingL;

                    Debug.Log(touchRightL);

                    break;
                }
            case Finger.FingerType.TYPE_PINKY:
                {
                    debug += " pinky";
                    this.FpinchPinkL = 1;
                    Debug.Log(touchePinkL);
                    GameManagerMoles.gm.touchePinkL = this.FpinchPinkL;

                    break;
                }
            case Finger.FingerType.TYPE_THUMB:
                {
                    debug += " thumb";
                    this.FpinchThumbL = 1;
                    Debug.Log(toucheThumbL);
                    GameManagerMoles.gm.toucheThumbL = 1;

                    break;
                }
            default:
                {
                    debug += " error";
                    break;
                }
        }

        Debug.Log(debug + "Toque efectivo");
    }

    public int FpinchIndexL
    {
        get
        {
            return touchIndexL;
        }

        set
        {
            touchIndexL = touchIndexL + value;
        }
    }
    public int FpinchMiddleL
    {
        get
        {
            return touchMiddleL;
        }

        set
        {
            touchMiddleL = touchMiddleL + value;
        }
    }
    public int FpinchRingL
    {
        get
        {
            return touchRightL;
        }

        set
        {
            touchRightL = touchRightL + value;
        }
    }
    public int FpinchPinkL
    {
        get
        {
            return touchePinkL;
        }

        set
        {
            touchePinkL = touchePinkL + value;
        }
    }

    public int FpinchThumbL
    {
        get
        {
            return toucheThumbL;
        }

        set
        {
            toucheThumbL = toucheThumbL + value;
        }
    }
}
