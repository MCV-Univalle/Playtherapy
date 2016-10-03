﻿using UnityEngine;
using System.Collections;
using Npgsql;

public class TherapySessionDAO
{
    public static bool InsertTherapySession(TherapySession therapy)
    {
        bool exito = false;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            try
            {
                string sql = string.Format("INSERT INTO therapy_session VALUES ('{1}', '{2}', '{3}', '{4}', '{5}');",
                    therapy.Fecha, therapy.Objetivos, therapy.Observaciones, therapy.Numero_doc_Therapist, therapy.Numero_doc_Patient);

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
