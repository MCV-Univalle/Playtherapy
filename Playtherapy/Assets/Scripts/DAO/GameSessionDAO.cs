﻿using UnityEngine;
using System.Collections;
using Npgsql;

public class GameSessionDAO
{
    public static bool InsertGameSession(GameSession game)
    {
        bool exito = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            try
            {
                string sql = string.Format("INSERT INTO game_session VALUES ('{1}', {2}, {3}, {4}, '{5}', '{6}');",
                    game.Fecha, game.Puntaje, game.Repeticiones, game.Tiempo, game.Nivel, game.Id_minigame);

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                //Debug.Log("");
                exito = true;
            }
            catch (NpgsqlException ex)
            {
                Debug.Log(ex.Message);
            }

            // clean up
            dbcmd.Dispose();
            dbcmd = null;
        }
        else
        {
            Debug.Log("Database connection not established");
        }

        return exito;
    }
}
