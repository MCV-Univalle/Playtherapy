using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
// for your own scripts make sure to add the following line:
using DigitalRuby.Tween;
public class ManagerGreatJourney : MonoBehaviour
{


    public static ManagerGreatJourney gm;

    Animator tutorial_movements;
    GameObject paramenters_canvas;
    GameObject results_canvas;
    GameObject tutorial_canvas;
    GameObject count_objects_canvas;
    GameObject tutorial_page_info;
    GameObject cam;
    //GameObject timerUI;
    Slider timeSlider;
    public GameObject timerUI;

    Vector3 cam_initial_pos;
    Quaternion cam_initial_rot;

    Text txt_rubies;
    Text txt_dodge;
    Text txt_time;


    SpannerOfMovements spanner;
    PutDataResults results_script;
    ScoreHandler score_script;
    PlaneController planeControler;


    List<MonoBehaviour> array_scrips_disabled;
    List<GameObject> tutorial_pages_array;
    List<GameObject> array_arrows;

    TinyPauseScript pausa;
    public string movement;
    public float finalTotalRepetition;
    public float finalTotalTime;
    bool game_over;
    bool hasStart;
    float timer_game = -1;
    float timeMillis;
    int tutorial_page = 0;

    // Use this for initialization
    void Start()
    {

        if (gm == null)
        {
            gm = this;
        }


        pausa = FindObjectOfType<TinyPauseScript>();

        hasStart = false;
        game_over = false;

        spanner = FindObjectOfType<SpannerOfMovements>();
        planeControler = FindObjectOfType<PlaneController>();
        score_script = FindObjectOfType<ScoreHandler>();
        results_script = FindObjectOfType<PutDataResults>();


        array_scrips_disabled = new List<MonoBehaviour>();
        array_scrips_disabled.Add(spanner);
        array_scrips_disabled.Add(FindObjectOfType<SpanwClouds>());
        array_scrips_disabled.Add(planeControler);


        paramenters_canvas = GameObject.Find("parameters_canvas");
        results_canvas = GameObject.Find("results_canvas");
        tutorial_canvas = GameObject.Find("tutorial_canvas");
        timerUI.SetActive(true);
        count_objects_canvas = GameObject.Find("count_objects_canvas");
        timeSlider = GameObject.Find("slideTimeUI").GetComponent<Slider>();

        cam = GameObject.Find("PlayGameCamera");
        cam_initial_pos = cam.transform.parent.transform.position;
        cam_initial_rot = cam.transform.parent.transform.rotation;
        txt_rubies = GameObject.Find("txt_rubies").GetComponent<Text>();
        txt_dodge = GameObject.Find("txt_dodge").GetComponent<Text>();
        txt_time = GameObject.Find("txt_timer").GetComponent<Text>();
        tutorial_movements = GameObject.Find("anim_moves").GetComponent<Animator>();

        array_arrows = new List<GameObject>();

        array_arrows.Add(GameObject.Find("left_img"));
        array_arrows.Add(GameObject.Find("right_img"));
        array_arrows.Add(GameObject.Find("down_img"));
        timeMillis = 1000f;

        timeSlider.transform.parent.gameObject.SetActive(false);
        results_canvas.transform.localScale = Vector3.zero;
        tutorial_canvas.transform.localScale = Vector3.zero;
        tutorial_pages_array = new List<GameObject>();

        if (PlaylistManager.pm == null || (PlaylistManager.pm != null && !PlaylistManager.pm.active))
        {
            paramenters_canvas.SetActive(true);
            //MainPanel.SetActive(false);
        }



        int contador = 0;


        do
        {


            contador++;
            tutorial_page_info = GameObject.Find("tutorial_page" + contador);

            if (tutorial_page_info != null)
            {
                tutorial_pages_array.Add(tutorial_page_info);
                tutorial_page_info.SetActive(false);
            }

        } while (tutorial_page_info != null);



        // esto para activar el panel de parámetros en caso de que no se esté en playlist
        if (PlaylistManager.pm == null || (PlaylistManager.pm != null && !PlaylistManager.pm.active))
        {
            TweenShowParameters();
        }





    }
    void updateTimeText()
    {

        if (timer_game > 0)
        {

            timeMillis -= Time.deltaTime * 1000;
            if (timeMillis < 0)
            {
                timeMillis = 1000f;
            }

            txt_time.text = (((int)timer_game) / 60).ToString("00") + ":"
                + (((int)timer_game) % 60).ToString("00") + ":"
                + ((int)(timeMillis * 60 / 1000)).ToString("00");




        }
        else
        {
            txt_time.text = "00:00:00";
        }


    }
    void Update()
    {


        if (hasStart == true)
        {
            if (game_over == false)
            {
                if (HoldParametersGreatJourney.use_time == true)
                {
                    if (timer_game > 0)
                    {

                        timer_game -= Time.deltaTime;

                        timeSlider.value = (timer_game / (HoldParametersGreatJourney.select_jugabilidad * 60)) * 100;


                    }
                    else
                    {
                        timer_game = 0;
                        timeSlider.value = 0;
                        game_over = true;
                        EndGame();
                    }
                    updateTimeText();


                }
                else
                {
                    //print ("repeticiones_restantes:"+HoldParametersGreatJourney.repeticiones_restantes +", planes:"+spanner.PlanesParentArray.transform.childCount);
                    if (HoldParametersGreatJourney.repeticiones_restantes == 0 && spanner.PlanesParentArray.transform.childCount == 0)
                    {
                        game_over = true;
                        //print ("termino gameplay");
                        TweenFinishGame();

                    }
                }
            }
        }

    }
    public void EndGame()
    {
        //saveData ();

        int performance_game = Mathf.RoundToInt(((float)score_script.score_obtain / (float)score_script.score_max) * 100);
        int performance_loaded_BD = 0;
        results_script.updateData(performance_game, performance_loaded_BD);
        string idMinigame = "3";

        hasStart = false;
        //paramenters_canvas.SetActive (true);
        foreach (MonoBehaviour behaviour in array_scrips_disabled)
        {
            behaviour.enabled = false;
        }
        FinalAnimation();
        int angle = (int)HoldParametersGreatJourney.select_angle_max;
        GameSessionController gameCtrl = new GameSessionController();

        if (HoldParametersGreatJourney.use_time == true)
        {
            float repetitionsC = 0;
            gameCtrl.addGameSession(performance_game, repetitionsC, HoldParametersGreatJourney.select_jugabilidad, score_script.score_obtain, idMinigame);
            PerformanceController performanceCtrl = new PerformanceController();
            performanceCtrl.addPerformance(angle, this.GetMovement());
        }
        if (HoldParametersGreatJourney.use_time == false)
        {
            float repetitionsC = 0;
            gameCtrl.addGameSession(performance_game, HoldParametersGreatJourney.select_jugabilidad, repetitionsC, score_script.score_obtain, idMinigame);
            PerformanceController performanceCtrl = new PerformanceController();
            performanceCtrl.addPerformance(angle, this.GetMovement());
        }



        if (PlaylistManager.pm != null && PlaylistManager.pm.active)
        {
            PlaylistManager.pm.NextGame();
        }

    }
    public void RetryGame()
    {
        TweenHideResults();
        TweenShowParameters();



    }
    public void StartGame(bool use_time = true, int select_jugabilidad = 10, int select_movimiento = HoldParametersGreatJourney.MOVIMIENTO_MIEMBROS_INFERIORES, int lados_involucrados = HoldParametersGreatJourney.LADO_TODOS, float angle_min = 5, float angle_min_frontal = 10, float angle_max = 20, float sostener = 2, float descanso = 2)
    {

        StartGame();

    }
    public void StartGame()
    {
        this.SetMovement(HoldParametersGreatJourney.select_movimiento);
        //FinalTotalRepetition = HoldParametersGreatJourney.select_jugabilidad;
        //FinalTotalTime = HoldParametersGreatJourney.select_jugabilidad;
        timerUI.SetActive(true);
        timer_game = -1;
        game_over = false;
        hasStart = true;
        timeSlider.value = 100;
        score_script.reset();
        planeControler.resetData();
        planeControler.resetPositionsSpineBase();

        foreach (MonoBehaviour behaviour in array_scrips_disabled)
        {
            behaviour.enabled = true;
        }

        if (HoldParametersGreatJourney.use_time == true)
        {
            timer_game = HoldParametersGreatJourney.select_jugabilidad * 60;

        }
        else
        {
            switch (HoldParametersGreatJourney.lados_involucrados)
            {
                case HoldParametersGreatJourney.LADO_TODOS:
                    HoldParametersGreatJourney.repeticiones_restantes = (int)HoldParametersGreatJourney.select_jugabilidad * 3;
                    break;
                case HoldParametersGreatJourney.LADO_IZQ_DER:
                    HoldParametersGreatJourney.repeticiones_restantes = (int)HoldParametersGreatJourney.select_jugabilidad * 2;
                    break;
                case HoldParametersGreatJourney.LADO_DERECHO:
                    HoldParametersGreatJourney.repeticiones_restantes = (int)HoldParametersGreatJourney.select_jugabilidad;
                    break;
                case HoldParametersGreatJourney.LADO_IZQUIERDO:
                    HoldParametersGreatJourney.repeticiones_restantes = (int)HoldParametersGreatJourney.select_jugabilidad;
                    break;
                case HoldParametersGreatJourney.LADO_ABAJO:
                    HoldParametersGreatJourney.repeticiones_restantes = (int)HoldParametersGreatJourney.select_jugabilidad;
                    break;

                default:
                    break;
            }

        }
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //cam.transform.LookAt (planeControler.transform.position);
        cam.transform.parent.position = cam_initial_pos;
        cam.transform.parent.rotation = cam_initial_rot;
        spanner.setup();
        TweenHideParameters();

    }
    private void saveData()
    {
        GameObject tre = GameObject.Find("TherapySession");

        if (tre != null)
        {
            TherapySessionObject objTherapy = tre.GetComponent<TherapySessionObject>();

            if (objTherapy != null)
            {

                objTherapy.fillLastSession(score_script.score_obtain, score_script.score_max, (int)0, "1");
                objTherapy.saveLastGameSession();

                objTherapy.savePerformance((int)HoldParametersGreatJourney.best_angle_left, "14");



            }
        }




        //		TherapySessionObject objTherapy = GameObject.Find ("TherapySession").GetComponent<TherapySessionObject> ();
        //		objTherapy.fillLastSession(score, currentReps, (int)totalTime, level.ToString());
        //		objTherapy.saveLastGameSession ();
        //
        //		objTherapy.savePerformance((int)bestTotalLeftShoulderAngle, "4");
        //		objTherapy.savePerformance((int)bestTotalRightShoulderAngle, "5");

    }
    private void FinalAnimation()
    {
        txt_rubies.text = "x" + score_script.rubies_caught;
        txt_dodge.text = "x" + score_script.airplanes_dodge;
        TweenShowCountObjets();
        TweenRotatePlane();
        TweenRotateCamera();

    }
    private void TweenRotatePlane()
    {
        float startAngle = planeControler.transform.rotation.eulerAngles.z;
        float endAngle = startAngle + 720.0f;
        planeControler.gameObject.Tween("RotateAiplane", startAngle, endAngle, 2.0f, TweenScaleFunctions.CubicEaseInOut, (t) =>
        {
            // progress
            planeControler.transform.rotation = Quaternion.identity;
            planeControler.transform.Rotate(planeControler.transform.forward, t.CurrentValue);
        }, (t) =>
        {
            //completion
            TweenHideCountObjets();
            TweenShowResults();
        });
    }
    private void TweenRotateCamera()
    {

        cam.gameObject.Tween("RotateCamera", 0, 100, 2.0f, TweenScaleFunctions.CubicEaseInOut, (t) =>
        {
            // progress
            cam.transform.RotateAround(planeControler.transform.position, Vector3.up, 2 * ((100 - t.CurrentValue) / 100));
            //cam.transform.rotation = Quaternion.identity;
            //cam.transform.Rotate(planeControler.transform.forward, t.CurrentValue);
        }, (t) =>
        {
            //completion
        });
    }
    private void TweenShowResults()
    {
        results_canvas.transform.localScale = Vector3.zero;
        this.gameObject.Tween("ShowResults", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            results_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {
            // esto para verificar si hay una playlist y reproducir el siguiente juego
            if (PlaylistManager.pm != null && PlaylistManager.pm.active)
                PlaylistManager.pm.NextGame();

        });



    }
    private void TweenHideResults()
    {
        results_canvas.transform.localScale = Vector3.one;
        this.gameObject.Tween("HideResults", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            results_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {
            //complete
        });



    }
    private void TweenShowCountObjets()
    {
        count_objects_canvas.transform.localScale = Vector3.zero;
        this.gameObject.Tween("ShowCountObjets", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            count_objects_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {
            //complete
        });



    }
    private void TweenHideCountObjets()
    {
        count_objects_canvas.transform.localScale = Vector3.one;
        this.gameObject.Tween("HideCountObjets", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            count_objects_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {
            //complete
        });



    }
    public void putPageTutorial()
    {

        foreach (GameObject obj in tutorial_pages_array)
        {
            obj.SetActive(false);
        }


        if (tutorial_page < tutorial_pages_array.Count)
        {
            tutorial_page_info = tutorial_pages_array[tutorial_page];
            tutorial_page_info.SetActive(true);

            PutRespectiveTextTutorial();

        }
        else
        {
            CloseTutorial();
        }
        tutorial_page++;

    }
    private void PutRespectiveTextTutorial()
    {
        if (tutorial_page == 1)
        {


            Debug.Log("lenght:" + array_arrows.Count);
            foreach (GameObject obj in array_arrows)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }


            List<string> anims = new List<string>();

            anims.Add("limbs_all");
            anims.Add("limbs_izq_der");
            anims.Add("limbs_down");
            anims.Add("trunck_all");
            anims.Add("trunck_izq_der");
            anims.Add("trunck_down");


            for (int i = 0; i < anims.Count; i++)
            {
                tutorial_movements.SetBool(anims[i], false);
            }

            // pagina en donde explicaremos que movimientos deben hacer 
            Text txt_mover = GameObject.Find("txt_mover").GetComponent<Text>();
            switch (HoldParametersGreatJourney.select_movimiento)
            {
                case HoldParametersGreatJourney.MOVIMIENTO_MIEMBROS_INFERIORES:

                    switch (HoldParametersGreatJourney.lados_involucrados)
                    {

                        case HoldParametersGreatJourney.LADO_TODOS:
                            foreach (GameObject obj in array_arrows)
                            {
                                if (obj != null)
                                {
                                    obj.SetActive(true);
                                }

                            }
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_DISOCIACION_MIEMBROS_INFERIORES;
                            tutorial_movements.SetBool(anims[0], true);
                            break;
                        case HoldParametersGreatJourney.LADO_IZQ_DER:
                            array_arrows[0].SetActive(true);
                            array_arrows[1].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_MIEMBROS_INFERIORES_LATERAL;
                            tutorial_movements.SetBool(anims[1], true);
                            break;
                        case HoldParametersGreatJourney.LADO_DERECHO:
                            array_arrows[0].SetActive(true);
                            array_arrows[1].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_MIEMBROS_INFERIORES_LATERAL;
                            tutorial_movements.SetBool(anims[1], true);
                            break;
                        case HoldParametersGreatJourney.LADO_IZQUIERDO:
                            array_arrows[0].SetActive(true);
                            array_arrows[1].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_MIEMBROS_INFERIORES_LATERAL;
                            tutorial_movements.SetBool(anims[1], true);
                            break;

                        case HoldParametersGreatJourney.LADO_ABAJO:
                            array_arrows[2].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_MIEMBROS_INFERIORES_SENTADILLAS;
                            tutorial_movements.SetBool(anims[2], true);
                            break;
                        default:
                            break;
                    }
                    break;


                case HoldParametersGreatJourney.MOVIMIENTO_TRONCO:

                    switch (HoldParametersGreatJourney.lados_involucrados)
                    {

                        case HoldParametersGreatJourney.LADO_TODOS:
                            foreach (GameObject obj in array_arrows)
                            {
                                obj.SetActive(true);
                            }
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_DISOCIACION_TRONCO;
                            tutorial_movements.SetBool(anims[3], true);
                            break;
                        case HoldParametersGreatJourney.LADO_IZQ_DER:
                            array_arrows[0].SetActive(true);
                            array_arrows[1].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_TRONCO_LATERAL;
                            tutorial_movements.SetBool(anims[4], true);
                            break;
                        case HoldParametersGreatJourney.LADO_DERECHO:
                            array_arrows[0].SetActive(true);
                            array_arrows[1].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_TRONCO_LATERAL;
                            tutorial_movements.SetBool(anims[4], true);
                            break;
                        case HoldParametersGreatJourney.LADO_IZQUIERDO:
                            array_arrows[0].SetActive(true);
                            array_arrows[1].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_TRONCO_LATERAL;
                            tutorial_movements.SetBool(anims[4], true);
                            break;

                        case HoldParametersGreatJourney.LADO_ABAJO:
                            array_arrows[2].SetActive(true);
                            txt_mover.text = GlosarioGreatJourney.MOVER_CON_TRONCO_ANTERIOR;
                            tutorial_movements.SetBool(anims[5], true);
                            break;
                        default:
                            break;
                    }


                    break;

                default:
                    break;
            }

        }

    }
    private void CloseTutorial()
    {
        if (hasStart == false)
        {
            TweenHideTutorial();
            TweenShowParameters();
        }
        else
        {
            tutorial_canvas.transform.localScale = Vector3.zero;
            pausa.gameObject.SetActive(true);
        }



    }
    public void OpenTutorial()
    {

        tutorial_page = 0;
        putPageTutorial();

        if (hasStart == false)
        {
            TweenShowTutorial();
            TweenHideParameters();
        }
        else
        {
            tutorial_canvas.transform.localScale = Vector3.one;
            pausa.gameObject.SetActive(false);
        }

    }
    private void TweenShowTutorial()
    {
        tutorial_canvas.transform.localScale = Vector3.zero;
        this.gameObject.Tween("ShowTutorial", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            tutorial_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {

            //complete
        });



    }
    private void TweenHideTutorial()
    {
        tutorial_canvas.transform.localScale = Vector3.one;
        this.gameObject.Tween("HideTutorial", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            tutorial_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {
            //complete
        });



    }
    private void TweenShowParameters()
    {
        paramenters_canvas.transform.localScale = Vector3.zero;
        this.gameObject.Tween("ShowParameters", Vector3.zero, Vector3.one, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            paramenters_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {
            //complete
        });



    }
    private void TweenHideParameters()
    {
        paramenters_canvas.transform.localScale = Vector3.one;
        this.gameObject.Tween("HideParameters", Vector3.one, Vector3.zero, 0.75f, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress
            paramenters_canvas.transform.localScale = t.CurrentValue;

        }, (t) =>
        {
            //complete
        });



    }
    private void TweenFinishGame(float time = 2f)
    {
        this.gameObject.Tween("FinishGameJourney", this.transform.position.x, this.transform.position.x, time, TweenScaleFunctions.QuadraticEaseOut, (t) =>
        {
            // progress


        }, (t) =>
        {
            //complete time
            EndGame();
        });
    }
    public float FinalTotalTime
    {
        get
        {
            return finalTotalTime;
        }

        set
        {
            finalTotalTime = value;
        }
    }

    public float FinalTotalRepetition
    {
        get
        {
            return finalTotalRepetition;
        }

        set
        {
            finalTotalRepetition = value;
        }
    }

    public string GetMovement()
    {
        return movement;
    }
    public void SetMovement(float movementX)
    {

        if (movementX == 0)
        {

            movement = "12";
        }
        if (movementX == 1)
        {
            movement = "13";
        }
    }
}

