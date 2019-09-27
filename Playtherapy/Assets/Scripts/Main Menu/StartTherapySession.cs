using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class StartTherapySession : MonoBehaviour
{
    public static StartTherapySession stateGame;
    public static int valueList;
    public EventSystem eventSystem;
    public GameObject content; // grid container
    public GameObject buttonPrefab; // minigame frame
    public Text patient_name;
    public Text patient_id;
    public Text therapist_name;
    public Text notificationText;
    public InputField inputObjective;
    public InputField inputDescription;
    public InputField inputTherapyId;
    public InputField inputPatientId;
    public GameObject notification;
    public bool statePatient;
    public bool stateTherapist;
    public TherapySessionObject tso;

    // used for transition between menus
    public GameObject loginCanvas;
    public GameObject loginMenu;
    public GameObject observationAndDescription;
    public GameObject minigamesMenuCanvas;
    public GameObject panelDescription;
    public GameObject playList;
    public TherapistDAO therapyDAO;
    public PatientDAO patientDAO;
    public TherapySessionDAO therapySessionDAO;
    private List<Minigame> minigames = null;

    public GameObject inputID;

    // Use this for initialization
    void Start()
    {
        InputField inputTherapyId = GetComponent<InputField>();
        InputField inputPatientId = GetComponent<InputField>();
        InputField inputObjective = GetComponent<InputField>();
        InputField inputDescription = GetComponent<InputField>();

        minigames = new List<Minigame>();

        minigames.Add(new Minigame("16", "Sushi Samurai", "tirar pura katana"));
        minigames.Add(new Minigame("17", "Atrapalo", "tirar puro tiki"));
        minigames.Add(new Minigame("18", "Dulce Hogar", "tirar pura katana"));
        minigames.Add(new Minigame("19", "El Gran Viaje", "tirar pura katana"));
        minigames.Add(new Minigame("20", "Tiro Libre", "tirar pura katana"));
        minigames.Add(new Minigame("21", "Rieles", "tirar pura katana"));
    }
    void Update()
    {

        if (inputTherapyId.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            inputPatientId.ActivateInputField();
        }
        if (inputPatientId.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            inputTherapyId.ActivateInputField();
        }
        if (inputDescription.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            inputObjective.ActivateInputField();
        }
        if (inputObjective.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            inputDescription.ActivateInputField();
        }
        if (inputObjective.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            inputDescription.ActivateInputField();
        }
    }

    private void Awake()
    {
        if (stateGame == null)
        {
            stateGame = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("soy el primero");
            Debug.Log(valueList);
        } else if (stateGame != this)
        {
            Destroy(gameObject);
            Debug.Log("Soy el segundo y BUM ");
            Debug.Log(valueList);
        }
    }
    public void StartTherapy()
    {
        QueryIdTherapy();
        QueryIdPatient();

        if (inputTherapyId.text != "" && inputPatientId.text != "")
        {
            if (statePatient == true && stateTherapist == true)
            {
                tso.Login();
                DisplayPatientInfo();
                DisplayTherapistInfo();
                LoadMinigames(true, false, false, true, true, false);
            }
            else
            {
                notificationText.text = "Datos incorrectos";
                notification.SetActive(true);

            }
        }

        else
        {
            notificationText.text = "Campos vacios";
            notification.SetActive(true);
        }


    }


    public void SaveFullTherapy()
    {
        if (tso != null)
        {
            tso.Login();
            InsertTherapySession();
            LoadMinigames(true, false, false, false, false, true);
        }
        else
        {
            Debug.Log("No therapy session object found");
            LoadMinigames(true, false, false, false, false, true);
        }

    }

    public void StartPlayList()
    {
        if (tso != null)
        {
            tso.Login();
            LoadMinigames(true, false, false, false, false, true);
        }
        else
        {
            Debug.Log("No therapy session object found");
            LoadMinigames(true, false, false, false, false, true);
        }
    }

    public void DisplayPatientInfo()
    {
        if (tso.Patient != null)
        {
            patient_name.text = tso.Patient.Name + " " + tso.Patient.Lastname;
            patient_id.text = tso.Patient.Id_num;
        }
        else
        {
            Debug.Log("Patient not loaded");
        }
    }

    public void DisplayTherapistInfo()
    {
        if (tso.Therapist != null)
        {
            therapist_name.text = tso.Therapist.Name + " " + tso.Therapist.Lastname;
        }
        else
        {
            Debug.Log("Therapist not loaded");
        }
    }

    public void LoadMinigames(bool mCanvas, bool lMenu, bool lCanvas, bool pDescription, bool oDescription, bool pList)
    {
        if (minigames != null && content != null)
        {
            foreach (Minigame minigame in minigames)
            {
                GameObject m = Instantiate(buttonPrefab, content.transform) as GameObject;
                m.GetComponentInChildren<Text>().text = minigame.Name;
                m.GetComponent<Image>().sprite = GameObject.Find(minigame.Name + " Image").GetComponent<Image>().sprite;
                m.GetComponent<LoadGameScene>().Minigame = minigame;
            }
            valueList = 3;
            Debug.Log(valueList);
            minigamesMenuCanvas.SetActive(mCanvas);
            loginMenu.SetActive(lMenu);
            loginCanvas.SetActive(lCanvas);
            panelDescription.SetActive(pDescription);
            observationAndDescription.SetActive(oDescription);
            playList.SetActive(pList);
        }
    }

    public string QueryIdPatient()
    {
        PatientDAO patientDAO = new PatientDAO();
        Debug.Log(inputPatientId.text);
        int idPatient = patientDAO.GetIdPatient(inputPatientId.text);
        string idPatientStr = idPatient.ToString();
        if (idPatientStr != "0")
        {
            statePatient = true;
            Debug.Log(statePatient);
        }
        return idPatientStr;
    }
    public string QueryIdTherapy()
    {
        TherapistDAO therapyDAO = new TherapistDAO();
        int idTherapy = therapyDAO.GetIdTherapy(inputTherapyId.text);
        string idTherapyStr = idTherapy.ToString();
        if (idTherapyStr != "0")
        {
            stateTherapist = true;
            Debug.Log(stateTherapist);

        }
        return idTherapyStr;
    }

    public void InsertTherapySession()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        TherapySessionController therapySessionCtrl = new TherapySessionController();
        string patient = QueryIdPatient();
        string therapist = QueryIdTherapy();
        therapySessionCtrl.AddTherapy(date, inputObjective.text, inputDescription.text, patient, therapist);
    }
    public void StartPlayList(int value)
    {
        if (value == 3)
        {
            if (tso != null)
            {
                tso.Login();
                LoadMinigames(true, false, false, false, false, true);
            }
            else
            {
                Debug.Log("No therapy session object found");
                LoadMinigames(true, false, false, false, false, true);
            }
        }
     
    }

    public void SetStateNotification()
    {
        notification.SetActive(false);
    }

    public void BackObservation()
    {
        LoadMinigames(true, false, false, true, true, false);

    }
}
