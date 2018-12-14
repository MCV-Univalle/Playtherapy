using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysManagerPiano : MonoBehaviour
{
    public float timeBetween;
    [SerializeField] GameObject[] keys;
    public int touchIndexL;
    public int touchePinkL;
    public int touchRightL;
    public int touchMiddleL;
    public int touchIndexR;
    public int touchePinkR;
    public int touchRightR;
    public int touchMiddleR;



    private Dictionary<Finger.FingerType, int> leftKeysDic;
    private Dictionary<Finger.FingerType, int> rightKeysDic;
    private bool leftKey;
    private bool rightKey;

    public bool isFinalAnimationDone;

    private void Start()
    {
        leftKeysDic = new Dictionary<Finger.FingerType, int>();
        leftKeysDic.Add(Finger.FingerType.TYPE_PINKY, 0);
        leftKeysDic.Add(Finger.FingerType.TYPE_RING, 1);
        leftKeysDic.Add(Finger.FingerType.TYPE_MIDDLE, 2);
        leftKeysDic.Add(Finger.FingerType.TYPE_INDEX, 3);

        rightKeysDic = new Dictionary<Finger.FingerType, int>();
        rightKeysDic.Add(Finger.FingerType.TYPE_PINKY, 7);
        rightKeysDic.Add(Finger.FingerType.TYPE_RING, 6);
        rightKeysDic.Add(Finger.FingerType.TYPE_MIDDLE, 5);
        rightKeysDic.Add(Finger.FingerType.TYPE_INDEX, 4);

        isFinalAnimationDone = false;
    }

    public void KeyBehaviour(Hand hand, Finger.FingerType fingerType)
    {
        Debug.Log(hand);
        if (hand.IsLeft && !GameManagerPiano.gm.leftFingers.Contains(fingerType))
            return;
        else if (hand.IsRight && !GameManagerPiano.gm.rightFingers.Contains(fingerType))
            return;

        int index = 0;
        if (hand.IsLeft)
            index = leftKeysDic[fingerType];
        else if (hand.IsRight)
            index = rightKeysDic[fingerType];

        if (keys[index])
        {
            if (keys[index].GetComponent<Animator>().GetBool("Blinking"))
            {
                keys[index].GetComponent<Animator>().SetBool("Blinking", false);
                keys[index].GetComponent<Animator>().Play("KeyPressed");
                keys[index].GetComponent<KeyBehaviourPiano>().PlayGoodSound();
                keys[index].GetComponentInChildren<ScoreFeedbackPiano>().ShowGreen();
                OkPinch(hand, fingerType);

                GameManagerPiano.gm.UpdateScore(1);
                if (GameManagerPiano.gm.useSimultaneous)
                {
                    if (index < 4)
                        StartCoroutine(DelayedNextLeftKey());
                    else
                        StartCoroutine(DelayedNextRightKey());
                }
                else if (GameManagerPiano.gm.useLeftHand && GameManagerPiano.gm.useRightHand)
                    StartCoroutine(DelayedNextAnyKey());
                else if (GameManagerPiano.gm.useLeftHand)
                    StartCoroutine(DelayedNextLeftKey());
                else if (GameManagerPiano.gm.useRightHand)
                    StartCoroutine(DelayedNextRightKey());
            }
            else
            {
                keys[index].GetComponent<Animator>().Play("KeyPressed");
                keys[index].GetComponent<KeyBehaviourPiano>().PlayBadSound();
                keys[index].GetComponentInChildren<ScoreFeedbackPiano>().ShowRed();
                GameManagerPiano.gm.UpdateScore(-1);
            }
        }
    }

    public void NextAnyKey()
    {
        int nextKey = 0;
        int hand = Random.Range(0, 2);

        if (hand == 0)
        {
            nextKey = Random.Range(0, GameManagerPiano.gm.leftFingers.Count);
            nextKey = leftKeysDic[GameManagerPiano.gm.leftFingers[nextKey]];
        }
        else
        {
            nextKey = Random.Range(0, GameManagerPiano.gm.rightFingers.Count);
            nextKey = rightKeysDic[GameManagerPiano.gm.rightFingers[nextKey]];
        }

        keys[nextKey].GetComponent<Animator>().SetBool("Blinking", true);
    }

    public void NextLeftKey()
    {
        int nextLeftKey = Random.Range(0, GameManagerPiano.gm.leftFingers.Count);
        nextLeftKey = leftKeysDic[GameManagerPiano.gm.leftFingers[nextLeftKey]];
        keys[nextLeftKey].GetComponent<Animator>().SetBool("Blinking", true);
        leftKey = true;
    }

    public void NextRightKey()
    {
        int nextRightKey = Random.Range(0, GameManagerPiano.gm.rightFingers.Count);
        nextRightKey = rightKeysDic[GameManagerPiano.gm.rightFingers[nextRightKey]];
        keys[nextRightKey].GetComponent<Animator>().SetBool("Blinking", true);
        rightKey = true;
    }

    private IEnumerator DelayedNextAnyKey()
    {
        yield return new WaitForSeconds(timeBetween);
        NextAnyKey();
    }

    private IEnumerator DelayedNextLeftKey()
    {
        yield return new WaitForSeconds(timeBetween);
        NextLeftKey();
    }

    private IEnumerator DelayedNextRightKey()
    {
        yield return new WaitForSeconds(timeBetween);
        NextRightKey();
    }

    public void FinalAnimation()
    {
        for (int i = 0; i < keys.Length; i++)
            keys[i].GetComponent<Animator>().SetBool("Blinking", false);

        StartCoroutine(PressKeys());
    }

    private IEnumerator PressKeys()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            yield return new WaitForSeconds(0.15f);
            keys[i].GetComponent<Animator>().Play("KeyPressed");
            keys[i].GetComponent<KeyBehaviourPiano>().PlayGoodSound();
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = keys.Length - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.15f);
            keys[i].GetComponent<Animator>().Play("KeyPressed");
            keys[i].GetComponent<KeyBehaviourPiano>().PlayGoodSound();
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].GetComponent<Animator>().Play("KeyPressed");
            keys[i].GetComponent<KeyBehaviourPiano>().PlayGoodSound();
        }

        isFinalAnimationDone = true;
    }

    public void OkPinch(Hand hand, Finger.FingerType fingerType)
    {
        if (fingerType == Finger.FingerType.TYPE_PINKY && hand.IsLeft == true)
        {
            this.FpinchPinkL = 1;
        }
        if (fingerType == Finger.FingerType.TYPE_RING && hand.IsLeft == true)
        {
            this.FpinchRingL = 1;
        }
        if (fingerType == Finger.FingerType.TYPE_MIDDLE && hand.IsLeft == true)
        {
            this.FpinchMiddleL = 1;
        }
        if (fingerType == Finger.FingerType.TYPE_INDEX && hand.IsLeft == true)
        {
            this.FpinchIndexL = 1;
        }
        if (fingerType == Finger.FingerType.TYPE_PINKY && hand.IsRight == true)
        {
            this.FpinchPinkL = 1;
        }
        if (fingerType == Finger.FingerType.TYPE_RING && hand.IsRight == true)
        {
            this.FpinchRingR = 1;
        }
        if (fingerType == Finger.FingerType.TYPE_MIDDLE && hand.IsRight == true)
        {
            this.FpinchMiddleR = 1;
        }
        if (fingerType == Finger.FingerType.TYPE_INDEX && hand.IsRight == true)
        {
            this.FpinchIndexR = 1;
        }
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

    //Hand right
    public int FpinchIndexR
    {
        get
        {
            return touchIndexR;
        }

        set
        {
            touchIndexR = touchIndexR + value;
        }
    }
    public int FpinchMiddleR
    {
        get
        {
            return touchMiddleR;
        }

        set
        {
            touchMiddleR = touchMiddleR + value;
        }
    }
    public int FpinchRingR
    {
        get
        {
            return touchRightR;
        }

        set
        {
            touchRightR = touchRightR + value;
        }
    }
    public int FpinchPinkR
    {
        get
        {
            return touchePinkR;
        }

        set
        {
            touchePinkR = touchePinkR + value;
        }
    }
}
