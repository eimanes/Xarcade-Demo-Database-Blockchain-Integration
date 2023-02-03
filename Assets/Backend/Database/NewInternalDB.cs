using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.Networking;
using TMPro;
using Backend.Database;

namespace Backend.Database
{
    

    public class NewInternalDB : MonoBehaviour
    {
        public NewDBHandler dbh;
        public GameObject claimButton;
        public TMP_Text usernameText;
        public TMP_Text TotalScoreDbText;
        public TMP_Text TotalTokenText;

        

        public string TimeOfEvents()
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

        public string nama()
        {
            string n = PlayerPrefs.GetString("username");
            return n;
        }

        public string token_id()
        {
            string token = PlayerPrefs.GetString("token_id");
            return token;
        }

        public string LastLogin()
        {
            string login = PlayerPrefs.GetString("LoginTime");
            return login;
        }

        public int CurrentScore()
        {
            int cs = PlayerPrefs.GetInt("CurrentScore");
            return cs;
        }

        public int TotalScore()
        {
            int ts = PlayerPrefs.GetInt("TotalScore");
            return ts;
        }

        public static int keepTotalScore()
        {
            int ts = PlayerPrefs.GetInt("TotalScore");
            return ts;
        }

        public string TotalScoreUpdated()
        {
            string t_tscu = PlayerPrefs.GetString("Time_TotalScoreUpdated");
            return t_tscu;
        }

        public int TokensRequested()
        {
            int tr = PlayerPrefs.GetInt("TokensReqested");
            return tr;
        }

        public static int keepTokensRequested()
        {
            int tr = PlayerPrefs.GetInt("TokensReqested");
            return tr;
        }

        public int TokensClaimed()
        {
            int tc = PlayerPrefs.GetInt("TokensClaimed");
            return tc;
        }

        public static int keepTokensClaimed()
        {
            int tc = PlayerPrefs.GetInt("TokensClaimed");
            return tc;
        }

        public string TokensClaimedUpdated()
        {
            string t_tcu = PlayerPrefs.GetString("Time_TokensClaimedUpdated");
            return t_tcu;
        }

        /*private void Awake()
        {

        }*/

        public void Start()
        {
            Time.timeScale = 1;
            string username = nama();
            usernameText.text = "" + username;
            Debug.Log("user in db: " + username);
            Debug.Log("token id: " + token_id());
            Debug.Log("total score in db: " + TotalScore());

            dbh.GetTotalScore();
            dbh.GetTokens();
            usernameText.text = nama();
            StartCoroutine(delayLogin());
        }

        public void Update()
        {
            /*StartCoroutine(delayUpdate());*/
            /*TotalTokenText.text = "" + TotalScore() + " " + token_id();*/
        }

        public void Login()
        {
            string time = TimeOfEvents();
            PlayerPrefs.SetString("LoginTime", time);
            dbh.PostLogin();
        }

        IEnumerator delayUpdate()
        {
            yield return new WaitForSeconds(10);
            dbh.GetTotalScore();
            dbh.GetTokens();
        }

        IEnumerator delayLogin()
        {
            yield return new WaitForSeconds(2);
            dbh.PostLogin();
        }


    }





























}
