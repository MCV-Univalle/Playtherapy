using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalAnimationFL : MonoBehaviour {

	public GameObject prefab_balon;
	public PhysicMaterial bouncy;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startAnimation(){

		int numbers_balls=10;
		GameObject ball;
		Vector3 direction;
		float distance = 3;
		for (int i = 0; i < numbers_balls; i++) {

			direction = new Vector3 (Random.Range (-distance, distance),Random.Range (-1f, 2.5f), Random.Range (-distance, distance));
			ball = Instantiate (prefab_balon, transform.position+direction, Quaternion.identity, transform);
			ball.AddComponent<SphereCollider> ().material = bouncy;
			ball.AddComponent<Rigidbody> ();

		}






	}



	public void cleanAnimation(){

		for (int i = 0; i < transform.childCount; i++) {
			Destroy(transform.GetChild (i).gameObject);
		}

	}
}
