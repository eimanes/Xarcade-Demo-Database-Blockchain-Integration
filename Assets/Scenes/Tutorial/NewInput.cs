using System;
using System.Net.Http;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using Proyecto26;
using UnityEngine.UI;

public class NewInput : MonoBehaviour
{
    public TMP_InputField newBearer;
    public TMP_InputField newUsername;
    public TMP_InputField newTokenId;

    public TMP_Text bearerText;
    public TMP_Text userText;
    public TMP_Text tokenText;

    public string auth;
    public string username;
    public string bearer = "xar-K1yKcEICAWuh10ZylQOs-bearer-3AQ3fOPMueqBlJS7f7UH-admin-P1ESJbighL6htWzLvPwX";
    public string tokenId = "BDT";
    public string baseUrl = "https://xar-autosigner.proximaxtest.com";

    private void Awake()
    {
        bearer = PlayerPrefs.GetString("bearer");
        tokenId = PlayerPrefs.GetString("token_id");
        username = PlayerPrefs.GetString("username");

        bearerText.text = bearer;
        userText.text = username;
        tokenText.text = tokenId;

    }

    public void Start()
    {
        PlayerPrefs.SetString("bearer", bearer);
        PlayerPrefs.SetString("token_id", tokenId);
        PlayerPrefs.SetString("username", username);
    }

    public void NewBearer()
    {
        PlayerPrefs.SetString("bearer", newBearer.text);
        bearer = PlayerPrefs.GetString("bearer");
        bearerText.text = bearer;
        
    }

    public void NewUser()
    {
        PlayerPrefs.SetString("username", newUsername.text);
        username = PlayerPrefs.GetString("username");
        userText.text = username;
    }

    public void NewToken()
    {
        PlayerPrefs.SetString("token_id", newTokenId.text);
        tokenId = PlayerPrefs.GetString("token_id");
        tokenText.text = tokenId;
    }


    public void DefaultInput()
    {
        CallAuth();
        PlayerPrefs.SetString("bearer", "xar-K1yKcEICAWuh10ZylQOs-bearer-3AQ3fOPMueqBlJS7f7UH-admin-P1ESJbighL6htWzLvPwX");
        PlayerPrefs.SetString("token_id", "BDT");

        bearer = PlayerPrefs.GetString("bearer");
        username = PlayerPrefs.GetString("username");
        tokenId = PlayerPrefs.GetString("token_id");

        bearerText.text = bearer;
        userText.text = username;
        tokenText.text = tokenId;
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


}
