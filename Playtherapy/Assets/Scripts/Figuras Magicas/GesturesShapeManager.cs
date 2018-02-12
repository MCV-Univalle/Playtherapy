
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using PDollarGestureRecognizer;

using UnityEngine;
using UnityEngine.UI;

using Windows.Kinect;
using Windows.Data;

public class GesturesShapeManager : MonoBehaviour {



	public float minScaleFigure;
	public Transform gestureOnScreenPrefab;
	public bool use_kinect=false;
	public Sprite grab_hand;
	public Sprite open_hand;



	private List<Gesture> trainingSet;

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	//GUI
	private string message;
	private bool recognized;
	private string newGestureName = "";





	PlayerControllerFM controller_player;
	SoundsFM sound_handler;
	EffectMagicFM effets;

	GameObject img_right;
	GameObject img_left;
	GameObject canvas;


	private bool _isPressingRight;//, _isHovering;
	public bool IsPressingRight
	{
		get { return _isPressingRight; }
		set
		{
			_isPressingRight = value;
			if (img_right!=null) {

				if (_isPressingRight) {

					if (img_right.GetComponent<Image> ().sprite != grab_hand) {
						img_right.GetComponent<Image> ().sprite = grab_hand;
					}


				} else {
					if (img_right.GetComponent<Image> ().sprite != open_hand) {
						img_right.GetComponent<Image> ().sprite = open_hand;
					}
				}




			}

		}
	}
	private bool _isPressingLeft;//, _isHovering;
	public bool IsPressingLeft
	{
		get { return _isPressingLeft; }
		set
		{
			_isPressingLeft = value;
			if (img_left!=null) {
				if (_isPressingLeft) {

					if (img_left.GetComponent<Image> ().sprite != grab_hand) {
						img_left.GetComponent<Image> ().sprite = grab_hand;
					}


				} else {
					if (img_left.GetComponent<Image> ().sprite != open_hand) {
						img_left.GetComponent<Image> ().sprite = open_hand;
					}
				}
			}

				
		}
	}
	// Gesture Detection Events
	public delegate void GestureAction(Result e);
	public event GestureAction onShapeRecognition;


	void Awake()
	{
		platform = Application.platform;
		//drawArea = new Rect(0, 0, Screen.width - Screen.width / 3, Screen.height);
		drawArea = new Rect(0, 0, Screen.width, Screen.height);
		//Load pre-made gestures
		
		sound_handler = FindObjectOfType<SoundsFM> ();
		effets = FindObjectOfType<EffectMagicFM> ();
		canvas = GameObject.Find ("Canvas") ;
		//loadGestures ();
		if (use_kinect == true) {


			img_left = new GameObject ("left_hand");
			img_left.transform.parent = canvas.transform;
			img_left.transform.localScale= new Vector3(-1,1, 1);
			Image circle = img_left.AddComponent<Image> ();
			circle.sprite = open_hand;


			img_right = new GameObject ("right_hand");
			img_right.transform.parent = canvas.transform;

			circle = img_right.AddComponent<Image> ();
			circle.sprite = open_hand;

		}



	}

	void Start () {
		

	
	}


	void setColorByHand(JointType type)
	{
		Color c1= Color.black;
		Color c2= Color.white;

		switch (type) {
		case JointType.HandLeft:
			c1 = Color.red;
			c2 = Color.yellow;
			break;
		case JointType.HandRight:
			c1 = Color.blue;
			c2 = Color.magenta;
			break;
		default:
			break;
		}

		//LineRenderer lineRenderer = gestureOnScreenPrefab.gameObject.GetComponent<LineRenderer>();
		//lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		//lineRenderer.SetColors(c1, c2);

	}

	public void loadGestures(List<int> array_index_used=null)
	{
		if (array_index_used!=null) {
			trainingSet = new List<Gesture>();
			List<String> src_gestures = new List<string> ();

			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/lineH");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/lineV");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/arrowUp");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/arrowDown");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/thunder");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/yInverse");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/chickenFoot");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/X");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/cross");
			src_gestures.Add ("TextAssets/Figuras Magicas/GestureSet/iluminati");
			List<TextAsset> gesturesXml = new List<TextAsset>();

			TextAsset textAssetsTemp;
			for (int i = 0; i < array_index_used.Count; i++) {
				
				textAssetsTemp = Resources.Load<TextAsset> (src_gestures [array_index_used.ElementAt (i)]);
				gesturesXml.Add (textAssetsTemp);
			}
				
			foreach (TextAsset gestureXml in gesturesXml) {
				trainingSet.Add (GestureIO.ReadGestureFromXML (gestureXml.text));

			}
				
		}


	}
		
	void Update () {

		if (use_kinect)
		{
			DrawWithHandsKinect ();





		} else {
			
			DrawWithTouch ();
		}



		if (Input.GetKeyDown(KeyCode.Space)) {
			recognizeShape ();
		}

	}

	Vector3 previousPos;
	Body onlyBody;
	void DrawWithHandsKinect()
	{
		
		onlyBody = FindObjectOfType<DetectSingleBody> ().onePlayerBody;


		if (onlyBody!=null) {
			if (onlyBody !=null &&onlyBody.IsTracked)
			{
				
				updateRightHand (onlyBody);
				updateLeftHand (onlyBody);

				handsAreTogether (onlyBody);
			}
		}
			


	}
	void handsAreTogether(Body body)
	{

		CameraSpacePoint pos = body.Joints[Windows.Kinect.JointType.HandLeft].Position;
		Vector3 pos_left = new Vector3 (Camera.main.pixelWidth*0.5f+pos.X*Camera.main.pixelWidth,Camera.main.pixelHeight*0.5f+ pos.Y*Camera.main.pixelHeight, pos.Z);

		pos = body.Joints[Windows.Kinect.JointType.HandRight].Position;
		Vector3 pos_right = new Vector3 (Camera.main.pixelWidth*0.5f+pos.X*Camera.main.pixelWidth,Camera.main.pixelHeight*0.5f+ pos.Y*Camera.main.pixelHeight, pos.Z);


		float distance = Vector3.Distance(pos_left,pos_right); 
		print ("distance:" + distance);
		if (distance< 70f && handsAreOpenOrNotTracket(body)) {
			recognizeShape ();
		}

	}
	bool handsAreOpenOrNotTracket(Body body)
	{

		return (body.HandLeftState == HandState.Open || body.HandLeftState == HandState.NotTracked) && (body.HandRightState == HandState.Open || body.HandRightState == HandState.NotTracked);

	}

	void updateLeftHand(Body body)
	{
		CameraSpacePoint pos = body.Joints[Windows.Kinect.JointType.HandLeft].Position;
		Vector3 pos_ = new Vector3 (Camera.main.pixelWidth*0.5f+pos.X*Camera.main.pixelWidth,Camera.main.pixelHeight*0.5f+ pos.Y*Camera.main.pixelHeight, pos.Z);
		//var orientation = body.JointOrientations[Windows.Kinect.JointType.HandRight].Orientation;
		img_left.transform.position = new Vector3(pos_.x, pos_.y, this.transform.position.z);

		virtualKeyPosition = img_left.transform.position;


		if (!IsPressingLeft && body.HandLeftState==HandState.Closed) 
		{
			IsPressingLeft = true;
			setColorByHand (JointType.HandLeft);

			++strokeId;

			Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
			currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
			gestureLinesRenderer.Add(currentGestureLineRenderer);

			vertexCount = 0;
		}

		if (IsPressingLeft == true && body.HandLeftState==HandState.Closed && IsPressingRight==false)
		{
			points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

			currentGestureLineRenderer.SetVertexCount(++vertexCount);
			currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
		}


		if (body.HandLeftState==HandState.Open || body.HandLeftState==HandState.NotTracked) {
			IsPressingLeft = false;
				
		
		}

	}

	void updateRightHand(Body body)
	{
		CameraSpacePoint pos = body.Joints[Windows.Kinect.JointType.HandRight].Position;
		Vector3 pos_ = new Vector3 (Camera.main.pixelWidth*0.5f+pos.X*Camera.main.pixelWidth,Camera.main.pixelHeight*0.5f+ pos.Y*Camera.main.pixelHeight, pos.Z);
		//var orientation = body.JointOrientations[Windows.Kinect.JointType.HandRight].Orientation;
		img_right.transform.position = new Vector3(pos_.x, pos_.y, this.transform.position.z);
		//print ("right:"+ img_right.transform.position);
		virtualKeyPosition = img_right.transform.position;



		if (!IsPressingRight && body.HandRightState==HandState.Closed && IsPressingLeft==false) 
		{
			IsPressingRight = true;
			setColorByHand (JointType.HandRight);

			++strokeId;

			Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
			currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
			gestureLinesRenderer.Add(currentGestureLineRenderer);

			vertexCount = 0;
		}

		if (IsPressingRight == true && body.HandRightState==HandState.Closed)
		{
			points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

			currentGestureLineRenderer.SetVertexCount(++vertexCount);
			currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
		}


		if (body.HandRightState==HandState.Open || body.HandRightState==HandState.NotTracked) {
			IsPressingRight = false;
		}

	}

	void DrawWithTouch()
	{
		if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
			if (Input.touchCount > 0) {
				virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
		} else {
			if (Input.GetMouseButton(0)) {
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}
		}

		if (drawArea.Contains(virtualKeyPosition)) {

			if (Input.GetMouseButtonDown(0)) {



				++strokeId;

				Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
				currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

				gestureLinesRenderer.Add(currentGestureLineRenderer);

				vertexCount = 0;
			}

			if (Input.GetMouseButton(0)) {
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

				currentGestureLineRenderer.SetVertexCount(++vertexCount);
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
			}



		}
	}

	public void recognizeShape()
	{



		if (points.Count>0)
		{

			List<float> posXs = new List<float> ();
			List<float> posYs = new List<float> ();
			foreach (var item in points) {
				posXs.Add (item.X);
				posYs.Add (item.Y);
			}


			float posXmin = posXs.Min();
			float posXmax = posXs.Max();
			float posYmin = posYs.Min();
			float posYmax = posYs.Max();

			float distanceX = posXmax - posXmin;
			float distanceY = posYmax - posYmin;

			float distanceCameraX = Camera.main.pixelWidth;
			float distanceCameraY = Camera.main.pixelHeight;



			float scaleX = distanceX / distanceCameraX;
			float scaleY = distanceY / distanceCameraY;



			recognized = true;
			if (scaleX >= minScaleFigure || scaleY >= minScaleFigure) {
				sound_handler.playRandomSound ();
				if (controller_player == null) {
					controller_player = FindObjectOfType<PlayerControllerFM> ();

				}
				Dictionary<String,int> array_names_spells;
				array_names_spells = new Dictionary<String,int> ();
				array_names_spells.Add ("lineH", 0);
				array_names_spells.Add ("lineV", 1);
				array_names_spells.Add ("arrowUp", 2);
				array_names_spells.Add ("arrowDown", 3);
				array_names_spells.Add ("thunder", 4);
				array_names_spells.Add ("yInverse", 5);
				array_names_spells.Add ("chickenFoot", 6);
				array_names_spells.Add ("X", 7);
				array_names_spells.Add ("cross", 8);
				array_names_spells.Add ("iluminati", 9);

				Gesture candidate = new Gesture (points.ToArray ());
				Result gestureResult = PointCloudRecognizer.Classify (candidate, trainingSet.ToArray ());

				message = gestureResult.GestureClass + " " + gestureResult.Score;

				controller_player.controlAnim.SetTrigger ("attack");

				int indexSpell;
				if (array_names_spells.TryGetValue (gestureResult.GestureClass, out indexSpell)) {
					effets.startCircleMagic (indexSpell);
				}


				if (onShapeRecognition != null) {
					onShapeRecognition (new Result (gestureResult.GestureClass, gestureResult.Score));

				}

			} else {
				sound_handler.playBlock ();
			}


			if (recognized) {

				recognized = false;
				strokeId = -1;

				points.Clear();

				foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

					lineRenderer.SetVertexCount(0);
					Destroy(lineRenderer.gameObject);
				}

				gestureLinesRenderer.Clear();
			}





		}




	}
	void OnGUI() {



		//GUI.DrawTexture (new Rect(0,0,Screen.width*0.25f,Screen.height*0.25f),texture);
		//GUI.Box(drawArea, "Draw Area");

		GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);

//		if (GUI.Button(new Rect(Screen.width - 100, 10, 100, 30), "Recognize")) {
//
//			recognizeShape ();
//		}

//		GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as: ");
//		newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);
//
//		if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add") && points.Count > 0 && newGestureName != "") {
//
//			string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName, DateTime.Now.ToFileTime());
//
//			#if !UNITY_WEBPLAYER
//			GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
//			#endif
//
//			trainingSet.Add(new Gesture(points.ToArray(), newGestureName));
//
//			newGestureName = "";
//		}
	}
}
