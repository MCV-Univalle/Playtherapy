using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerTiroLibre : MonoBehaviour
{
    public static GameManagerTiroLibre gm;
    public Kick kickScript;

    public int level;
    public bool useFrontPlane;
    public bool useBackPlane;
    public bool useShifts;

    public GameObject parametersPanel;
    public GameObject mainPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject frontLeftLegPanel;
    public GameObject frontRightLegPanel;
    public GameObject backLeftLegPanel;
    public GameObject backRightLegPanel;
    public GameObject shiftLeftPanel;
    public GameObject shiftRightPanel;

    public int score;
    public Text scoreText;
    public TextMesh boardScoreText;

    //public int scoreToWin;
    public bool isPlaying;
    public bool isGameOver;
    public bool withTime;

    public float totalTime;
    private float timeMillis;
    private float currentTime;
    public Slider sliderCurrentTime;
    public Text currentTimeText;

    public int totalRepetitions;
    public int remainingRepetitions;
    public Text repetitionsText;

    public GameObject timerPanel;
    public GameObject repetitionsPanel;

    public int currentTarget;
    public GameObject[] targets;    

    public bool targetReady;
    public float timeBetweenTargets;
    public bool changeMovement;
    public bool useLow;
    public bool useMedium;
    public bool useHigh;
    private int fromTarget;
    private int toTarget;

    public enum LegMovements { FrontLeftLeg, FrontRightLeg, BackLeftLeg, BackRightLeg };
    private LegMovements[] frontPlane;
    private LegMovements[] backPlane;
    public LegMovements currentMovement;
    private System.Random random;
    public ParticleSystem leftLegParticles1;
    public ParticleSystem leftLegParticles2;
    public ParticleSystem rightLegParticles1;
    public ParticleSystem rightLegParticles2;

    public bool isAbleToMove;
    public float shiftsFrequency;
    private bool lastLegMovement;
    private bool lastShiftMovement;
    public enum ShiftPlatforms { LeftPlatform, CenterPlatform, RightPlatform };
    public ShiftPlatforms currentPlatform;
    public GameObject avatarPlatform;
    public GameObject leftShiftPlatform;
    public GameObject centerShiftPlatform;
    public GameObject rightShiftPlatform;

    public Animator cameraAnimator;
    public GameObject resultsPanel;
    public Text resultsScoreText;
    public Text resultsBestScoreText;
    public Sprite starOn;
    public Sprite starOff;
    public Image star1;
    public Image star2;
    public Image star3;    


    // Use this for initialization
    void Start ()
    {
        if (gm == null)
            gm = gameObject.GetComponent<GameManagerTiroLibre>();

        //currentBallStartPosition = ball.transform.position;

        currentTime = totalTime;
        timeMillis = 1000f;

        remainingRepetitions = totalRepetitions;

        frontPlane = new LegMovements[] {LegMovements.FrontLeftLeg, LegMovements.FrontRightLeg};
        backPlane = new LegMovements[] {LegMovements.BackLeftLeg, LegMovements.BackRightLeg};
        random = new System.Random();
        lastLegMovement = false;
        lastShiftMovement = false;
        currentPlatform = ShiftPlatforms.CenterPlatform;

        if (PlaylistManager.pm == null || (PlaylistManager.pm != null && !PlaylistManager.pm.active)) // playlist active check
        {
            mainPanel.SetActive(false);
            parametersPanel.SetActive(true);
        }

        //StartGame();
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
                        currentTimeText.text = (((int)currentTime) / 60).ToString("00") + ":" 
                            + (((int)currentTime) % 60).ToString("00") + ":" 
                            + ((int)(timeMillis * 60 / 1000)).ToString("00");
                        sliderCurrentTime.value = currentTime * 100 / totalTime;                        
                    }
                    else
                    {
                        isPlaying = false;
                        isGameOver = true;
                        currentTimeText.text = "00:00:00";
                    }
                }
                else
                {
                    totalTime += Time.deltaTime;
                }
            }
            else
            {
                Debug.Log("not supposed to be seen");
            }
        }
        else if (isGameOver)
        {
            targetReady = false;
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
        if (withTime)
        {
            repetitionsPanel.SetActive(false);
            timerPanel.SetActive(true);
        }
        else
        {
            timerPanel.SetActive(false);
            repetitionsPanel.SetActive(true);
        }

        repetitionsText.text = remainingRepetitions.ToString();

        isPlaying = true;
        NextMovement();
    }

    public void StartGame(bool withTime, float time, int repetitions, float timeBetweenTargets, bool frontPlane, float frontAngle1, 
        float frontAngle2, float frontAngle3, bool backPlane, float backAngle1, float backAngle2, float backAngle3, bool shifts, 
        float shiftsFrequency, bool changeMovement, bool useLow, bool useMedium, bool useHigh)
    {
        this.withTime = withTime;

        if (withTime)
        {
            repetitionsPanel.SetActive(false);
            timerPanel.SetActive(true);
        }
        else
        {
            timerPanel.SetActive(false);
            repetitionsPanel.SetActive(true);
        }

        totalTime = time;
        currentTime = totalTime;
        totalRepetitions = repetitions;
        remainingRepetitions = totalRepetitions;
        repetitionsText.text = remainingRepetitions.ToString();
        this.timeBetweenTargets = timeBetweenTargets;

        useFrontPlane = frontPlane;
        useBackPlane = backPlane;
        useShifts = shifts;
        this.shiftsFrequency = shiftsFrequency;
        this.changeMovement = changeMovement;
        this.useLow = useLow;
        this.useMedium = useMedium;
        this.useHigh = useHigh;

        kickScript.firstFrontAngle = frontAngle1;
        kickScript.secondFrontAngle = frontAngle2;
        kickScript.thirdFrontAngle = frontAngle3;
        kickScript.firstBackAngle = backAngle1;
        kickScript.secondBackAngle = backAngle2;
        kickScript.thirdBackAngle = backAngle3;

        mainPanel.SetActive(true);
        pausePanel.SetActive(true);

        isPlaying = true;
        NextMovement();
    }

    public void BallHit(int points)
    {
        GameManagerTiroLibre.gm.targetReady = false;
        UpdateScore(points);

        if (!withTime)
            UpdateRepetitions(1);
        else
            totalRepetitions += 1;
    }

    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
        boardScoreText.text = score.ToString();
    }

    public void EnableMovement(bool enable)
    {
        isAbleToMove = enable;
        kickScript.skeleton.updateRootX = enable;
    }

    public void NextMovement()
    {
        if (isPlaying && !isGameOver)
        {
            if (useShifts && lastLegMovement)
            {
                int choice = UnityEngine.Random.Range(1, 101);

                if (choice > (100 - shiftsFrequency))
                {
                    NextShiftMovement();
                }
                else
                {
                    NextLegMovement();
                } 
            }
            else if (lastShiftMovement)
            {
                NextShiftMovement();
            }
            else
            {
                NextLegMovement();
            }
        }
    }

    public void NextShiftMovement()
    {
        frontLeftLegPanel.SetActive(false);
        frontRightLegPanel.SetActive(false);
        backLeftLegPanel.SetActive(false);
        backRightLegPanel.SetActive(false);
        leftLegParticles1.Stop();
        leftLegParticles2.Stop();
        rightLegParticles1.Stop();
        rightLegParticles2.Stop();

        ShiftPlatforms lastPlatform = currentPlatform;

        if (lastShiftMovement)
        {
            currentPlatform = ShiftPlatforms.CenterPlatform;
            lastShiftMovement = false;
        }
        else
        {
            ArrayList values = new ArrayList(Enum.GetValues(typeof(ShiftPlatforms)));
            values.Remove(ShiftPlatforms.CenterPlatform);
            currentPlatform = (ShiftPlatforms)values[(random.Next(values.Count))];
            lastShiftMovement = true;
        }

        if (currentPlatform == ShiftPlatforms.LeftPlatform)
            shiftLeftPanel.SetActive(true);
        else if (currentPlatform == ShiftPlatforms.RightPlatform)
            shiftRightPanel.SetActive(true);
        else if (lastPlatform == ShiftPlatforms.LeftPlatform)
            shiftRightPanel.SetActive(true);
        else if (lastPlatform == ShiftPlatforms.RightPlatform)
            shiftLeftPanel.SetActive(true);        

        avatarPlatform.SetActive(true);

        switch (currentPlatform)
        {
            case ShiftPlatforms.LeftPlatform:
                {                    
                    leftShiftPlatform.SetActive(true);
                    centerShiftPlatform.SetActive(false);
                    rightShiftPlatform.SetActive(false);
                    //currentBallStartPosition.x = leftShiftPlatform.transform.position.x;
                    break;
                }
            case ShiftPlatforms.CenterPlatform:
                {
                    leftShiftPlatform.SetActive(false);
                    centerShiftPlatform.SetActive(true);
                    rightShiftPlatform.SetActive(false);
                    //currentBallStartPosition.x = centerShiftPlatform.transform.position.x;
                    break;
                }
            case ShiftPlatforms.RightPlatform:
                {
                    leftShiftPlatform.SetActive(false);
                    centerShiftPlatform.SetActive(false);
                    rightShiftPlatform.SetActive(true);
                    //currentBallStartPosition.x = rightShiftPlatform.transform.position.x;
                    break;
                }
            default:
                break;
        }

        lastLegMovement = false;
        EnableMovement(true);
    }

    public void ShiftDone()
    {
        avatarPlatform.SetActive(false);
        leftShiftPlatform.SetActive(false);
        centerShiftPlatform.SetActive(false);
        rightShiftPlatform.SetActive(false);
        shiftLeftPanel.SetActive(false);
        shiftRightPanel.SetActive(false);
        EnableMovement(false);

        NextMovement();
    }

    public void NextLegMovement()
    {
        NextTarget();
        NextLeg();

        lastLegMovement = true;
        lastShiftMovement = false;
    }

    public void NextTarget()
    {
        int nextTarget = RandomTarget();
        StartCoroutine(targets[nextTarget].GetComponent<TiroLibreTargetBehaviour>().DelayedShow(timeBetweenTargets));

        currentTarget = nextTarget;
        targetReady = true;
    }

    public int RandomTarget()
    {
        int nextTarget = 0;

        if (useLow && useMedium && useHigh)
        {
            nextTarget = UnityEngine.Random.Range(0, 9);
        }
        else if (useLow && useMedium)
        {
            nextTarget = UnityEngine.Random.Range(0, 6);
        }
        else if (useMedium && useHigh)
        {
            nextTarget = UnityEngine.Random.Range(3, 9);
        }
        else if (useLow && useHigh)
        {
            int choice = UnityEngine.Random.Range(1, 101);
            if (choice > 50)
                nextTarget = UnityEngine.Random.Range(6, 9);
            else
                nextTarget = UnityEngine.Random.Range(0, 3);
        }
        else if (useLow)
        {
            nextTarget = UnityEngine.Random.Range(0, 3);
        }
        else if (useMedium)
        {
            nextTarget = UnityEngine.Random.Range(3, 6);
        }
        else if (useHigh)
        {
            nextTarget = UnityEngine.Random.Range(6, 9);
        }

        return nextTarget;
    }

    public void NextLeg()
    {
        if (useFrontPlane && useBackPlane)
        {
            Array values = Enum.GetValues(typeof(LegMovements));
            currentMovement = (LegMovements)values.GetValue(random.Next(values.Length));
        }
        else if (useFrontPlane)
        {
            currentMovement = (LegMovements)frontPlane.GetValue(random.Next(frontPlane.Length));
        }
        else
        {
            currentMovement = (LegMovements)backPlane.GetValue(random.Next(backPlane.Length));
        }

        switch (currentMovement)
        {
            case LegMovements.FrontLeftLeg:
                {
                    frontLeftLegPanel.SetActive(true);
                    if (currentTarget == 2 || currentTarget == 5 || currentTarget == 8)
                        frontLeftLegPanel.GetComponent<Animator>().Play("FrontLeftLeg Panel Right");
                    else if (currentTarget == 1 || currentTarget == 4 || currentTarget == 7)
                        frontLeftLegPanel.GetComponent<Animator>().Play("FrontLeftLeg Panel Center");
                    else
                        frontLeftLegPanel.GetComponent<Animator>().Play("FrontLeftLeg Panel Left");
                    frontRightLegPanel.SetActive(false);
                    backLeftLegPanel.SetActive(false);
                    backRightLegPanel.SetActive(false);
                    leftLegParticles1.Play();
                    leftLegParticles2.Stop();
                    rightLegParticles1.Stop();
                    rightLegParticles2.Stop();
                    break;
                }
            case LegMovements.FrontRightLeg:
                {
                    frontLeftLegPanel.SetActive(false);
                    frontRightLegPanel.SetActive(true);
                    if (currentTarget == 2 || currentTarget == 5 || currentTarget == 8)
                        frontRightLegPanel.GetComponent<Animator>().Play("FrontRightLeg Panel Right");
                    else if (currentTarget == 1 || currentTarget == 4 || currentTarget == 7)
                        frontRightLegPanel.GetComponent<Animator>().Play("FrontRightLeg Panel Center");
                    else
                        frontRightLegPanel.GetComponent<Animator>().Play("FrontRightLeg Panel Left");
                    backLeftLegPanel.SetActive(false);
                    backRightLegPanel.SetActive(false);
                    leftLegParticles1.Stop();
                    leftLegParticles2.Stop();
                    rightLegParticles1.Play();
                    rightLegParticles2.Stop();
                    break;
                }
            case LegMovements.BackLeftLeg:
                {
                    frontLeftLegPanel.SetActive(false);
                    frontRightLegPanel.SetActive(false);
                    backLeftLegPanel.SetActive(true);
                    if (currentTarget == 2 || currentTarget == 5 || currentTarget == 8)
                        backLeftLegPanel.GetComponent<Animator>().Play("BackLeftLeg Panel Right");
                    else if (currentTarget == 1 || currentTarget == 4 || currentTarget == 7)
                        backLeftLegPanel.GetComponent<Animator>().Play("BackLeftLeg Panel Center");
                    else
                        backLeftLegPanel.GetComponent<Animator>().Play("BackLeftLeg Panel Left");
                    backRightLegPanel.SetActive(false);
                    leftLegParticles1.Stop();
                    leftLegParticles2.Play();
                    rightLegParticles1.Stop();
                    rightLegParticles2.Stop();
                    break;
                }
            case LegMovements.BackRightLeg:
                {
                    frontLeftLegPanel.SetActive(false);
                    frontRightLegPanel.SetActive(false);
                    backLeftLegPanel.SetActive(false);
                    backRightLegPanel.SetActive(true);
                    if (currentTarget == 2 || currentTarget == 5 || currentTarget == 8)
                        backRightLegPanel.GetComponent<Animator>().Play("BackRightLeg Panel Right");
                    else if (currentTarget == 1 || currentTarget == 4 || currentTarget == 7)
                        backRightLegPanel.GetComponent<Animator>().Play("BackRightLeg Panel Center");
                    else
                        backRightLegPanel.GetComponent<Animator>().Play("BackRightLeg Panel Left");
                    leftLegParticles1.Stop();
                    leftLegParticles2.Stop();
                    rightLegParticles1.Stop();
                    rightLegParticles2.Play();
                    break;
                }
            default:
                break;
        }
    }

    public void UpdateRepetitions(int repetitionsDone)
    {
        remainingRepetitions -= repetitionsDone;
        repetitionsText.text = remainingRepetitions.ToString();

        if (remainingRepetitions <= 0)
        {
            isPlaying = false;
            isGameOver = true;
        }
    }

    public void EndGame()
    {
        mainPanel.SetActive(false);
        StartCoroutine(EndGameAnimation());       
    }

    private IEnumerator EndGameAnimation()
    {
        cameraAnimator.Play("Camera Final Animation");
        yield return new WaitForSeconds(6f);

        SaveAndShowResults();
    }

    public void SaveAndShowResults()
    {
        TherapySessionObject objTherapy = TherapySessionObject.tso;

        if (objTherapy != null)
        {
            objTherapy.fillLastSession(score, totalRepetitions, (int)totalTime, level.ToString());
            objTherapy.saveLastGameSession();

            objTherapy.savePerformance((int)kickScript.BestLeftHipFrontAngle, "4");
            objTherapy.savePerformance((int)kickScript.BestRightHipFrontAngle, "5");
        }
        
        int finalScore;        
        if (totalRepetitions > 0)
            finalScore = (int)(((float)score / totalRepetitions) * 100.0f);
        else
            finalScore = 0;
        resultsScoreText.text = "Desempeño: " + finalScore + "%";

        if (objTherapy != null)
            resultsBestScoreText.text = "Mejor: " + objTherapy.getGameRecord() + "%";

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

        resultsPanel.SetActive(true);

        //playlist block
        if (PlaylistManager.pm != null && PlaylistManager.pm.active)
            PlaylistManager.pm.NextGame();
    }

    public GameObject[] getCurrentTargets()
    {
        return targets;
    }

    public Vector3 getTargetPosition(int target)
    {
        return targets[target].transform.position;
    }

    public void EnableTarget(int target, bool enable)
    {
        targets[target].GetComponent<TiroLibreTargetBehaviour>().EnableTarget(enable);
    }
}
