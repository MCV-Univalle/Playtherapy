using UnityEngine;
using System.Collections;
using Npgsql;

using System;

public class DBConnection : MonoBehaviour
{
    public string host = "127.0.0.1";
    public string username = "postgres";
    public string password = "postgres";
    public string database = "playtherapy";

    public Animator cameraAnimator;

    public static NpgsqlConnection dbconn = null;

    private static DBConnection dbConnectionObject;

    public static DBConnection Instance()
    {
        if (!dbConnectionObject)
        {
            dbConnectionObject = FindObjectOfType(typeof(DBConnection)) as DBConnection;
            if (!dbConnectionObject)
                Debug.Log("There needs to be one active DBConnection script on a GameObject in your scene.");
        }

        return dbConnectionObject;
    }

    // Use this for initialization
    void Start()
    {
        Connect();
        
    }
    


    public void Connect()
    {
        string connectionString = 
            "Host=" + host + ";" + 
            "Username=" + username + ";" +
            "Password=" + password + ";" +
            "Database=" + database;

       

        try
        {
            dbconn = new NpgsqlConnection(connectionString);
            dbconn.Open();
            Debug.Log("Succesfully connected to the database");

            if (cameraAnimator != null)
                cameraAnimator.enabled = true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);

            ModalWindowMaker.Instance().DBConnectionError(host, username, password, database);
        }        
    }

    public void Connect(string host, string username, string password, string database)
    {
        string connectionString =
            "Host=" + host + ";" +
            "Username=" + username + ";" +
            "Password=" + password + ";" +
            "Database=" + database;
        

        try
        {
            dbconn = new NpgsqlConnection(connectionString);
            dbconn.Open();
            Debug.Log("Succesfully connected to the database");

            if (cameraAnimator != null)
                cameraAnimator.enabled = true;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);

            ModalWindowMaker.Instance().DBConnectionError(host, username, password, database);
        }
    }

    public void CloseConnection()
    {
        if (dbconn != null)
        {
            dbconn.Close();
            dbconn = null;
        }
    }
}
