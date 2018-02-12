using UnityEngine;
namespace GuerraMedieval
{
    public class CannonballBehavior : MonoBehaviour
    {
        public static CannonballBehavior bbh;

		public GameObject cannonBall;
		public GameObject fireBall;
        public float fireRate = 1f;

		private GameObject[] balls;
        private GameObject player;
        private Rigidbody m_rigidbody;

        private float nextFire;

        private bool destroyed;

        // Use this for initialization
        void Start()
        {
            if (bbh == null)
            {
                bbh = this.gameObject.GetComponent<CannonballBehavior>();
            }

            // Find all Bullets in the scene
			balls = new GameObject[2];
			balls[0] = cannonBall;
			balls[1] = fireBall;
            foreach (GameObject obj in balls)
            {
                obj.SetActive(false); //Inactive all bullets
            }

            player = GameObject.FindGameObjectWithTag("Player");
            m_rigidbody = gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
            //m_rigidbody.useGravity = GameManagerMedieval.gmm.WithFlexionExtension;

            nextFire = Time.time + fireRate;
            destroyed = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManagerMedieval.gmm.IsPlaying() && destroyed)
            {
                destroyed = false;
            }
            if (GameManagerMedieval.gmm.IsGameOver() && !destroyed)
            {
                DestroyAll();
                destroyed = true;
            }
            if (GameManagerMedieval.gmm.IsPlaying())
            {
				foreach (GameObject obj in balls)
				{
					obj.GetComponent<Rigidbody>().useGravity = GameManagerMedieval.gmm.WithFlexionExtension;
				}
            }
        }

        /// <summary>
        /// 
        /// </summary>
		public void Fire(Vector3 canonBallVelocity, Vector3 bulletPosition, bool isFireBall)
        {
			int ball = !isFireBall ? 0 : 1;

            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
				if (!balls[ball].activeSelf)
                {
					balls[ball].transform.position = bulletPosition;
					balls[ball].SetActive(true);
					balls[ball].GetComponent<Rigidbody>().velocity = canonBallVelocity;
                }
            }
        }

        public void DestroyAll()
        {
            foreach (GameObject obj in balls)
            {
                obj.GetComponent<CannonballDestroy>().ResetObject();
            }
        }
    }
}

