using UnityEngine;
using System.Collections;
using System;

public class GameSession
{
    private string date;
    private int score;
    private int repetitions;
    private int time;
    private string level;
    private string minigame_id;
    private string therapy_id;
    private int level1;


    public GameSession(string minigame_id)
    {
        date = DateTime.Now.ToString("yyyy-MM-dd");
        this.minigame_id = minigame_id;
        Debug.Log("fecha: " + date);
    }

    public string Date
    {
        get
        {
            return date;
        }

        set
        {
            date = value;
        }
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

    public int Repetitions
    {
        get
        {
            return repetitions;
        }

        set
        {
            repetitions = value;
        }
    }

    public int Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }
    public string Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public int Level1
    {
        get
        {
            return level1;
        }

        set
        {
            level1 = value;
        }
    }

    public string Minigame_id
    {
        get
        {
            return minigame_id;
        }

        set
        {
            minigame_id = value;
        }
    }
    public string Therapy_id
    {
        get
        {
            return therapy_id;
        }

        set
        {
            therapy_id = value;
        }
    }

}
