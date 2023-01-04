using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDetails : MonoBehaviour
{
    public TMP_Text bearerText;
    public TMP_Text userText;
    public TMP_Text tokenText;


    private void Awake()
    {
        string bearer = PlayerPrefs.GetString("bearer");
        string tokenId = PlayerPrefs.GetString("token_id");
        string username = PlayerPrefs.GetString("username");

        bearerText.text = "Bearer: " + bearer;
        userText.text = "Username: " + username;
        tokenText.text = "TokenID: " + tokenId;
    }
}
