using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.Events;
using UnityEngine.UI;

public class HandsManagerPiano : MonoBehaviour
{
    private LeapServiceProvider leap;

    public KeysManagerPiano keysManager;

    public float indexFlexionThreshold;
    public float middleFlexionThreshold;
    public float ringFlexionThreshold;
    public float pinkyFlexionThreshold;
    public float relaxedFingerAngle;

    public float pinchThreshold;
    public float minPinchStrength;
    public float noFingerPinchValue;
    public float pinchIndexL;



    private bool leftKeyPressed;
    private bool rightKeyPressed;

    private bool isLeftHandPinching;
    private bool isRightHandPinching;

    public Text debugText;

    private Vector2 v1;
    private Vector2 v2;
    private Vector3 v3;
    private Vector3 v4;

    private Hand leftHand;
    private Hand rightHand;

    // Use this for initialization
    void Start()
    {
        leap = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
        v1 = new Vector2();
        v2 = new Vector2();
        leftHand = new Hand();
        leftHand.IsLeft = true;
        rightHand = new Hand();
        rightHand.IsLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerPiano.gm != null && GameManagerPiano.gm.isPlaying)
        {
            KeyboardController();

            if (leap.IsConnected())
            {
                if (GameManagerPiano.gm.useFlexion)
                    VerifyFingersFlexion();
                else
                    VerifyFingersPinch();
            }
        }
    }

    public void VerifyFingersFlexion()
    {
        string text = "";
        float angle = 0;

        foreach (Hand hand in leap.CurrentFrame.Hands)
        {
            bool key1 = true;
            bool key2 = true;
            bool key3 = true;
            bool key4 = true;

            if (hand.IsLeft && GameManagerPiano.gm.useLeftHand)
            {
                v3 = (hand.GetIndex().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetIndex().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetIndex().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetIndex().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Index(Base): " + v3 + "\n";
                text += "--> Index(Tip): " + v4 + "\n";
                text += "--> Index(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= indexFlexionThreshold && !leftKeyPressed)
                {
                    OnLeftIndexActivated();
                    leftKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key1 = false;

                v3 = (hand.GetMiddle().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetMiddle().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetMiddle().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetMiddle().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Middle(Tip): " + v3 + "\n";
                text += "--> Middle(Base): " + v4 + "\n";
                text += "--> Middle(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= middleFlexionThreshold && !leftKeyPressed)
                {
                    OnLeftMiddleActivated();
                    leftKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key2 = false;

                v3 = (hand.GetRing().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetRing().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetRing().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetRing().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Ring(Tip): " + v3 + "\n";
                text += "--> Ring(Base): " + v4 + "\n";
                text += "--> Ring(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= ringFlexionThreshold && !leftKeyPressed)
                {
                    OnLeftRingActivated();
                    leftKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key3 = false;

                v3 = (hand.GetPinky().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetPinky().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetPinky().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetPinky().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Pinky(Tip): " + v3 + "\n";
                text += "--> Pinky(Base): " + v4 + "\n";
                text += "--> Pinky(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= pinkyFlexionThreshold && !leftKeyPressed)
                {
                    OnLeftPinkyActivated();
                    leftKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key4 = false;

                if (!key1 && !key2 && !key3 && !key4)
                    leftKeyPressed = false;
            }
            else if (hand.IsRight && GameManagerPiano.gm.useRightHand)
            {
                v3 = (hand.GetIndex().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetIndex().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetIndex().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetIndex().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Index(Base): " + v3 + "\n";
                text += "--> Index(Tip): " + v4 + "\n";
                text += "--> Index(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= indexFlexionThreshold && !rightKeyPressed)
                {
                    OnRightIndexActivated();
                    rightKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key1 = false;

                v3 = (hand.GetMiddle().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetMiddle().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetMiddle().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetMiddle().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Middle(Tip): " + v3 + "\n";
                text += "--> Middle(Base): " + v4 + "\n";
                text += "--> Middle(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= middleFlexionThreshold && !rightKeyPressed)
                {
                    OnRightMiddleActivated();
                    rightKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key2 = false;

                v3 = (hand.GetRing().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetRing().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetRing().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetRing().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Ring(Tip): " + v3 + "\n";
                text += "--> Ring(Base): " + v4 + "\n";
                text += "--> Ring(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= ringFlexionThreshold && !rightKeyPressed)
                {
                    OnRightRingActivated();
                    rightKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key3 = false;

                v3 = (hand.GetPinky().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint - hand.GetPinky().Bone(Bone.BoneType.TYPE_METACARPAL).PrevJoint).ToVector3();
                v4 = (hand.GetPinky().Bone(Bone.BoneType.TYPE_INTERMEDIATE).PrevJoint - hand.GetPinky().Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint).ToVector3();

                text += "--> Pinky(Tip): " + v3 + "\n";
                text += "--> Pinky(Base): " + v4 + "\n";
                text += "--> Pinky(Angle): " + Vector3.Angle(v3, v4) + "\n";

                angle = Vector3.Angle(v3, v4);
                if (angle >= pinkyFlexionThreshold && !rightKeyPressed)
                {
                    OnRightPinkyActivated();
                    rightKeyPressed = true;
                    //break;
                }
                else if (angle < relaxedFingerAngle)
                    key4 = false;

                if (!key1 && !key2 && !key3 && !key4)
                    rightKeyPressed = false;
            }
        }

        debugText.text = text;
    }

    public void VerifyFingersPinch()
    {
        string text = "";
        float distance = 0;

        foreach (Hand hand in leap.CurrentFrame.Hands)
        {
            bool pinch1 = true;
            bool pinch2 = true;
            bool pinch3 = true;
            bool pinch4 = true;


            if (hand.IsLeft && GameManagerPiano.gm.useLeftHand)
            {
                v1.x = hand.GetThumb().TipPosition.x;
                v1.y = hand.GetThumb().TipPosition.y;

                v2.x = hand.GetPinky().TipPosition.x;
                v2.y = hand.GetPinky().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Pinky(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isLeftHandPinching)
                {
                    OnLeftPinkyActivated();
                    isLeftHandPinching = true;
                }
                else if (distance > noFingerPinchValue)
                    pinch1 = false;

                v2.x = hand.GetRing().TipPosition.x;
                v2.y = hand.GetRing().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Ring(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isLeftHandPinching)
                {
                    OnLeftRingActivated();
                    isLeftHandPinching = true;
                }
                else if (distance > noFingerPinchValue)
                    pinch2 = false;

                v2.x = hand.GetMiddle().TipPosition.x;
                v2.y = hand.GetMiddle().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Middle(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isLeftHandPinching)
                {
                    OnLeftMiddleActivated();
                    isLeftHandPinching = true;

                }
                else if (distance > noFingerPinchValue)
                    pinch3 = false;

                v2.x = hand.GetIndex().TipPosition.x;
                v2.y = hand.GetIndex().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Index(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isLeftHandPinching)
                {
                    OnLeftIndexActivated();
                    isLeftHandPinching = true;
                    pinchIndexL = pinchIndexL++;
                    Debug.Log(pinchIndexL);
                }
                else if (distance > noFingerPinchValue)
                    pinch4 = false;

                if (!pinch1 && !pinch2 && !pinch3 && !pinch4)
                    isLeftHandPinching = false;

            }
            else if (hand.IsRight && GameManagerPiano.gm.useRightHand)
            {
                v1.x = hand.GetThumb().TipPosition.x;
                v1.y = hand.GetThumb().TipPosition.y;

                v2.x = hand.GetPinky().TipPosition.x;
                v2.y = hand.GetPinky().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Pinky(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isRightHandPinching)
                {
                    OnRightPinkyActivated();
                    isRightHandPinching = true;
                }
                else if (distance > noFingerPinchValue)
                    pinch1 = false;

                v2.x = hand.GetRing().TipPosition.x;
                v2.y = hand.GetRing().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Ring(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isRightHandPinching)
                {
                    OnRightRingActivated();
                    isRightHandPinching = true;
                }
                else if (distance > noFingerPinchValue)
                    pinch2 = false;

                v2.x = hand.GetMiddle().TipPosition.x;
                v2.y = hand.GetMiddle().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Middle(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isRightHandPinching)
                {
                    OnRightMiddleActivated();
                    isRightHandPinching = true;
                }
                else if (distance > noFingerPinchValue)
                    pinch3 = false;

                v2.x = hand.GetIndex().TipPosition.x;
                v2.y = hand.GetIndex().TipPosition.y;
                distance = (v2 - v1).magnitude;
                text += "Index(Pinch): " + distance + "\n";
                text += "Force: " + hand.PinchStrength + "\n";

                if (distance < pinchThreshold && hand.PinchStrength >= minPinchStrength && !isRightHandPinching)
                {
                    OnRightIndexActivated();
                    isRightHandPinching = true;
                }
                else if (distance > noFingerPinchValue)
                    pinch4 = false;

                if (!pinch1 && !pinch2 && !pinch3 && !pinch4)
                    isRightHandPinching = false;
            }
        }

        debugText.text = text;
    }

    public void KeyboardController()
    {
        if (Input.GetKeyDown(KeyCode.A))
            OnLeftPinkyActivated();
        if (Input.GetKeyDown(KeyCode.S))
            OnLeftRingActivated();
        if (Input.GetKeyDown(KeyCode.D))
            OnLeftMiddleActivated();
        if (Input.GetKeyDown(KeyCode.F))
            OnLeftIndexActivated();

        if (Input.GetKeyDown(KeyCode.L))
            OnRightPinkyActivated();
        if (Input.GetKeyDown(KeyCode.K))
            OnRightRingActivated();
        if (Input.GetKeyDown(KeyCode.J))
            OnRightMiddleActivated();
        if (Input.GetKeyDown(KeyCode.H))
            OnRightIndexActivated();
    }

    public void OnLeftIndexActivated()
    {
        keysManager.KeyBehaviour(leftHand, Finger.FingerType.TYPE_INDEX);
    }

    public void OnLeftMiddleActivated()
    {
        keysManager.KeyBehaviour(leftHand, Finger.FingerType.TYPE_MIDDLE);
        Debug.Log("Logrado");
    }

    public void OnLeftRingActivated()
    {
        keysManager.KeyBehaviour(leftHand, Finger.FingerType.TYPE_RING);
    }

    public void OnLeftPinkyActivated()
    {
        keysManager.KeyBehaviour(leftHand, Finger.FingerType.TYPE_PINKY);
    }

    public void OnRightIndexActivated()
    {
        keysManager.KeyBehaviour(rightHand, Finger.FingerType.TYPE_INDEX);
    }

    public void OnRightMiddleActivated()
    {
        keysManager.KeyBehaviour(rightHand, Finger.FingerType.TYPE_MIDDLE);
    }

    public void OnRightRingActivated()
    {
        keysManager.KeyBehaviour(rightHand, Finger.FingerType.TYPE_RING);
    }

    public void OnRightPinkyActivated()
    {
        keysManager.KeyBehaviour(rightHand, Finger.FingerType.TYPE_PINKY);
    }


}
