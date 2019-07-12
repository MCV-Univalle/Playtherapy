using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceController {

    public GameObject PerformanceDAO;
    public GameObject Performance;


    public int addPerformance(int angle, string movement_id)
    {

        Performance performance = new Performance();
        PerformanceDAO performanceDao = new PerformanceDAO();
        performance.Angle = angle;
        performance.Movement_id = movement_id;

        int result = performanceDao.InsertToSPerformance(performance);

        if (result == -1)
        {
            return result;
        }
        else
        {
            Debug.Log("Se ha presentado un error con la base de datos");
        }

        return 0;
    }
}
