using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TherapySessionController : MonoBehaviour {

    public GameObject TherapySessionDAO;
    public GameObject TherapySession;

    public TherapySessionController()
    {

    }

    public int AddTherapy(string date, string objetive, string description, string patient_id, string therapy_id)
    {

        TherapySession therapySession = new TherapySession();
        TherapySessionDAO therapySessionDao = new TherapySessionDAO();
        Debug.Log("aqui");

        therapySession.Date = date;
        therapySession.Objective = objetive;
        therapySession.Description = description;
        therapySession.Patient_id = patient_id;
        therapySession.Therapist_id = therapy_id;

        //game.Level();

        int result = therapySessionDao.InsertTherapySessions(therapySession);

        if (result == -1)
        {
            Debug.Log("llegue");
            return result;
        }
        else
        {
            Debug.Log("erro");
        }

        return 0;
    }
}
