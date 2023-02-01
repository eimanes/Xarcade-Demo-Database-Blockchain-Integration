using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.Networking;
using TMPro;
using Scenes.Tutorial.Database;
using FirebaseWebGL.Scripts.FirebaseBridge;

public class InternalDB : MonoBehaviour
{

    public int currentScore = 0;
    public int totalScore;
    public TMP_Text GetDataText;
    public TMP_Text totalScoreText;

    public DBHandler db;

    public void Awake()
    {
        db.GetTotalScore();
        totalScoreText.text = TotalScore().ToString();
    }

    public void Update()
    {
        db.GetJSON();
    }

    public void Login()
    {
        string time = TimeOfEvents();
        PlayerPrefs.SetString("LoginTime", time);
    }

    public void Claim()
    {
        int totalScore = PlayerPrefs.GetInt("TotalScore");
        int tokensClaimed = PlayerPrefs.GetInt("TokensClaimed");

        int finalAmountTokens = totalScore + tokensClaimed;
        PlayerPrefs.SetInt("TokensClaimed", finalAmountTokens);
        PlayerPrefs.SetInt("TotalScore", 0);
        Debug.Log("Processing in db ...");

        string time = TimeOfEvents();
        PlayerPrefs.SetString("Time_TokensClaimedUpdated", time);
        DBHandler.PostTokensClaim();
    }

    public static string nama()
    {
        string n = PlayerPrefs.GetString("username");
        return n;
    }

    public static string TimeOfEvents()
    {
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;

        int hour = System.DateTime.Now.Hour;
        int minutes = System.DateTime.Now.Minute;
        int seconds = System.DateTime.Now.Second;

        string toe = "" + day + "/" + month + "/" + year + " | " + hour + ":" + minutes + ":" + seconds;
        return toe;
    }

    public static string LastLogin()
    {
        string login = PlayerPrefs.GetString("LoginTime");
        return login;
    }
    public static string TotalScoreUpdated()
    {
        string t_tscu = PlayerPrefs.GetString("Time_TotalScoreUpdated");
        return t_tscu;
    }

    public static string TokensClaimedUpdated()
    {
        string t_tcu = PlayerPrefs.GetString("Time_TokensClaimedUpdated");
        return t_tcu;
    }


    public static string token_id()
    {
        string token = PlayerPrefs.GetString("token_id");
        return token;
    }

    public static int CurrentScore()
    {
        int cs = PlayerPrefs.GetInt("CurrentScore");
        return cs;
    }

    public static int TotalScore()
    {
        int ts = PlayerPrefs.GetInt("TotalScore");
        return ts;
    }

    public static int TokensRequested()
    {
        int tr = PlayerPrefs.GetInt("TokenReqested");
        return tr;
    }

    public static int TokensClaimed()
    {
        int tc = PlayerPrefs.GetInt("TokensClaimed");
        return tc;
    }








}
