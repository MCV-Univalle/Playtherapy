using Leap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerPiano : MonoBehaviour
{
    public static GameManagerPiano gm;

    public KeysManagerPiano keysManager;
    public HandsManagerPiano handsManager;

    public GameObject mainPanel;
    public GameObject parametersPanel;
    public GameObject pausePanel;
    public GameObject resultsPanel;
    public Text resultsScoreText;
    public Text resultsBestScoreText;
    public Sprite starOn;
    public Sprite starOff;
    public UnityEngine.UI.Image star1;
    public UnityEngine.UI.Image star2;
    public UnityEngine.UI.Image star3;
    public GameObject scoreFeedback;

    public bool isPlaying;
    public bool isGameOver;

    private int score = 0;
    public Text textScore;
    private int fullScore = 0;

    public GameObject timerPanel;
    public GameObject repetitionsPanel;

    public bool withTime;
    public float totalTime;

    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    private float currentTime;
    private float timeMillis;

    private int repetitions;
    public int totalRepetitions;
    public Text textRepetitions;

    public bool useLeftHand;
    public bool useRightHand;
    public bool useSimultaneous;
    public bool useFlexion;

    public List<Finger.FingerType> leftFingers;
    public List<Finger.FingerType> rightFingers;

    // Use this for initialization
    void Start ()
    {
        if (gm == null)
            gm = gameObject.GetComponent<GameManagerPiano>();

        currentTime = totalTime;
        timeMillis = 1000f;

        leftFingers = new List<Finger.FingerType>();
        rightFingers = new List<Finger.FingerType>();

        if (PlaylistManager.pm == null || (PlaylistManager.pm != null && !PlaylistManager.pm.active)) // playlist active check
        {
            parametersPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isPlaying)
        {
            if (!isGameOver)
            {
                if (withTime)
                {
                    currentTime -= Time.deltaTime;

                    if (currentTime >= 0)
                    {
                        timeMillis -= Time.deltaTime * 1000;
                        if (timeMillis < 0)
                            timeMillis = 1000f;
                        //Debug.Log(currentTime);
                        textCurrentTime.text = (((int)currentTime) / 60).ToString("00") + ":"
                            + (((int)currentTime) % 60).ToString("00") + ":"
                            + ((int)(timeMillis * 60 / 1000)).ToString("00");
                        sliderCurrentTime.value = currentTime * 100 / totalTime;
                    }
                    else
                    {
                        isPlaying = false;
                        isGameOver = true;
                        textCurrentTime.text = "00:00:00";
                    }
                }
                else
                {
                    totalTime += Time.deltaTime;

                    if (repetitions == 0)
                    {
                        isPlaying = false;
                        isGameOver = true;
                    }
                }
            }
            else
            {
                Debug.Log("not supposed to be seen");
            }
        }
        else if (isGameOver)
        {
            isGameOver = false;
            EndGame();
        }
    }

    public void TutorialPause()
    {
        isPlaying = false;

        Time.timeScale = 1;
    }
    public void EndTutorial()
    {

        isPlaying = true;
        Time.timeScale = 0;
    }


    public void StartGame()
    {
        isPlaying = true;
    }

    public void StartGame(bool withTime, float time, int repetitions, List<Finger.FingerType> leftFingers, List<Finger.FingerType> rightFingers, 
        bool useSimultaneous, float timeBetweenReps, bool useFlexion, float indexAngle, float middleAngle, float ringAngle, float pinkyAngle, 
        float minPinchStrenght)
    {
        this.withTime = withTime;
        totalTime = time;
        currentTime = totalTime;
        totalRepetitions = repetitions;
        this.repetitions = totalRepetitions;
        useLeftHand = leftFingers.Count > 0;
        useRightHand = rightFingers.Count > 0;
        this.leftFingers = leftFingers;
        this.rightFingers = rightFingers;
        this.useSimultaneous = useSimultaneous;
        keysManager.timeBetween = timeBetweenReps;
        this.useFlexion = useFlexion;
        handsManager.indexFlexionThreshold = indexAngle;
        handsManager.middleFlexionThreshold = middleAngle;
        handsManager.ringFlexionThreshold = ringAngle;
        handsManager.pinkyFlexionThreshold = pinkyAngle;
        handsManager.minPinchStrength = minPinchStrenght;

        if (withTime)
        {
            timerPanel.SetActive(true);
            repetitionsPanel.SetActive(false);
        }
        else
        {
            textRepetitions.text = totalRepetitions.ToString();
            timerPanel.SetActive(false);
            repetitionsPanel.SetActive(true);
        }

        mainPanel.SetActive(true);
        pausePanel.SetActive(true);

        if (useSimultaneous)
        {
            keysManager.NextLeftKey();
            keysManager.NextRightKey();
        }
        else if (useLeftHand && useRightHand)
            keysManager.NextAnyKey();
        else if (useLeftHand)
            keysManager.NextLeftKey();
        else if (useRightHand)
            keysManager.NextRightKey();

        isPlaying = true;
    }

    private void UpdateRepetitions()
    {
        repetitions--;
        textRepetitions.text = repetitions.ToString();
    }

    public void UpdateScore(int value)
    {
        if ((score += value) < 0)
            score = 0;

        textScore.text = score.ToString();

        if (!withTime)
            UpdateRepetitions();

        if (value > 0)
            fullScore += value;
    }

    private void EndGame()
    {
        mainPanel.SetActive(false);

        TherapySessionObject objTherapy = TherapySessionObject.tso;

        if (objTherapy != null)
        {
            //objTherapy.fillLastSession(score, fullScore, (int)totalTime, "0");
            //objTherapy.saveLastGameSession();

            //objTherapy.savePerformance((int)kickScript.BestLeftHipFrontAngle, "4");
            //objTherapy.savePerformance((int)kickScript.BestRightHipFrontAngle, "5");
        }

        int finalScore;
        if (fullScore > 0)
            finalScore = (int)(((float)score / fullScore) * 100.0f);
        else
            finalScore = 0;
        resultsScoreText.text = "Desempeño: " + finalScore + "%";

        if (objTherapy != null)
            resultsBestScoreText.text = "Mejor: " + objTherapy.getGameRecord() + "%";
        else
            resultsBestScoreText.text = "Mejor: --";

        if (finalScore <= 60)
        {
            //resultMessage.GetComponent<TextMesh>().text = "¡Muy bien!";
            star1.sprite = starOn;
            star2.sprite = starOff;
            star3.sprite = starOff;
        }
        else if (finalScore <= 90)
        {
            //resultMessage.GetComponent<TextMesh>().text = "¡Grandioso!";
            star1.sprite = starOn;
            star2.sprite = starOn;
            star3.sprite = starOff;
        }
        else if (finalScore <= 100)
        {
            //resultMessage.GetComponent<TextMesh>().text = "¡Increíble!";
            star1.sprite = starOn;
            star2.sprite = starOn;
            star3.sprite = starOn;
        }

        //StartCoroutine(DelayedFinalAnimation());
        //resultsPanel.SetActive(true);
        StartCoroutine(FinalAnimation());
    }

    private IEnumerator FinalAnimation()
    {
        yield return new WaitForSeconds(1f);
        keysManager.FinalAnimation();
        yield return new WaitUntil(() => { return keysManager.isFinalAnimationDone; });
        yield return new WaitForSeconds(1f);
        resultsPanel.SetActive(true);

        //playlist block
        if (PlaylistManager.pm != null && PlaylistManager.pm.active)
            PlaylistManager.pm.NextGame();
    }
}
