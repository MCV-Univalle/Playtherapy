using UnityEngine;
using System.Collections;
using Npgsql;

public class TherapySessionDAO
{
    public TherapySession therapySession;

    public TherapySessionDAO()
    {


    }

    public static bool InsertTherapySession(TherapySession therapy)
    {
        bool exito = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();


            try
            {
                string sql = string.Format("INSERT INTO start_therapysession (date, objective, description, patient_id, therapist_id) VALUES ('{0}', '{1}', '{2}', (SELECT id FROM patient_patient WHERE id_num = '{3}'), (SELECT user_ptr_id FROM therapist_therapist, auth_user WHERE therapist_therapist.user_ptr_id = auth_user.id and auth_user.username = '{4}'));",
                    therapy.Date, therapy.Objective, therapy.Description, therapy.Patient_id, therapy.Therapist_id);
                Debug.Log(sql);

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

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

    public static bool insertObservations(string therapyId, string observations)
    {
        bool success = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();


            try
            {
                string sql = string.Format("UPDATE start_therapysession SET description = {0} WHERE id = {1};", observations, therapyId);

                dbcmd.CommandText = sql;
                dbcmd.ExecuteNonQuery();

                //Debug.Log("");
                success = true;
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

        return success;
    }

    public static int GetLastTherapyId(string id_patient)
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = ("SELECT id FROM start_therapysession WHERE id = (SELECT max(id) FROM start_therapysession WHERE patient_id = (SELECT id FROM patient_patient WHERE id_num = '" + id_patient + "'));");
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

    public int InsertTherapySessions(TherapySession therapy)
    {
        bool exito = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            try
            {
                string save_sql = "INSERT INTO start_therapysession VALUES (NEXTVAL ('start_therapysession_id_seq'), '"
                        + therapy.Date + "','"
                        + therapy.Objective + "','"
                        + therapy.Description + "','"
                        + therapy.Patient_id + "','"
                        + therapy.Therapist_id + "');";

                Debug.Log(save_sql);
                dbcmd.CommandText = save_sql;
                dbcmd.ExecuteNonQuery();

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



}
