using DigitalRuby.Tween;
using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerMoles : MonoBehaviour
{
    public enum GameModeMoles { Touch, Grab }
    public static GameManagerMoles gm;

    public FinalAnimationMoles finalAnimation;
    public MolesManager molesManager;

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
    public AudioSource music;

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

    public float moleUptime;
    public float timeBetweenMoles;
    public GameModeMoles gameMode;
    public float minGrabStrenght;
    private List<Finger.FingerType> leftFingers;
    private List<Finger.FingerType> rightFingers;
    public Collider[] leftFingerTips;
    public Collider[] rightFingerTips;
    public Collider[] grabColliders;
    public int touchIndexL;
    public int touchePinkL;
    public int touchRightL;
    public int touchMiddleL;
    public int toucheThumbL;
    public int countTotalGrabL;
    public int countTotalGrabR;

    // Use this for initialization
    void Start()
    {
        if (gm == null)
            gm = gameObject.GetComponent<GameManagerMoles>();

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
    void Update()
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
        molesManager.NextMole();


        Time.timeScale = 0;
    }

    public void StartGame(bool withTime, float time, int repetitions, List<Finger.FingerType> leftFingers, List<Finger.FingerType> rightFingers,
        float timeBetweenReps, float moleUptime, GameModeMoles gameMode, float minGrabStrenght)
    {
        Debug.Log(gameMode);
        this.withTime = withTime;
        totalTime = time;
        currentTime = totalTime;
        totalRepetitions = repetitions;
        this.repetitions = totalRepetitions;
        this.leftFingers = leftFingers;
        this.rightFingers = rightFingers;
        this.timeBetweenMoles = timeBetweenReps;
        this.moleUptime = moleUptime;
        this.gameMode = gameMode;
        this.minGrabStrenght = minGrabStrenght;

        if (gameMode == GameModeMoles.Touch)
        {
            for (int i = 0; i < leftFingers.Count; i++)
            {
                for (int j = 0; j < leftFingerTips.Length; j++)
                {
                    if (leftFingers[i] == leftFingerTips[j].gameObject.GetComponent<FingerTipBehaviour>().fingerType)
                        leftFingerTips[j].enabled = true;
                }
            }

            for (int i = 0; i < rightFingers.Count; i++)
            {
                for (int j = 0; j < rightFingerTips.Length; j++)
                {
                    if (rightFingers[i] == rightFingerTips[j].gameObject.GetComponent<FingerTipBehaviour>().fingerType)
                        rightFingerTips[j].enabled = true;
                }
            }
        }
        else if (gameMode == GameModeMoles.Grab)
        {
            for (int i = 0; i < grabColliders.Length; i++)
            {
                grabColliders[i].enabled = true;
            }
        }

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

        music.Play();
        isPlaying = true;
        molesManager.NextMole();
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
        string idMinigame = "8";
        GameSessionDAO gameDao = new GameSessionDAO();

        int finalScore;
        if (fullScore > 0)
            finalScore = (int)(((float)score / fullScore) * 100.0f);
        else
            finalScore = 0;
        resultsScoreText.text = "Desempeño: " + finalScore + "%";

        if (objTherapy != null)
            resultsBestScoreText.text = "Mejor: " + objTherapy.getGameRecord() + "%";
        else
            resultsBestScoreText.text = "Mejor:" + gameDao.GetScore(idMinigame) + "%";

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

        GameSessionController gameCtrl = new GameSessionController();

        if (withTime == true)
        {
            gameCtrl.addGameSession(finalScore, 0, totalTime, fullScore, idMinigame);
        }
        if (withTime == false)
        {
            gameCtrl.addGameSession(finalScore, totalRepetitions, 0, fullScore, idMinigame);
        }
        if (gameMode == GameModeMoles.Touch)
        {
            sendPerformanceTouch();
        }
        else
        {
            sendPerformancePinch();
        }

        StartCoroutine(FinalAnimation());


    }

    private IEnumerator FinalAnimation()
    {
        if (molesManager.gameObject.activeSelf)
        {
            for (int i = 0; i < molesManager.moles.Length; i++)
            {
                molesManager.moles[i].GetComponentInChildren<MoleBehaviour>().ResetMole();
            }
        }

        float startPitch = music.pitch;
        float finalPitch = 0.8f;
        Tween<float> tween = gameObject.Tween("pitch", startPitch, finalPitch, 3f, TweenScaleFunctions.CubicEaseInOut,
            (t) =>
            {
                music.pitch = t.CurrentValue;
            },
            (t) =>
            {

            });

        yield return new WaitUntil(() => { return tween.State == TweenState.Stopped; });

        finalAnimation.Begin(molesManager.moles);

        yield return new WaitUntil(() => { return finalAnimation.isDone; });
        yield return new WaitForSeconds(1f);

        music.Stop();
        pausePanel.SetActive(false);
        resultsPanel.SetActive(true);

        //playlist block
        if (PlaylistManager.pm != null && PlaylistManager.pm.active)
        {
            PlaylistManager.pm.NextGame();
        }
    }

    public void sendPerformancePinch()
    {
        PerformanceController performanceCtrl = new PerformanceController();
        performanceCtrl.addPerformance(this.AllGrabLeft, "30");
        performanceCtrl.addPerformance(this.AllGrabRigth, "10");

    }
    public void sendPerformanceTouch()
    {
        MoleBodyBehaviour molesBh = new MoleBodyBehaviour();
        PerformanceController performanceCtrl = new PerformanceController();
        performanceCtrl.addPerformance(this.FpinchMiddleL, "27");
        performanceCtrl.addPerformance(this.FpinchIndexL, "26");
        performanceCtrl.addPerformance(this.FpinchRingL, "25");
        performanceCtrl.addPerformance(this.FpinchPinkL, "28");
        performanceCtrl.addPerformance(this.FpinchThumbL, "29");

    }
    public int FpinchIndexL
    {
        get
        {
            return touchIndexL;
        }

        set
        {
            touchIndexL = value;
        }
    }
    public int FpinchMiddleL
    {
        get
        {
            return touchMiddleL;
        }

        set
        {
            touchMiddleL = value;
        }
    }
    public int FpinchRingL
    {
        get
        {
            return touchRightL;
        }

        set
        {
            touchRightL = value;
        }
    }
    public int FpinchPinkL
    {
        get
        {
            return touchePinkL;
        }

        set
        {
            touchePinkL = value;
        }
    }

    public int FpinchThumbL
    {
        get
        {
            return toucheThumbL;
        }

        set
        {
            toucheThumbL = value;
        }
    }
    public int AllGrabRigth
    {
        get
        {
            return countTotalGrabR;
        }

        set
        {
            countTotalGrabR = countTotalGrabR + value;
        }
    }
    public int AllGrabLeft
    {
        get
        {
            return countTotalGrabL;
        }

        set
        {
            countTotalGrabL = countTotalGrabL + value;
        }
    }
}
