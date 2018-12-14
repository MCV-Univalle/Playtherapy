using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TherapySessionObject : MonoBehaviour
{
    public static TherapySessionObject tso;

    private Patient patient;
    private Therapist therapist;
    private TherapySession therapySession;
    private List<GameSession> gameSessionList;
    private string therapyId;
    private int currentGameSessionId;

    void Start()
    {
        if (tso != null)
            tso = gameObject.GetComponent<TherapySessionObject>();

        gameSessionList = new List<GameSession>();
        therapyId = "";
    }

    public void Login()
    {
        GameObject input = GameObject.Find("Input ID Text");
        string id = input.GetComponent<Text>().text;

        patient = PatientDAO.ConsultPatient(id);
        therapist = TherapistDAO.ConsultTherapist("123");

        if (patient != null && therapist != null)
        {
            therapySession = new TherapySession(therapist.Id_num, patient.Id_num);
            bool insertion = TherapySessionDAO.InsertTherapySession(therapySession);
        }
    }

    public void addGameSession(GameSession gs)
    {
        if (gs != null)
        {
            gameSessionList.Add(gs);
        }
        else
        {
            Debug.Log("Null Game Session");
        }
    }

    public void fillLastSession(int score, int repetitions, int time, string level)
    {
        Debug.Log("Entra FillLastSection");
        Debug.Log("Old score: " + gameSessionList[gameSessionList.Count - 1].Score + " reps " + gameSessionList[gameSessionList.Count - 1].Repetitions);
        gameSessionList[gameSessionList.Count - 1].Score = score;
        gameSessionList[gameSessionList.Count - 1].Repetitions = repetitions;
        gameSessionList[gameSessionList.Count - 1].Time = time;
        gameSessionList[gameSessionList.Count - 1].Level = level;
        Debug.Log("New score: " + gameSessionList[gameSessionList.Count - 1].Score + " reps " + gameSessionList[gameSessionList.Count - 1].Repetitions);
    }

    public void saveLastGameSession()
    {
        if (therapyId == "")
            therapyId = TherapySessionDAO.GetLastTherapyId(patient.Id_num).ToString();
        GameSessionDAO.InsertGameSession(gameSessionList[gameSessionList.Count - 1], therapyId);
        currentGameSessionId = GameSessionDAO.GetLastGameSessionId(therapyId);
    }

    public void savePerformance(int angle, string mov)
    {
        Performance pf = new Performance(angle, mov, currentGameSessionId.ToString());
        PerformanceDAO.InsertPerformance(pf);
    }

    public void restartLastSession()
    {
        GameSession gs = new GameSession(gameSessionList[gameSessionList.Count - 1].Minigame_id);
        addGameSession(gs);
        Debug.Log("Duplica GameSession");
    }

    public float getGameRecord()
    {
        return GameSessionDAO.GetRecord(gameSessionList[gameSessionList.Count - 1], patient.Id_num);
    }

    public void SaveObservations(string observations)
    {
        therapySession.Description = observations;
        if (therapyId == "")
            therapyId = TherapySessionDAO.GetLastTherapyId(patient.Id_num).ToString();
        if (TherapySessionDAO.insertObservations(therapyId, therapySession.Description))
            Debug.Log("Observations successfully saved");
        else
            Debug.Log("Observations not saved");
    }

    public Patient Patient
    {
        get
        {
            return patient;
        }

        set
        {
            patient = value;
        }
    }

    public Therapist Therapist
    {
        get
        {
            return therapist;
        }

        set
        {
            therapist = value;
        }
    }

    public TherapySession TherapySession
    {
        get
        {
            return therapySession;
        }

        set
        {
            therapySession = value;
        }
    }

    public List<GameSession> GameSessionList
    {
        get
        {
            return gameSessionList;
        }

        set
        {
            gameSessionList = value;
        }
    }
}
