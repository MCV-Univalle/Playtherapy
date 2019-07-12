using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerRieles : MonoBehaviour
{
    public static GameManagerRieles gm;

    public float obstalcleSpeed = 5f;
    public int obstacleCollisionValue = -5;

    private GameObject player;
    private Vector3 lastPlayerPosition;
    public float timeToWalkReminder = 1f;
    private float timerWalkReminder;
    public GameObject walkReminder;

    public GameObject mainPanel;
    public GameObject parametersPanel;
    public GameObject pausePanel;
    //public GameObject resultsPanel;
    public Text resultsScoreText;
    public Text resultsBestScoreText;
    public Sprite starOn;
    public Sprite starOff;
    public Image star1;
    public Image star2;
    public Image star3;
    public GameObject scoreFeedback;

    public bool isPlaying;
    public bool isGameOver;

    private int score = 0;
    public Text textScore;
    private int fullScore = 0;

    public GameObject timerPanel;
    public GameObject repetitionsPanel;
    public GameObject ParametersScreenManagerRieles;


    public bool withTime;
    public float totalTime;

    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    private float currentTime;
    private float timeMillis;

    private int repetitions;
    public int totalRepetitions;
    public Text textRepetitions;
    public float JogThreshold;
    public float JumpThreshold;
    public float CrouchThreshold;
    public bool JogBool;
    public bool JumpBool;
    public bool CrouchBool;
    public int difficulty;
    private ArrayList validIndexes;

    public Animator timerAnimator;
    public AudioSource singleBeep;
    public AudioSource finalBeep;
    private bool controlTimer = true;

    public FinalAnimationManagerRieles finalAnimation;

    // Use this for initialization
    void Start()
    {
        if (gm == null)
            gm = gameObject.GetComponent<GameManagerRieles>();

        player = GameObject.FindGameObjectWithTag("Player");

        lastPlayerPosition = player.transform.position;
        timerWalkReminder = timeToWalkReminder;

        currentTime = totalTime;
        timeMillis = 1000f;

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
                if (player.transform.position.z <= lastPlayerPosition.z)
                    timerWalkReminder -= Time.deltaTime;
                else
                    timerWalkReminder = timeToWalkReminder;

                walkReminder.SetActive(timerWalkReminder <= 0);

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

                    TimerAnimation();
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

                lastPlayerPosition = player.transform.position;
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

    public void StartGame(bool withTime, float time, int repetitions, bool useJog, float jogThreshold, float jogTime, bool useCrouch, float crouchThreshold, bool useJump, float jumpThreshold, bool useShifts, int difficulty)
    {
        this.withTime = withTime;
        totalTime = time;
        currentTime = totalTime;
        totalRepetitions = repetitions;
        this.repetitions = totalRepetitions;
        JogThreshold = jogThreshold;
        CrouchThreshold = crouchThreshold;
        JumpThreshold = jumpThreshold;
        JumpBool = useJump;
        CrouchBool = useCrouch;
        JogBool = useJog;
        KinectPlayerController kinectPlayer = player.GetComponent<KinectPlayerController>();
        kinectPlayer.SetParameters(useJog, jogThreshold, jogTime, useCrouch, crouchThreshold, useJump, jumpThreshold);
        kinectPlayer.UpdateRootPosition = useShifts;
        PlayerControllerRieles playerController = player.GetComponent<PlayerControllerRieles>();
        playerController.UpdateX = useShifts;
        playerController.AutoMove = !useJog;

        RielesCharacter character = player.GetComponent<RielesCharacter>();
        //character.SetCharacterSpeed(playerSpeed);
        this.difficulty = difficulty;

        validIndexes = new ArrayList();
        if (useShifts)
            validIndexes.Add(0); // trains
        if (useJump)
            validIndexes.Add(1); // barrels
        if (useCrouch)
            validIndexes.Add(2); // blocks
        validIndexes.Add(3); // pickups

        if (useShifts)
            TrackSpawner.ts.trackType = TrackSpawner.TrackType.ThreeLane;
        else
            TrackSpawner.ts.trackType = TrackSpawner.TrackType.SingleLane;

        TrackSpawner.ts.SpawnInitialTracks();
        player.GetComponent<Rigidbody>().useGravity = true;

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
        isPlaying = true;
    }

    // mejorar esto, pensar en otra forma
    public ArrayList GetValidIndexes()
    {
        return validIndexes;
    }

    public void UpdateRepetitions()
    {
        repetitions--;
        textRepetitions.text = repetitions.ToString();
    }

    public void UpdateScore(int value)
    {
        if ((score += value) < 0)
            score = 0;

        textScore.text = score.ToString();

        if (value > 0)
            fullScore += value;
        else
            scoreFeedback.GetComponent<ScoreFeedbackBehaviour>().Show(player.transform.position);
    }

    public void EndGame()
    {
        player.SetActive(false);
        mainPanel.SetActive(false);
        string idMiniGame = "7";

        TherapySessionObject objTherapy = TherapySessionObject.tso;

        if (objTherapy != null)
        {
            //objTherapy.fillLastSession(score, fullScore, (int)totalTime, "0");
            //objTherapy.saveLastGameSession();

            //objTherapy.savePerformance((int)kickScript.BestLeftHipFrontAngle, "4");
            //objTherapy.savePerformance((int)kickScript.BestRightHipFrontAngle, "5");
        }
        GameSessionDAO gameDao = new GameSessionDAO();
        int finalScore;
        if (fullScore > 0)
            finalScore = (int)(((float)score / fullScore) * 100.0f);
        else
            finalScore = 0;
        resultsScoreText.text = "Desempeño: " + finalScore + "%";

        //int angle = (int)_angleLeft;
        GameSessionController gameCtrl = new GameSessionController();
        PerformanceController performanceCtrl = new PerformanceController();
        if (this.withTime == true)
        {
            if (JogBool == true && CrouchBool == true && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalTime, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
                performanceCtrl.addPerformance((int)JumpThreshold, "7");

            }
            if (JogBool == false && CrouchBool == true && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalTime, score, idMiniGame);
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
                performanceCtrl.addPerformance((int)JumpThreshold, "7");
            }
            if (JogBool == false && CrouchBool == false && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalTime, score, idMiniGame);
                performanceCtrl.addPerformance((int)JumpThreshold, "7");
            }
            if (JogBool == false && CrouchBool == true && JumpBool == false)
            {
                gameCtrl.addGameSession(finalScore, 0, totalTime, score, idMiniGame);
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
            }
            if (JogBool == true && CrouchBool == false && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalTime, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
                performanceCtrl.addPerformance((int)JumpThreshold, "7");

            }
            if (JogBool == true && CrouchBool == true && JumpBool == false)
            {
                gameCtrl.addGameSession(finalScore, 0, totalTime, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
            }
            if (JogBool == true && CrouchBool == false && JumpBool == false)
            {
                gameCtrl.addGameSession(finalScore, 0, totalTime, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
            }
        }
        if (this.withTime == false)
        {
            if (JogBool == true && CrouchBool == true && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalRepetitions, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
                performanceCtrl.addPerformance((int)JumpThreshold, "7");
            }
            if (JogBool == false && CrouchBool == true && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalRepetitions, score, idMiniGame);
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
                performanceCtrl.addPerformance((int)JumpThreshold, "7");
            }
            if (JogBool == false && CrouchBool == false && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalRepetitions, score, idMiniGame);
                performanceCtrl.addPerformance((int)JumpThreshold, "7");
            }
            if (JogBool == false && CrouchBool == true && JumpBool == false)
            {
                gameCtrl.addGameSession(finalScore, 0, totalRepetitions, score, idMiniGame);
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
            }
            if (JogBool == true && CrouchBool == false && JumpBool == true)
            {
                gameCtrl.addGameSession(finalScore, 0, totalRepetitions, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
                performanceCtrl.addPerformance((int)JumpThreshold, "7");
            }
            if (JogBool == true && CrouchBool == true && JumpBool == false)
            {
                gameCtrl.addGameSession(finalScore, 0, totalRepetitions, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
                performanceCtrl.addPerformance((int)CrouchThreshold, "8");
            }
            if (JogBool == true && CrouchBool == false && JumpBool == false)
            {
                gameCtrl.addGameSession(finalScore, 0, totalRepetitions, score, idMiniGame);
                performanceCtrl.addPerformance((int)JogThreshold, "16");
            }
        }

        if (objTherapy != null)
            resultsBestScoreText.text = "Mejor: " + objTherapy.getGameRecord() + "%";
        else
            resultsBestScoreText.text = "Mejor:"+ gameDao.GetScore(idMiniGame) + "%";

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

        StartCoroutine(DelayedFinalAnimation());
        //resultsPanel.SetActive(true);
    }

    private void TimerAnimation()
    {
        if (controlTimer && currentTime <= 10 && currentTime > 0)
        {
            timerAnimator.Play("Timer");
            singleBeep.Play();
            controlTimer = false;
        }
        else if (!controlTimer && currentTime <= 0)
        {
            singleBeep.Stop();
            finalBeep.Play();
        }
    }

    private IEnumerator DelayedFinalAnimation()
    {
        yield return new WaitWhile(IsFinalBeepPlaying);
        finalAnimation.FinalAnimation(score);
    }

    private bool IsFinalBeepPlaying()
    {
        return finalBeep.isPlaying;
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

}
