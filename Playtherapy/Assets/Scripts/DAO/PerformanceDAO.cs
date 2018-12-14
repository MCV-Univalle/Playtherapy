using UnityEngine;
using System.Collections;
using Npgsql;

public class PerformanceDAO
{

    public GameObject Performance;

    public int InsertToSPerformance(Performance performance)
    {
        bool exito = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            try
            {
                string save_sql = "INSERT INTO start_performance VALUES (NEXTVAL ('start_performance_id_seq'), "
                    + performance.Angle + ","
                    + this.GetLastGameSessionId() + ","
                    + performance.Movement_id + ");";
                Debug.Log(save_sql);

                dbcmd.CommandText = save_sql;
                dbcmd.ExecuteNonQuery();

                Debug.Log("GGIZI");
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

        return -1;
    }


    public static bool InsertPerformance(Performance performance)
    {
        bool exito = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            try
            {
                string sql = string.Format("INSERT INTO start_performance (angle, game_session_id, movement_id) VALUES ({0}, {1}, '{2}');",
                    performance.Angle, performance.Game_session_id, performance.Movement_id);

                Debug.Log(sql);

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

    public int GetLastGameSessionId()
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = ("SELECT MAX (id) as id FROM start_gamesession;");
            dbcmd.CommandText = sql;

            NpgsqlDataReader reader = dbcmd.ExecuteReader();
            if (reader.Read())
            {
                //string numero_doc = (int)reader["id_num"];
                int id = (int)reader["id"];

                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                return id;
            }
            else
            {
                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                Debug.Log("Error de consulta o elemento no encontrado");
                return 0;
            }
        }
        else
        {
            Debug.Log("Database connection not established");
            return 0;
        }
    }
}

