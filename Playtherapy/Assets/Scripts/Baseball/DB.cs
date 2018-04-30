using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine;

public class DB : MonoBehaviour {

    private string connection;
    private IDbConnection dbcon  ;
    private IDbCommand dbcmd  ;
    private IDataReader reader;
 
    

    // Use this for initialization
    void Start () {

        //OpenDB("D:/Users/Boku Cortes/Downloads/playtherapy-webadmin/workspace/db.sqlite3");
        //OnlineDB();

    }

    void OnlineDB() {

        //string connectionString ="URI=file:https://playtherapy-webadmin-edwingamboa.c9users.io/db.sqlite3;";
        //string connectionString = "URI=file:https://playtherapy-webadmin-edwingamboa.c9users.io/db.sqlite3";

        string connectionString = "URI=file:https://playtherapy-webadmin-edwingamboa.c9users.io/db.sqlite3;Version=3";



        //  \\serverName\shareName\folder\myDatabase.mdb; User Id = admin;
        // Password =;



        dbcon = new SqliteConnection(connectionString);
        dbcon.Open();
        print(dbcon.State);
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = "SELECT 'numero_documento' FROM Auth_User ;";
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            print(reader.GetValue(0));
        }

    }
    void OpenDB(string p)
    {
        connection = "URI=file:" + p; // we set the connection to our database
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
        dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = "SELECT * FROM Auth_User WHERE username = 'admin';";
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
           print(reader.GetValue(0));
            print(reader.GetValue(1));
            print(reader.GetValue(2));
            print(reader.GetValue(3));
            print(reader.GetValue(4));
            print(reader.GetValue(5));
            print(reader.GetValue(6));
            print(reader.GetValue(7));
            print(reader.GetValue(8));
            print(reader.GetValue(9));
            print(reader.GetValue(10));
        }

        print(dbcon.State);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
