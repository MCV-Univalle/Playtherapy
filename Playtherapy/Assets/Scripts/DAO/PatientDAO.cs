using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Npgsql;
using System;

public class PatientDAO
{
    // returns null if error

    public PatientDAO()
    {

    }

    public static Patient ConsultPatient(string id_num)
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

			string sql = ("SELECT * FROM patient_patient WHERE id_num = '" + id_num.ToString() + "';");
            dbcmd.CommandText = sql;

            NpgsqlDataReader reader = dbcmd.ExecuteReader();
            if (reader.Read())
            {
                //string numero_doc = (int)reader["id_num"];
                string id_type = (string)reader["id_type"];
                string name = (string)reader["name"];
                string lastname = (string)reader["lastname"];
                string genre = (string)reader["genre"];
                string occupation = (string)reader["occupation"];
                string birthday = ((DateTime)reader["birthday"]).ToString();                

                Patient patient = new Patient(id_num, id_type, name, lastname, genre, occupation, birthday);

                // clean up
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;

                Debug.Log("Name: " + name + " " + lastname);
                return patient;                
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

    // returns null if error
    public static List<Patient> ConsultPatients()
    {
        List<Patient> patients = null;

        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = "SELECT * FROM patient_patient;";
            dbcmd.CommandText = sql;

            patients = new List<Patient>();

            NpgsqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                string id_num = (string)reader["numero_doc"];
                string id_type = (string)reader["tipo_doc"];
                string name = (string)reader["nombre"];
                string lastname = (string)reader["apellido"];
                string genre = (string)reader["genero"];
                string occupation = (string)reader["ocupacion"];
                string birthday = (string)reader["fecha_nacimiento"];

                Patient patient = new Patient(id_num, id_type, name, lastname, genre, occupation, birthday);
                patients = new List<Patient>();
                patients.Add(patient);

                Debug.Log("Name: " + name + " " + lastname);                
            }

            // clean up
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;

            return patients;
        }
        else
        {
            Debug.Log("Database connection not established");
            return null;
        }
    }

    public int GetIdPatient(string idPatient)
    {
        if (DBConnection.dbconn != null)
        {
            NpgsqlCommand dbcmd = DBConnection.dbconn.CreateCommand();

            string sql = ("SELECT id FROM patient_patient WHERE id_num =  '" + idPatient + "';");
            Debug.Log(sql);
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
