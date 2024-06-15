using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TMPro;

[System.Serializable]
public class Question
{
    public string questionText;
    public List<string> answers;
    public List<int> correctAnswerIndexes;
}
public class multi : MonoBehaviour
{
    public List<Question> questions;
    public Text questionText;
    public Toggle[] answerToggles;
    public Text resultText;
    public Button confirmButton;
    public TextMeshPro tMesh;

    private Question currentQuestion;
    private int currentQuestionIndex;
    private float questionStartTime;
    private List<float> questionTimes;
    private List<int> questionResults;
    public string filepath_3D;
    public NodeShow main;

    void Start()
    {
        
        questionTimes = new List<float>();
        questionResults = new List<int>();
        tMesh = GameObject.Find("Question").GetComponent<TextMeshPro>();
        confirmButton.onClick.AddListener(ConfirmAnswer);
        NextQuestion();
    }

    void NextQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            if(currentQuestionIndex == 1){
                main.rightHandReset = 0;
                main.allReset = 0;
                main.leftHandReset = 0;
                main.leftGraphReset = 0;
            }
            int temp = 1;
            currentQuestion = questions[currentQuestionIndex];
            questionText.text = currentQuestion.questionText;
            tMesh.text = currentQuestion.questionText + "\n\n";
            for (int i = 0; i < answerToggles.Length; i++)
            {
                answerToggles[i].isOn = true;

                if (i < currentQuestion.answers.Count)
                {

                    answerToggles[i].gameObject.SetActive(true);
                    answerToggles[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
                    tMesh.text += currentQuestion.answers[i];


                    if(temp % 4 == 0){
                        tMesh.text += "\n";
                    }
                    else{
                        tMesh.text += "\t\t";
                    }

                    temp++;
                }
                else
                {
                    answerToggles[i].gameObject.SetActive(false);
                }
            }
            questionStartTime = Time.time;
        }
        else
        {
            questionText.text = "Task Completed";
            // Optionally, disable the confirm button so it can't be clicked again
            confirmButton.gameObject.SetActive(false);
            foreach (var toggle in answerToggles)
            {
                toggle.gameObject.SetActive(false);
            }
            EndQuiz();
        }
    }

    public void ConfirmAnswer()
    {
        float questionEndTime = Time.time;
        float answerTime = questionEndTime - questionStartTime;
        questionTimes.Add(answerTime);

        List<int> chosenAnswerIndexes = new List<int>();
        for (int i = 0; i < answerToggles.Length; i++)
        {
            if (answerToggles[i].isOn)
            {
                // chosenAnswerIndexes.Add(100);
                chosenAnswerIndexes.Add(int.Parse(answerToggles[i].name));
                Debug.Log(int.Parse(answerToggles[i].name));
            }
        }
        if (!File.Exists(filepath_3D))
        {
            // 使用 'using' 确保文件流被正确关闭和销毁
            using (var fs = File.Create(filepath_3D)) { }
        }
        using (StreamWriter sw = new StreamWriter(filepath_3D, true))
        {
            foreach (int answerIndex in chosenAnswerIndexes)
            {
                //第一道问题前加入换行符
                if(currentQuestionIndex == 0)
                {
                    sw.WriteLine();
                }
                string contentToWrite = $"Question {currentQuestionIndex}: " + answerIndex + $" Time: {questionTimes[currentQuestionIndex]} seconds";
                sw.WriteLine(contentToWrite);
            }

        }
        chosenAnswerIndexes.Clear();
        currentQuestionIndex++;
        NextQuestion();
    }

    void EndQuiz()
    {
        //if (!File.Exists(filepath))
        //{
        //    // 使用 'using' 确保文件流被正确关闭和销毁
        //    using (var fs = File.Create(filepath)) { }
        //}

        //using (StreamWriter sw = new StreamWriter(filepath))
        //{
        //    for (int i = 0; i < questionResults.Count; i++)
        //    {

        //        string contentToWrite = $"Question {i + 1}: " + (questionResults[i] ? "Correct" : "Wrong") + $" Time: {questionTimes[i]} seconds";
        //        sw.WriteLine(contentToWrite);

        //        if(i == questionResults.Count - 1){
        //            sw.WriteLine("rightHandReset:" + main.rightHandReset + "\t" + "allReset:" + main.allReset + "\t" + "leftHandReset:" + main.leftHandReset + "\t" + "leftGraphReset:" + main.leftGraphReset);
        //        }
        //    }
            
        //}

        // if (!File.Exists(filepath))
        // {
        //     File.Create(filepath);
        // }
        // for (int i = 0; i < questionResults.Count; i++)
        // {
        //     string contentToWrite = $"Question {i + 1}: " + (questionResults[i] ? "Correct" : "Wrong") + $" Time: {questionTimes[i]} seconds";
        //     using (StreamWriter sw = new StreamWriter("filepath"))
        //     {
        //         sw.WriteLine(contentToWrite);
        //     }

            
        //     // File.WriteAllText(filepath, contentToWrite);
        //     // Debug.Log($"Question {i + 1}: " + (questionResults[i] ? "Correct" : "Wrong") + $" Time: {questionTimes[i]} seconds");
           
        // }
        // Optionally, display the results on the UI here.
    }

}

