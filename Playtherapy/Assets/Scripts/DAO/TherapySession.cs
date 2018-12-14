using UnityEngine;
using System.Collections;
using System;

public class TherapySession
{
    //private string id;
    private string date;
    private string objective;
    private string description;
    private string therapist_id;
    private string patient_id;

    public TherapySession(string therapist_id, string patient_id)
    {
        date = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
        this.therapist_id = therapist_id;
        this.patient_id = patient_id;
    }

    public TherapySession()
    {

    }

    public string Date
    {
        get
        {
            return date;
        }

        set
        {
            date = value;
        }
    }

    public string Objective
    {
        get
        {
            return objective;
        }

        set
        {
            objective = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public string Therapist_id
    {
        get
        {
            return therapist_id;
        }

        set
        {
            therapist_id = value;
        }
    }

    public string Patient_id
    {
        get
        {
            return patient_id;
        }

        set
        {
            patient_id = value;
        }
    }
}
