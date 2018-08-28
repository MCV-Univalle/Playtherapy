using UnityEngine;

namespace GuerraMedieval
{
    public class WarriorSpawner : MonoBehaviour
    {

        private GameObject[] warriors;
        public float velocity = 4f;
        public float distance = 20f;

        private Vector3 planeSize;

        public float minSecondsBetweenSpawning = 3.0f;
        public float maxSecondsBetweenSpawning = 6.0f;

        private float savedTime;
        private float secondsBetweenSpawning;
        private int warriorCount;

        private bool destroyed;


        void Start()
        {
            warriors = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in warriors)
            {
                obj.SetActive(false);
            }
            planeSize = gameObject.GetComponent<MeshCollider>().bounds.size;
            destroyed = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManagerMedieval.gmm.IsPlaying())
            {
                Spawn();
            }

            if (GameManagerMedieval.gmm.IsPlaying() && destroyed)
            {
                destroyed = false;
            }

            if (GameManagerMedieval.gmm.IsGameOver() && !destroyed)
            {
                DestroyAll();
                destroyed = true;
            }
        }

        public void Spawn()
        {
            if (Time.time - savedTime >= secondsBetweenSpawning && !warriors[warriorCount].activeSelf)
            {
                if (GameManagerMedieval.gmm.WithFlexionExtension && GameManagerMedieval.gmm.WithPronation == false)
                {
                    print("only Center");
                    warriors[warriorCount].transform.position = new Vector3(0f, 0f, Random.Range(32,50)) ;
                    warriors[warriorCount].SetActive(true);
                    warriors[warriorCount].GetComponent<Rigidbody>().velocity = Vector3.back * velocity;

                    warriorCount++;
                    if (warriorCount >= warriors.Length)
                    {
                        warriorCount = 0;
                    }

                    savedTime = Time.time;
                    secondsBetweenSpawning = Random.Range(minSecondsBetweenSpawning, maxSecondsBetweenSpawning);

                }
                else {

                    warriors[warriorCount].transform.position = new Vector3(Random.Range(-planeSize.x / 2, planeSize.x / 2), 0f, transform.position.z);
                    warriors[warriorCount].SetActive(true);
                    warriors[warriorCount].GetComponent<Rigidbody>().velocity = Vector3.back * velocity;

                    warriorCount++;
                    if (warriorCount >= warriors.Length)
                    {
                        warriorCount = 0;
                    }

                    savedTime = Time.time;
                    secondsBetweenSpawning = Random.Range(minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
                }

                
            }
        }

        public void DestroyAll()
        {
            foreach (GameObject obj in warriors)
            {
                obj.GetComponent<WarriorDestroy>().ResetObject();
            }
        }
    }
}
