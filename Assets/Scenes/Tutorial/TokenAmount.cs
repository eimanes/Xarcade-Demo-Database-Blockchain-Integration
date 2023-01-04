using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using TMPro;

public class TokenAmount : MonoBehaviour
{
    public TMP_Text currentScoreText;
    public TMP_Text totalScoreText;
    public int currentScore = 0;
    public int totalScore;
    public GameObject AddButton;
    [SerializeField] public string db_url = "https://fir-token-ccdbb-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public Database Database;

    //Update the current score, when '+1' button is clicked.
    //The current score will be saved in PlayerPrefs as internal db.
    //Current score also will be post the data in db.
    public void CurrentScore()
    {
        currentScore += 1;
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        currentScoreText.text = "" + currentScore;
        PostToDb();
    }

    //Looping is made to ensure that user cannot add current score more than 3.
    private void Update()
    {
        if (currentScore > 2)
        {
            AddButton.SetActive(false);
        }
        else { AddButton.SetActive(true);}
    }

    //When confirmed button is clicked, the total score will be saved
    //in both external and internal db.
    //Total score is the total of current score added.
    //Time will be updated and posted in db whenever the button is clicked.
    public void ConfirmButton()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore");
        totalScore = PlayerPrefs.GetInt("TotalScore");
        totalScore += currentScore;
        totalScoreText.text = "" + totalScore;
        PlayerPrefs.SetInt("CurrentScore", 0);
        PlayerPrefs.SetInt("TotalScore", totalScore);
        currentScore = PlayerPrefs.GetInt("CurrentScore");
        currentScoreText.text = "" + currentScore;
        string time = Database.TimeOfEvents();
        PlayerPrefs.SetString("Time_TotalScoreUpdated", time);
        PostToDb();
    }

    //The data will be posted based on the username
    void PostToDb()
    {
        User user = new User();
        name = PlayerPrefs.GetString("username");
        RestClient.Put(db_url + name + ".json", user);
    }

}
