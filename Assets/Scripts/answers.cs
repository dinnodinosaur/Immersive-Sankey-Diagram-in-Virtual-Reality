using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class answers : MonoBehaviour
{
    public bool isCorrect = false;
    public quizmanager QM;
    

    public void Answer()
    {
        if(isCorrect)
        {
            Debug.Log(QM.currentQuestion+"correct"+QM.time);
            
            QM.correct();
            QM.time = 0;
        }
        else
        {
            Debug.Log(QM.currentQuestion+"wrong" + QM.time);
            
            QM.correct();
            QM.time = 0;
        }
    }



}
