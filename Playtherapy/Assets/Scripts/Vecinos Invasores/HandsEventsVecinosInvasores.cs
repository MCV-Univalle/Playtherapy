using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
public class HandsEventsVecinosInvasores : MonoBehaviour {


	ScoreHandlerVecinosInvasores score_handler;
	HandleFireworksEffetsVecinosInasores effectFireworks;
	SoundsInVecinosInvasores sounds;

	List<GameObject> enemies;
	void Start()
	{
		enemies = GameObject.FindObjectOfType<SpannerShipsEnemies>().all__ships;

		score_handler = GameObject.FindObjectOfType<ScoreHandlerVecinosInvasores> ();
		effectFireworks = GameObject.FindObjectOfType<HandleFireworksEffetsVecinosInasores> ();
		sounds = GameObject.FindObjectOfType<SoundsInVecinosInvasores> ();
	}
	public void disableShip(GameObject enemie)
	{
		if (enemie.gameObject.activeSelf==true)
		{
			CatchAnimalAndRun cacthScript = enemie.transform.GetComponent<CatchAnimalAndRun> ();
			if (cacthScript != null) {
				cacthScript.releaseAnimal ();
				score_handler.sum_score (5);
			}
			//sounds.sound_explotion.Play ();
			effectFireworks.simulateFireworkIn (enemie.transform.position);
			enemie.SetActive (false);
		}

	}
	public void DeleteSpaceShip()
	{
		
		Vector3 pos_pinch = handDetector.position_pinch ();
		//print ("call of deletespaceship");
		float distance=float.MaxValue;
		foreach (var enemie in enemies) {
			//print (enemie.transform.position);
			if (HoldParametersVecinosInvasores.type_game == HoldParametersVecinosInvasores.USE_PINCHS)
			{
				distance = Mathf.Abs(Vector3.Distance (enemie.transform.position, pos_pinch));
				//print ("search enemie: "+distance);
				if (distance < 10) {


					if (HoldParametersVecinosInvasores.mode_game == HoldParametersVecinosInvasores.SIMPLE) {
						disableShip (enemie);
					} else {

						EnemyData data = enemie.GetComponent<EnemyData> ();

						if (data.finger_to_destroy.Equals(handDetector.pinch_id) && handDetector.Make_a_good_pinch()) 
						{
							disableShip (enemie);
						}

					}

					//print ("enemigo eliminado");


				}
			}

		}



	}

	void LateUpdate()
	{

		if (HoldParametersVecinosInvasores.type_game == HoldParametersVecinosInvasores.USE_FINGERS)
		{

			foreach (var enemie in enemies)
			{
				float distance = float.MaxValue;


				Vector3 tip_position;

				foreach (var type_finger in HoldParametersVecinosInvasores.fingerTypes) {

					tip_position = handDetector.positionTipFingerLeft (type_finger);

					distance = Mathf.Abs(Vector3.Distance (enemie.transform.position, tip_position));
					//print ("distance for enemie:"+distance);
					if (distance < 2) {

						if (HoldParametersVecinosInvasores.mode_game == HoldParametersVecinosInvasores.SIMPLE) {
							disableShip (enemie);
								
						} else {
							EnemyData data = enemie.GetComponent<EnemyData> ();
							if (data.finger_to_destroy.Equals(type_finger)) {
                                Debug.Log(type_finger);
								disableShip (enemie);
							}
						}

						break;
						break;
					}
					tip_position = handDetector.positionTipFingerRight (type_finger);

					distance = Mathf.Abs(Vector3.Distance (enemie.transform.position, tip_position));

					if (distance < 2) {
						if (HoldParametersVecinosInvasores.mode_game == HoldParametersVecinosInvasores.SIMPLE) {
							disableShip (enemie);

						} else {
							EnemyData data = enemie.GetComponent<EnemyData> ();
							if (data.finger_to_destroy.Equals(type_finger)) {
                                Debug.Log(type_finger);

                                disableShip(enemie);
							}
						}
						break;
						break;
					}

				}
			}
		}
	}

}
