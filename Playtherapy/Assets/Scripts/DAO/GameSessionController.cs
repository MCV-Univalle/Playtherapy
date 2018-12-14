using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameSessionController 
{
    
    public GameObject GameSessionDAO;
    public GameObject GameSession;
    

    public int addGameSession(int score , float repititions, float time, int level , string minigame)
    {
        Debug.Log("sddsd");

        GameSession game = new GameSession(minigame);
        GameSessionDAO gameDao = new GameSessionDAO();
        Debug.Log("aqui");


        int repetitionsInt = (int)repititions;
        int timeInt = (int)time;
        game.Score = score;
        game.Repetitions = repetitionsInt;
        game.Time = timeInt;
        game.Level1 = level;

        int result = gameDao.InsertGameSessions(game);

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
