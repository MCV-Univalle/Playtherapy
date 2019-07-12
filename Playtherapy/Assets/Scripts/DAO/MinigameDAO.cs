using UnityEngine;
using System.Collections.Generic;
using Npgsql;

public class MinigameDAO : MonoBehaviour
{
    // returns null if error
    public static Minigame ConsultMinigame(string id)
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = string.Format("SELECT * FROM start_minigame WHERE id = {0};", id);
            dbcmd.CommandText = sql;

            NpgsqlDataReader reader = dbcmd.ExecuteReader();
            if (reader.Read())
            {
                //string id = (int)reader["id"];
                string name = (string)reader["name"];
                string description = (string)reader["description"];

                Minigame minigame = new Minigame(id, name, description);

                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                return minigame;
            }
            else
            {
                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                return null;
            }
        }
        else
        {
            Debug.Log("Database connection not established");
            return null;
        }
    }

    // returns null if error
    public static List<Minigame> ConsultMinigames()
    {
        List<Minigame> minigames = null;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = "SELECT * FROM start_minigame;";
            dbcmd.CommandText = sql;

            minigames = new List<Minigame>();

            NpgsqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                string id = (string)reader["id"];
                string name = (string)reader["nombre"];
                string description = (string)reader["descripcion"];

                Minigame minigame = new Minigame(id, name, description);
                minigames.Add(minigame);

            }

            // clean up
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;

            return minigames;
        }
        else
        {
            Debug.Log("Database connection not established");
            return null;
        }
    }
}
