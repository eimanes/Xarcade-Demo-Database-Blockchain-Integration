using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.Networking;
using TMPro;

public class Database : MonoBehaviour
{
    public GameObject claimButton;
    public TMP_Text TotalScoreDbText;
    public TMP_Text GetDataText;

    [SerializeField] public string db_url;
    private string db_key = ".json?auth=IyehURIqxdvTlwZkafQs4gemkTHWUaXsVVdQ8Qvt";

    User user = new User();

    public static string nama()
    {
        string n = PlayerPrefs.GetString("username");
        return n;
    }

    public string TimeOfEvents()
    {
        int hour = System.DateTime.Now.Hour;
        int minutes = System.DateTime.Now.Minute;
        int seconds = System.DateTime.Now.Second;

        string toe = "" + hour + ":" + minutes + ":" + seconds;
        return toe;
    }

    public static string LastLogin()
    {
        string login = PlayerPrefs.GetString("LoginTime");
        return login;
    }
    public static string LastTotalScoreUpdated()
    {
        string t_tscu = PlayerPrefs.GetString("Time_TotalScoreUpdated");
        return t_tscu;
    }

    public static string LastTokensClaimedUpdated()
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

    public static int TokensClaimed()
    {
        int tc = PlayerPrefs.GetInt("TokensClaimed");
        return tc;
    }

    public void PostToDb()
    {
        User user = new User();
        name = PlayerPrefs.GetString("username");
        RestClient.Post(db_url + db_key, user);

    }



    public void Login()
    {
        string time = TimeOfEvents();
        PlayerPrefs.SetString("LoginTime", time);
        GetAssistingUser();
    }

    private void GetAssistingUser()
    {
        name = PlayerPrefs.GetString("username");
        RestClient.Get<User>(db_url + db_key).Then(response =>
        {
            user = response;
            if (user.a_Username == name)
            {
                RestClient.Put(db_url + name + db_key, user);
            }
            else { PostToDb(); }
        });
    }

    public void GetFromDb()
    {
        name = PlayerPrefs.GetString("username");
        RestClient.Get<User>(db_url + name + db_key).Then(response =>
        {
            user = response;
            TotalScoreDbText.text = "" + user.e_TotalScore;
        });
    }

    //This function is for GetData Button in DemoClaimScene
    //Getting all data from firebase and display in GetDataPanel.
    public void GetAll()
    {
        name = PlayerPrefs.GetString("username");
        RestClient.Get<User>(db_url + name + db_key).Then(response =>
        {
            user = response;
            GetDataText.text = "Username: " + user.a_Username + "\n" +
                               "Token ID: " + user.b_TokenID + "\n" + 
                               "Login Time: " + user.c_LoginTime + "\n" +
                               "Current Score: " + user.d_CurrentScore + "\n" +
                               "Total Score: " + user.e_TotalScore + "\n" + 
                               "TS Updated: " + user.f_LastTotalScoreUpdated + "\n" +
                               "Tokens Claimed: " + user.g_TokensClaimed + "\n" + 
                               "TC Updated: " + user.h_LastTokensClaimedUpdated;
        });
    }

    public void Start()
    {
        Time.timeScale = 1;         //Developers should add this so, the button function works everytime.
        string username = PlayerPrefs.GetString("username");
        Debug.Log("user in db: " + username);
        Debug.Log("token id: " + token_id());
        Debug.Log("total score in db: " + TotalScore());
        

    }

    private void Awake()
    {
        PlayerPrefs.SetString("db_url", db_url);
        name = PlayerPrefs.GetString("username");
        RestClient.Get<User>(db_url + name + db_key).Then(response =>
        {
            user = response;

            if (user.a_Username == null)
            {
                PlayerPrefs.SetInt("TotalScore", 0);
                TotalScoreDbText.text = "" + 0;
                claimButton.SetActive(false);
            }

            else
            {
                PlayerPrefs.SetInt("TotalScore", user.e_TotalScore);
                TotalScoreDbText.text = "" + user.e_TotalScore;
                PlayerPrefs.SetInt("TokensClaimed", user.g_TokensClaimed);
                if (user.e_TotalScore == 0)
                {
                    claimButton.SetActive(false);
                }
                else { claimButton.SetActive(true); }
            }

        });

        Debug.Log("Total score = " + TotalScore());

    }

    private void Update()
    {
        name = PlayerPrefs.GetString("username");
        RestClient.Get<User>(db_url + name + db_key).Then(response =>
        {
            user = response;
            PlayerPrefs.SetInt("TotalScore", user.e_TotalScore);
            PlayerPrefs.SetInt("TokensClaimed", user.g_TokensClaimed);

            if (user.e_TotalScore == 0)
            {
                claimButton.SetActive(false);
            }
            else { claimButton.SetActive(true); }

            GetDataText.text = "Username: " + user.a_Username + "\n" +
                               "Token ID: " + user.b_TokenID + "\n" +
                               "Login Time: " + user.c_LoginTime + "\n" +
                               "Current Score: " + user.d_CurrentScore + "\n" +
                               "Total Score: " + user.e_TotalScore + "\n" +
                               "TS Updated: " + user.f_LastTotalScoreUpdated + "\n" +
                               "Tokens Claimed: " + user.g_TokensClaimed + "\n" +
                               "TC Updated: " + user.h_LastTokensClaimedUpdated;
        });
    }

    


}
