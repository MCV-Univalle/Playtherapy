using UnityEngine;
using System.Collections;

public class Performance
{
    //public string id;
    private int angle;
    private string movement_id;
    private string game_session_id;

    public Performance(int angle, string movement_id, string game_session_id)
    {
        this.Angle = angle;
        this.Movement_id = movement_id;
        this.Game_session_id = game_session_id;
    }
    public Performance()
    {

    }

    public int Angle
    {
        get
        {
            return angle;
        }

        set
        {
            angle = value;
        }
    }

    public string Movement_id
    {
        get
        {
            return movement_id;
        }

        set
        {
            movement_id = value;
        }
    }

    public string Game_session_id
    {
        get
        {
            return game_session_id;
        }

        set
        {
            game_session_id = value;
        }
    }
}
