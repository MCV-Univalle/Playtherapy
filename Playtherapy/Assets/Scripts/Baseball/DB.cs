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
    private string route = "D:/Users/Boku Cortes/Downloads/playtherapy-webadmin/workspace/db.sqlite3";



    // Use this for initialization
    void Start () {

        OpenDB("D:/Users/Boku Cortes/Downloads/playtherapy-webadmin/workspace/db.sqlite3");
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
       /* dbcmd = dbcon.CreateCommand();
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
        */
        
    }

    void InsertInGameSession(System.DateTime date, int score, int repetitions,int time, int level,string therapy,string minigame,string movements,string performance) {


        connection = "URI=file:" + route; // we set the connection to our database
        dbcon = new SqliteConnection(connection);
        dbcon.Open();

        dbcmd = dbcon.CreateCommand();

        dbcmd.CommandText = "INSERT INTO GameSession (date, score, repetitions,time,level,therapy,minigame,movements,gameperformance)" +
            " VALUES (" + date+","+ score + "," + repetitions + "," + time + "," + level + ",'" + therapy + "','" + minigame + "','" + movements + "','" + performance +"');";

        dbcmd.ExecuteNonQuery();


    }




}
