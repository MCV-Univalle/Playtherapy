using UnityEngine;
using System.Collections;
using Npgsql;

public class TherapistDAO
{
    // returns null if error
    public static Therapist ConsultTherapist(string id_num)
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

			string sql = string.Format("SELECT * FROM therapist_therapist, auth_user WHERE therapist_therapist.user_ptr_id = auth_user.id and auth_user.username = '{0}';", id_num);
            dbcmd.CommandText = sql;

            NpgsqlDataReader reader = dbcmd.ExecuteReader();
            if (reader.Read())
            {
                //string numero_doc = (int)reader["id_num"];
                string id_type = (string)reader["id_type"];
                string name = (string)reader["name"];
                string lastname = (string)reader["lastname"];
                string genre = (string)reader["genre"];
                //string password = (string)reader["password"];

                Therapist therapist = new Therapist(id_num, id_type, name, lastname, genre);

                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                Debug.Log("Name: " + name + " " + lastname);
                return therapist;
            }
            else
            {
                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                Debug.Log("Error de consulta o elemento no encontrado");
                return null;
            }
        }
        else
        {
            Debug.Log("Database connection not established");
            return null;
        }
    }

    public int GetIdTherapy(string idTherapy)
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = ("SELECT id FROM auth_user WHERE username =  '" + idTherapy + "';");
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
