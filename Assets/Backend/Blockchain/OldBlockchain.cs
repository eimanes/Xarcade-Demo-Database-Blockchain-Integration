using System;
using System.Net.Http;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using Proyecto26;
using UnityEngine.UI;
using Backend.Database;

public class OldBlockchain : MonoBehaviour
{
    public NewInternalDB idb;
    public NewDBHandler dbh;
    public static OldBlockchain Instance { get; private set; }

    public string bearer = "xar-K1yKcEICAWuh10ZylQOs-bearer-3AQ3fOPMueqBlJS7f7UH-admin-P1ESJbighL6htWzLvPwX";
    public string baseUrl = "https://xar-autosigner.proximaxtest.com";
    public string auth;
    public string gameId;

    public string token_id;
    public string username;

    public HttpClient client = new HttpClient();
    public string deeplinkURL;

    User user = new User();
    public string db_url;
    private string db_key = ".json?auth=IyehURIqxdvTlwZkafQs4gemkTHWUaXsVVdQ8Qvt";

    public Button ClaimButton;
    public TMP_Text userText;


    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        Time.timeScale = 1;
        CallAuth();
        DontDestroyOnLoad(this.gameObject);
        client = new HttpClient();
     
    }

    public class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    public void CallAuth()
    {
#if UNITY_WEBGL
        GetAuthFromWebGL();
#endif

#if UNITY_EDITOR
        StartCoroutine(GetUser(() =>
        {
            GetDBToken();
        }));
#endif
    }

    public void GetAuthFromWebGL()
    {
        int pm = Application.absoluteURL.IndexOf("?");
        if (pm != -1)
        {
            auth = Application.absoluteURL.Split("?"[0])[1].Split("=")[1];
            Debug.Log("new user: " + auth);
            PlayerPrefs.SetString("Auth", auth);
        }
    }

    public void GetDBToken()
    {

    }

    public IEnumerator GetUser(Action callback)
    {
        var cert = new ForceAcceptAll();
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/api/v1/users/{auth}");
        request.SetRequestHeader("Authorization", "Bearer " + bearer);
        request.certificateHandler = cert;

        // Send
        cert?.Dispose();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Login Error :(");
            // onErrorCallback(request.result);
            Debug.LogError(request.error, this);
        }
        else
        {
            var jsonData = JSON.Parse(request.downloadHandler.text);
            this.username = jsonData["username"];
            PlayerPrefs.SetString("username", username);
            Debug.Log("User retrieved. Username: " + username);
            userText.text = username;
            callback.Invoke();
        }
    }

    public static string nama()
    {
        string uname = PlayerPrefs.GetString("username");
        return uname;
    }

    private void onDeepLinkActivated(string url)
    {
        //bearer = PlayerPrefs.GetString("Bearer");
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;
        // Decode the URL to determine action. 
        // In this example, the app expects a link formatted like this:
        // unitydl://mylink?scene1
        this.auth = url.Split("?"[0])[1].Split("=")[1];
        //authText.text = this.auth;
        StartCoroutine(GetUser(() =>
        {
            GetDBToken();
        }));

        Debug.Log("Opened from deeplink!");
    }

    public void VoidClaimIGT()
    {
        Debug.Log("Clicked claim button");
        StartCoroutine(ClaimIGT());
    }



    public IEnumerator ClaimIGT()
    {
        var amount = Database.TotalScore();
        var id_igt = PlayerPrefs.GetString("token_id");
        var cert = new ForceAcceptAll();

        //var amount = PlayerPrefs.GetInt("IGT");
        Debug.Log("Claiming " + amount + " " + id_igt);

        UnityWebRequest request = new UnityWebRequest($"{baseUrl}/api/v1/transactions/distribute?TokenId={id_igt}&Amount={amount}&Auth={auth}", "POST");
        request.SetRequestHeader("Authorization", "Bearer " + bearer);

        request.certificateHandler = cert;

        // Send
        cert?.Dispose();

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error :(");
            // onErrorCallback(request.result);
            Debug.LogError(request.error, this);
            
        }
        else
        {
            GetDBToken();
            Debug.Log("Claimed Token. Please check Xarcade.");
            ClaimingToken();

        }

    }

    void ClaimingToken()
    {
        int totalScore = PlayerPrefs.GetInt("TotalScore");
        int tokensClaimed = PlayerPrefs.GetInt("TokensClaimed");

        int finalAmountTokens = totalScore + tokensClaimed;
        PlayerPrefs.SetInt("TokensClaimed", finalAmountTokens);
        PlayerPrefs.SetInt("TotalScore", 0);
        Debug.Log("Processing in db ...");

        idb.TotalScoreDbText.text = PlayerPrefs.GetInt("TotalScore").ToString();
        string time = TimeOfEvents();
        PlayerPrefs.SetString("Time_TokensClaimedUpdated", time);
        /*PostToDb();*/
        dbh.PostTokensReq();

    }

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

    void PostToDb()
    {
        int a = PlayerPrefs.GetInt("TokensClaimed");
        User user = new User();
        name = PlayerPrefs.GetString("username");
        RestClient.Put(db_url + name + db_key, user);

        Debug.Log("Total tokens claimed in db = " + a);
        Debug.Log("Total score in database resets to 0");
    }

}


