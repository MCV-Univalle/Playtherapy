using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.Events;
public class handDetector : MonoBehaviour {


	public UnityEvent onActivate;
	public UnityEvent onDeactivate;

	static public Finger.FingerType pinch_id;
	static public Hand onEventHand;
	LeapServiceProvider provider;
	static Hand left_hand;
	List<bool> is_pinching_left;
	List<bool> already_pinch_left;
	static Hand right_hand;
	List<bool> is_pinching_right;
	List<bool> already_pinch_right;
	Dictionary<Finger.FingerType, int> finger_index;

	void Start ()
	{
		provider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
		finger_index = new Dictionary<Finger.FingerType, int> ();

		finger_index.Add (Finger.FingerType.TYPE_INDEX,0);
		finger_index.Add (Finger.FingerType.TYPE_MIDDLE,1);
		finger_index.Add (Finger.FingerType.TYPE_RING,2);
		finger_index.Add (Finger.FingerType.TYPE_PINKY,3);
	}


	static public Vector3 positionTipFingerLeft(Finger.FingerType id=Finger.FingerType.TYPE_INDEX)
	{

		if (left_hand!=null) {
			Finger finger;
			switch (id) 
			{
			case Finger.FingerType.TYPE_INDEX:

				finger = left_hand.GetIndex ();
				break;
			case Finger.FingerType.TYPE_MIDDLE:

				finger = left_hand.GetMiddle ();
				break;
			case Finger.FingerType.TYPE_RING:

				finger = left_hand.GetRing ();
				break;
			case Finger.FingerType.TYPE_PINKY:
				finger = left_hand.GetPinky ();

				break;

			default:
				finger = left_hand.GetThumb ();
				break;
			}


			return finger.TipPosition.ToVector3 ();
		}
		return Vector3.one * float.MaxValue;

	}
	static public Vector3 positionTipFingerRight(Finger.FingerType id=Finger.FingerType.TYPE_INDEX)
	{

		if (right_hand!=null) {
			Finger finger;
			switch (id) 
			{
			case Finger.FingerType.TYPE_INDEX:

				finger = right_hand.GetIndex ();
				break;
			case Finger.FingerType.TYPE_MIDDLE:

				finger = right_hand.GetMiddle ();
				break;
			case Finger.FingerType.TYPE_RING:

				finger = right_hand.GetRing ();
				break;
			case Finger.FingerType.TYPE_PINKY:
				finger = right_hand.GetPinky ();

				break;

			default:
				finger = right_hand.GetThumb ();
				break;
			}


			return finger.TipPosition.ToVector3 ();
		}
		return Vector3.one * float.MaxValue;

	}
	static public Vector3 position_pinch()
	{

		Finger finger;
		switch (pinch_id) 
		{
		case Finger.FingerType.TYPE_INDEX:

			finger = onEventHand.GetIndex ();
			break;
		case Finger.FingerType.TYPE_MIDDLE:

			finger = onEventHand.GetMiddle ();
			break;
		case Finger.FingerType.TYPE_RING:

			finger = onEventHand.GetRing ();
			break;
		case Finger.FingerType.TYPE_PINKY:
			finger = onEventHand.GetPinky ();

			break;

		default:
			finger = onEventHand.GetIndex ();
			break;
		}



		Finger thumb = onEventHand.GetThumb();
		Vector3 vector= ((finger.TipPosition-thumb.TipPosition)*0.5f).ToVector3();

		return vector;
	}

	float proximity_pinch_with(Hand hand,Finger.FingerType id=Finger.FingerType.TYPE_INDEX )
	{

		Finger finger;
		switch (id) 
		{


		case Finger.FingerType.TYPE_INDEX:

			finger = hand.GetIndex ();
			break;
		case Finger.FingerType.TYPE_MIDDLE:

			finger = hand.GetMiddle ();
			break;
		case Finger.FingerType.TYPE_RING:

			finger = hand.GetRing ();
			break;
		case Finger.FingerType.TYPE_PINKY:
			finger = hand.GetPinky ();

			break;

		default:
			finger = hand.GetIndex ();
			break;
		}



		Finger thumb = hand.GetThumb();
		Vector3 vector= (finger.TipPosition-thumb.TipPosition).ToVector3();

		return vector.magnitude;
	}
	static public bool Make_a_good_pinch()
	{
		return (left_hand.PinchStrength>=HoldParametersVecinosInvasores.select_strenght_pinch || right_hand.PinchStrength>=HoldParametersVecinosInvasores.select_strenght_pinch);


	}
	void verify_pinch_with(Hand hand,Finger.FingerType id=Finger.FingerType.TYPE_INDEX )
	{
		float proximity;
		int index;
		if (hand.IsRight) 
		{
			proximity = proximity_pinch_with (hand, id);
			index = finger_index [id];
			if (proximity < Vector3.up.magnitude) 
			{
				is_pinching_right [index] = true;
			}
			else 
			{
				onDeactivate.Invoke ();
				is_pinching_right [index] = false;
				if (already_pinch_right[index]==true)
				{
					already_pinch_right [index] = false;
					onEventHand = hand;
					onDeactivate.Invoke ();
					onEventHand = null;
				}
			}


			if (already_pinch_right [index] == false && is_pinching_right [index] == true)
			{

				already_pinch_right [index] = true;
				onEventHand = hand;
				pinch_id = id;
				//print ("pinch with id:" + id + "proximity:" + proximity);
				onActivate.Invoke ();

			}
		}
		if (hand.IsLeft) 
		{
			proximity = proximity_pinch_with (hand, id);
			index = finger_index [id];
			if (proximity < Vector3.up.magnitude)
			{

				is_pinching_left [index] = true;
			}
			else 
			{
				
				is_pinching_left [index] = false;

				if (already_pinch_left[index]==true) {
					already_pinch_left [index] = false;
					onEventHand = hand;
					onDeactivate.Invoke ();
					onEventHand = null;
				}

			}


			if (already_pinch_left[index]==false && is_pinching_left[index]==true)
			{
				already_pinch_left [index] = true;
				onEventHand = hand;
				pinch_id = id;
				//print ("pinch with id:"+id+"proximity:"+proximity);
				onActivate.Invoke ();

			}
		}
	}
	void initLeftHand(Hand hand)
	{
		if (hand.IsLeft) {

			if (left_hand==null) {
				left_hand = hand;
				is_pinching_left = new List<bool> ();
				is_pinching_left.Add (false);
				is_pinching_left.Add (false);
				is_pinching_left.Add (false);
				is_pinching_left.Add (false);

				already_pinch_left = new List<bool> ();
				already_pinch_left.Add (false);
				already_pinch_left.Add (false);
				already_pinch_left.Add (false);
				already_pinch_left.Add (false);
			}

			if (!left_hand.Equals(hand))
			{
				left_hand = hand;
				is_pinching_left = new List<bool> ();
				is_pinching_left.Add (false);
				is_pinching_left.Add (false);
				is_pinching_left.Add (false);
				is_pinching_left.Add (false);

				already_pinch_left = new List<bool> ();
				already_pinch_left.Add (false);
				already_pinch_left.Add (false);
				already_pinch_left.Add (false);
				already_pinch_left.Add (false);
			}

		}


	}
	void initRightHand(Hand hand)
	{
		if (hand.IsRight) {

			if (right_hand==null) {
				right_hand = hand;
				is_pinching_right = new List<bool> ();
				is_pinching_right.Add (false);
				is_pinching_right.Add (false);
				is_pinching_right.Add (false);
				is_pinching_right.Add (false);

				already_pinch_right = new List<bool> ();
				already_pinch_right.Add (false);
				already_pinch_right.Add (false);
				already_pinch_right.Add (false);
				already_pinch_right.Add (false);
			}

			if (!right_hand.Equals(hand))
			{
				right_hand = hand;
				is_pinching_right = new List<bool> ();
				is_pinching_right.Add (false);
				is_pinching_right.Add (false);
				is_pinching_right.Add (false);
				is_pinching_right.Add (false);

				already_pinch_right = new List<bool> ();
				already_pinch_right.Add (false);
				already_pinch_right.Add (false);
				already_pinch_right.Add (false);
				already_pinch_right.Add (false);
			}

		}


	}



	void Update ()
	{
			Frame frame = provider.CurrentFrame;
			foreach (Hand hand in frame.Hands)
			{
				initLeftHand (hand);
				initRightHand (hand);


			//print ("strngth:"+ hand.PinchStrength);

				if (HoldParametersVecinosInvasores.type_game==HoldParametersVecinosInvasores.USE_PINCHS)
				{

				foreach (var item in HoldParametersVecinosInvasores.fingerTypes) {
					verify_pinch_with (hand, item);
				}
//					verify_pinch_with (hand, Finger.FingerType.TYPE_INDEX);
//
//					verify_pinch_with (hand, Finger.FingerType.TYPE_MIDDLE);
//
//					verify_pinch_with (hand, Finger.FingerType.TYPE_RING);
//
//					verify_pinch_with (hand, Finger.FingerType.TYPE_PINKY);





				}
			}
	}

}
