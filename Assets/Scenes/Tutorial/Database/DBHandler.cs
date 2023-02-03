/*using Backend.FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;
using UnityEngine;

namespace Scenes.Tutorial.Database
{
    public class DBHandler : MonoBehaviour
    {
        public TMP_Text GetDataText;
        
        public void GetJSON() =>
            FirebaseDatabase.GetJSON(InternalDB.nama(), gameObject.name, "DisplayData", "DisplayErrorObject");

        public void GetTotalScore() =>
            FirebaseDatabase.GetJSON(InternalDB.nama(), gameObject.name, "KeepDataTotalScore", "DisplayErrorObject");

        public void KeepDataTotalScore(string data)
        {
            int ts = int.Parse(data);
            PlayerPrefs.SetInt("TotalScore", ts);
        }

        public void DisplayData(string data)
        {
            GetDataText.text = data;
            Debug.Log(data);
        }

        public void PostLogin()
        {
            FirebaseDatabase.PostLogin(InternalDB.nama(), InternalDB.token_id(), InternalDB.LastLogin(), gameObject.name,
            "DisplayInfo", "DisplayErrorObject");
        }

        public static void PostCurrentScore()
        {
            FirebaseDatabase.PostCurrentScore(InternalDB.nama(), InternalDB.CurrentScore(), *//*gameObject.name,*//*
            "DisplayInfo", "DisplayErrorObject");
        }

        public static void PostTotalScore()
        {
            FirebaseDatabase.PostTotalScore(InternalDB.nama(), InternalDB.TotalScore(), InternalDB.TotalScoreUpdated(), *//*gameObject.name,*//*
            "DisplayInfo", "DisplayErrorObject");
        }

        public void PostTokensReq()
        {
            FirebaseDatabase.PostTokensReq(InternalDB.nama(), InternalDB.TokensRequested(), gameObject.name,
            "DisplayInfo", "DisplayErrorObject");
        }

        public static void PostTokensClaim()
        {
            FirebaseDatabase.PostTokensClaim(InternalDB.nama(), InternalDB.TokensClaimed(), InternalDB.TokensClaimedUpdated(), "",
            "DisplayInfo", "DisplayErrorObject");
        }
    }
}

*/