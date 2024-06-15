using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class quizmanager : MonoBehaviour
{
    public List<QandA> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public Text QuestionTxt;
    
    public int A = 0;
    public float time=0;
    public bool timerun;
   
   

    private void Start()
    {

            QuestionTxt.text = QnA[0].Question;
            setanswer();
           timerun = true;
            
            



    }
    public void correct()
    {
        // QnA.RemoveAt(currentQuestion);
        

        generateQuestion();
        A++;
        

    }
    void setanswer()
    {
        for(int i = 0; i<options.Length;i++)
        {
            options[i].GetComponent<answers>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];
            
            if(QnA[currentQuestion].CorrectAnswer==i+1)
            {
                options[i].GetComponent<answers>().isCorrect = true;
            }
        }
    }

    
    void generateQuestion()
    {
     
        currentQuestion = A;
        QuestionTxt.text = QnA[currentQuestion].Question;
        setanswer();
        timerun = true;



    }
    void Update()
    {
        if(timerun==true)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
    }

}
