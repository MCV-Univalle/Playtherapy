using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MovementDetectionLibrary
{
    public class FullBody : MonoBehaviour
    {
        Dictionary<BodyParts, BodyPoint> bodyPointsCollection;
        
        //KinectTwoAdapter sensorTwoKinect;
        public GameObject KinectTAdapter;
        private KinectTwoAdapter _KinectTAdapter;
		public MovementsCollection bodyMovements;
        public Text gt;

        public Text infoText;

        // Use this for initialization
        void Start()
        {
			bodyMovements = new MovementsCollection ();
            //sensorTwoKinect = new KinectTwoAdapter();
            bodyPointsCollection = new Dictionary<BodyParts, BodyPoint>();
            for (int i = 0; i < (int)BodyParts.ThumbRight; i++)
            {
                bodyPointsCollection.Add(((BodyParts) i), new BodyPoint((BodyParts) i));
            }
        }

        // Update is called once per frame
        void Update()
        {
            /*
            foreach (char c in Input.inputString)
            {
                if (c == "\b"[0])
                    if (gt.text.Length != 0)
                        gt.text = gt.text.Substring(0, gt.text.Length - 1);

                    else
                    if (c == "\n"[0] || c == "\r"[0])
                        print("User entered their name: " + gt.text);
                    else
                        gt.text += c;
            }
            */
            if (KinectTAdapter == null)
            {
                return;
            }

            _KinectTAdapter = KinectTAdapter.GetComponent<KinectTwoAdapter>();
            if (_KinectTAdapter == null)
            {
                return;
            }

            for (int i = 0; i < (int)BodyParts.ThumbRight; i++)
            {
                UpdateBodyPoint((BodyParts)i);
				angleMovement ();
            }

            
        }

        void UpdateBodyPoint(BodyParts joint)
        {
            BodyPointPosition position = _KinectTAdapter.ReturnPosition(joint);
           if(joint == BodyParts.FootRight)
            {
                //infoText.text =("Posicion New "+position.name+": " + position.x + " " + position.y + " " + position.z + " ");

            }

            bodyPointsCollection[joint].setPosition(position);
        }

        public BodyPointPosition ReturnPointPosition(BodyParts joint)
        {
            return bodyPointsCollection[joint].getCurrentPosition();
        }

        public void angleMovement() {

 

            bodyMovements.setBodyPointsCollection(bodyPointsCollection);
            //this.testCollectionMovement();
            //infoText.text = ("Angle " + bodyMovements.headFrontalFleAngle());
        }

        public float getAngle(string mov)
        {
            double doubleAngle = 0.0;
            float angle = 0.0f;

            if (mov == "shoulderAbdLeft")
            {
                doubleAngle = (float)bodyMovements.shoulderAbdLeftMovements();
                if (double.IsNaN(doubleAngle))
                {
                    doubleAngle = 0.0;
                }
                angle = (float)doubleAngle;
            }

            if (mov == "shoulderAbdRight")
            {
                doubleAngle = (float)bodyMovements.shoulderAbdRigthMovements();
                if (double.IsNaN(doubleAngle))
                {
                    doubleAngle = 0.0;
                }
                angle = (float)doubleAngle;
            }


            return angle;

        }

        public void testCollectionMovement()
        {

            if (infoText) {
                if (Input.GetKey(KeyCode.A))
                {
                    infoText.text = ("Angle frontal flexion " + bodyMovements.headFrontalFleAngle());

                }

                if (Input.GetKey(KeyCode.B))
                {
                    infoText.text = ("Angle frontal extension " + bodyMovements.headFrontalExtAngle());

                }

                if (Input.GetKey(KeyCode.C))
                {
                    infoText.text = ("Angle lateral extension " + bodyMovements.headLateralAngle());

                }

                if (Input.GetKey(KeyCode.D))
                {
                    infoText.text = ("Angle left shoulder abduccion " + bodyMovements.shoulderAbdLeftMovements());

                }

                if (Input.GetKey(KeyCode.E))
                {
                    infoText.text = ("Angle left shoulder extension " + bodyMovements.shoulderExtLeftMovements());

                }

                if (Input.GetKey(KeyCode.F))
                {
                    infoText.text = ("Angle left shoulder flexion " + bodyMovements.shoulderFlexLeftMovements());

                }
                if (Input.GetKey(KeyCode.G))
                {
                    infoText.text = ("Angle left shoulder extension " + bodyMovements.shoulderRotExtLeftMovements());

                }

                if (Input.GetKey(KeyCode.H))
                {
                    infoText.text = ("Angle left shoulder interior " + bodyMovements.shoulderRotIntLeftMovements());

                }

                if (Input.GetKey(KeyCode.I))
                {
                    infoText.text = ("Angle rigth shoulder abduccion " + bodyMovements.shoulderAbdRigthMovements());

                }

                if (Input.GetKey(KeyCode.J))
                {
                    infoText.text = ("Angle rigth shoulder extension " + bodyMovements.shoulderExtRigthMovements());

                }

                if (Input.GetKey(KeyCode.K))
                {
                    infoText.text = ("Angle rigth shoulder flexion " + bodyMovements.shoulderFlexRigthMovements());

                }

                if (Input.GetKey(KeyCode.L))
                {
                    infoText.text = ("Angle rigth shoulder rotation ext " + bodyMovements.shoulderRotExtRigthMovements());

                }
                if (Input.GetKey(KeyCode.M))
                {
                    infoText.text = ("Angle rigth shoulder rotation int " + bodyMovements.shoulderRotIntRigthMovements());

                }

                if (Input.GetKey(KeyCode.N))
                {
                    infoText.text = ("Angle flexion extension elbow left " + bodyMovements.elbowFleExtLeftMovement());

                }

                if (Input.GetKey(KeyCode.O))
                {
                    infoText.text = ("Angle flexion extension elbow rigth " + bodyMovements.elbowFleExtRigthMovement());

                }

                if (Input.GetKey(KeyCode.P))
                {
                    infoText.text = ("Angle hip left abduccion " + bodyMovements.hipLeftAbMovement());

                }

                if (Input.GetKey(KeyCode.Q))
                {
                    infoText.text = ("Angle hip left aduccion " + bodyMovements.hipLeftAdMovement());

                }

                if (Input.GetKey(KeyCode.R))
                {
                    infoText.text = ("Angle hip left extension " + bodyMovements.hipLeftExtMovement());

                }
                if (Input.GetKey(KeyCode.S))
                {
                    infoText.text = ("Angle hip left flexion " + bodyMovements.hipLeftFlexMovement());

                }

                if (Input.GetKey(KeyCode.T))
                {
                    infoText.text = ("Angle hip left rot ext " + bodyMovements.hipLeftRotExtMovement());

                }

                if (Input.GetKey(KeyCode.V))
                {
                    infoText.text = ("Angle hip left rot int " + bodyMovements.hipLeftRotIntMovement());

                }

                if (Input.GetKey(KeyCode.U))
                {
                    infoText.text = ("Angle hip rigth abduccion " + bodyMovements.hipRigthAbMovement());

                }

                if (Input.GetKey(KeyCode.W))
                {
                    infoText.text = ("Angle hip rigth aduccion " + bodyMovements.hipRigthAdMovement());

                }

                if (Input.GetKey(KeyCode.X))
                {
                    infoText.text = ("Angle hip rigth flex " + bodyMovements.hipRigthFlexMovement());

                }
                if (Input.GetKey(KeyCode.Y))
                {
                    infoText.text = ("Angle hip rigth rot ext " + bodyMovements.hipRigthRotExtMovement());

                }

                if (Input.GetKey(KeyCode.Z))
                {
                    infoText.text = ("Angle hip rigth rot int " + bodyMovements.hipRigthRotIntMovement());

                }

                if (Input.GetKey(KeyCode.Tab))
                {
                    infoText.text = ("Angle knee left " + bodyMovements.kneeLeftMovement());

                }

                if (Input.GetKey(KeyCode.Space))
                {
                    infoText.text = ("Angle kneee rigth " + bodyMovements.kneeRigthMovement());

                }

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    infoText.text = ("Angle spine lat " + bodyMovements.spineLatMovement());

                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    infoText.text = ("Angle spine incli" + bodyMovements.spineIncMovement());

                }
            }
            

        }
    }
}
