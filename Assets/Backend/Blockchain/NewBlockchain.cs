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

public class NewBlockchain : MonoBehaviour
{
    public NewDBHandler dbh;
    public NewInternalDB idb;
    public static NewBlockchain Instance { get; private set; }

    public string bearer = "xar-K1yKcEICAWuh10ZylQOs-bearer-3AQ3fOPMueqBlJS7f7UH-admin-P1ESJbighL6htWzLvPwX";
    public string baseUrl = "https://xar-autosigner.proximaxtest.com";
    public string auth;
    public string gameId;

    public string token_id;
    public string username;

    public HttpClient client = new HttpClient();
    public string deeplinkURL;

    public TMP_Text usernameText;

    /*Claim panel*/
    public GameObject panelClose;
    public GameObject panelOpen1;
    public GameObject panelOpen2;
    public GameObject YesButton;
    public TMP_Text amountTokenText;
    public GameObject notiText;


    User user = new User();

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
        PlayerPrefs.SetString("token_id", token_id);
        //Global.backend = this;
        DontDestroyOnLoad(this.gameObject);
        client = new HttpClient();
        // originalFontSize = tokentext.fontSize;
        Debug.Log("baseurl: " + baseUrl);
        Debug.Log("bearer: " + bearer);
        Debug.Log("auth: " + auth);

    }

    public class ForceAcceptAll : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    private void CallAuth()
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
            PlayerPrefs.SetString("username", "!!ERROR!!");
            usernameText.text = "!!ERROR!!";
        }
        else
        {
            var jsonData = JSON.Parse(request.downloadHandler.text);
            this.username = jsonData["username"];
            if (this.username == null || this.username == "")
            {
                PlayerPrefs.SetString("username", "!!ERROR!!");
                Debug.Log("User retrieved. Username: !!ERROR!!");
                usernameText.text = "!!ERROR!!";
            }
            else
            {
                PlayerPrefs.SetString("username", username);
                Debug.Log("User retrieved. Username: " + username);
            }
            
            usernameText.text = "" + username;
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
        YesButton.SetActive(false);
        StartCoroutine(ClaimIGT());
        StartCoroutine(ClosePanelDelayed());
        /*Debug.Log("Bearer: " + bearer);*/
    }




    public IEnumerator ClaimIGT()
    {
        var amount = PlayerPrefs.GetInt("TotalScore");
        var id_igt = PlayerPrefs.GetString("token_id");
        var cert = new ForceAcceptAll();

        //var amount = PlayerPrefs.GetInt("IGT");
        Debug.Log("Claiming Token " + id_igt);

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

    private void ClaimingToken()
    {
        int totalScore = PlayerPrefs.GetInt("TotalScore");
        int tokensClaimed = PlayerPrefs.GetInt("TokensClaimed");

        int finalAmountTokens = totalScore + tokensClaimed;
        PlayerPrefs.SetInt("TokensClaimed", finalAmountTokens);
        PlayerPrefs.SetInt("TotalScore", 0);
        Debug.Log("Processing in db ...");
        amountTokenText.text = PlayerPrefs.GetInt("TotalScore").ToString() + " " + idb.token_id();

        idb.TotalScoreDbText.text = PlayerPrefs.GetInt("TotalScore").ToString();
        string time = idb.TimeOfEvents();
        PlayerPrefs.SetString("Time_TokensClaimedUpdated", time);
        dbh.PostTokensClaim();

    }

    /*private void PostToDb()
    {
        string db_key = PlayerPrefs.GetString("db_key");
        string db_key_write = PlayerPrefs.GetString("db_key_write");
        string db_url = PlayerPrefs.GetString("db_url");
        int a = PlayerPrefs.GetInt("TokensClaimed");
        User user = new User();
        name = PlayerPrefs.GetString("username");
        if (name != null || name != "")
        {
            RestClient.Put(db_url + name + db_key_write, user);
        }

        Debug.Log("Total tokens claimed in db = " + a);
        Debug.Log("Total score in database resets to 0");
    }*/

    
    

    public IEnumerator ClosePanelDelayed()
    {
        yield return new WaitForSeconds(5f);    // Wait for 5 seconds
        panelClose.SetActive(false);
        panelOpen1.SetActive(true);
        panelOpen2.SetActive(true);
        YesButton.SetActive(true);
        notiText.SetActive(false);
    }
}
