using Backend.FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using UnityEngine;
using Proyecto26;
using System;

namespace Backend.Database
{
    public class User
    {
        public int d_TotalScore;
        public int f_TokensReq;
        public int g_TokensClaimed;

        public User()
        {
            d_TotalScore = NewInternalDB.keepTotalScore();
            f_TokensReq = NewInternalDB.keepTokensRequested();
            g_TokensClaimed = NewInternalDB.keepTokensClaimed();
        }
    }

    public class NewDBHandler : MonoBehaviour
    {
        public NewInternalDB idb;
        public string db_url = "https://bot-game-a4374-default-rtdb.asia-southeast1.firebasedatabase.app/";
        private string db_key = ".json?auth=0tFpVOVjeJrTmthkUpZFbSc7l89KRGE4GyGiDbgx";
        private string secretKey = "cOaRJDdfgWqdAECJppWCza5EOIeElFtivJRXD6UR";
        //Get

        public TMP_Text GetDataText;

        User user = new User();

        

        public void GetJSON() =>
            FirebaseDatabase.GetJSON(idb.nama(), gameObject.name, "DisplayData", "DisplayErrorObject");

        public void DisplayData(string data)
        {
            GetDataText.text = data;
            Debug.Log(data);
        }

        public void GetTotalScore()
        {
            try
            {
                RestClient.Get<User>(db_url + idb.nama() + "/b_Score/" + db_key).Then(response =>
                {
                    user = response;
                    PlayerPrefs.SetInt("TotalScore", user.d_TotalScore);
                    idb.TotalTokenText.text = "" + user.d_TotalScore;

                    if (user.d_TotalScore == 0) { idb.claimButton.SetActive(false); }
                    else { idb.claimButton.SetActive(true); } //hide claim button if no total score

                });
            } catch (FormatException e)
            {
                Debug.LogError("Cannot GET Data! Refresh or reboot now!");
            }
        }

        public void GetTokens()
        {
            try
            {
                RestClient.Get<User>(db_url + idb.nama() + "/c_Token/" + db_key).Then(response =>
                {
                    user = response;
                    PlayerPrefs.SetInt("TokensReqested", user.f_TokensReq);
                    PlayerPrefs.SetInt("TokensClaimed", user.g_TokensClaimed);
                    if (user.g_TokensClaimed >= 1000000)
                    {
                        idb.claimButton.SetActive(false);
                    }
                    else { idb.claimButton.SetActive(true); } //avoid player claims more tha 10k
                });
            } catch (FormatException e)
            {
                Debug.LogError("Cannot GET Data! Refresh or reboot now!");
            }
        }

        /*public void GetTotalScore() =>
            FirebaseDatabase.GetTotalScore(idb.nama(), gameObject.name, "KeepDataTotalScore", "DisplayErrorObject");*/


        public void KeepDataTotalScore(string value4)
        {
            int ts = int.Parse(value4);
            idb.TotalScoreDbText.text = value4;
            PlayerPrefs.SetInt("TotalScore", ts);
            
            idb.TotalTokenText.text = "" + ts;

            if (ts == 0) { idb.claimButton.SetActive(false); }
            else { idb.claimButton.SetActive(true); } //hide claim button if no total score
        }

        /*public void GetTokens() =>
            FirebaseDatabase.GetTokens(idb.nama(), gameObject.name, "KeepDataTokens", "DisplayErrorObject");*/

        public void KeepDataTokens(string value6, string value7)
        {
            int tokensReq = int.Parse(value6);
            int tokensClaim = int.Parse(value7);

            PlayerPrefs.SetInt("TokensRequested", tokensReq);
            PlayerPrefs.SetInt("TokensClaimed", tokensClaim);

            if (tokensClaim >= 1000000)
            {
                idb.claimButton.SetActive(false);
            }
            else { idb.claimButton.SetActive(true); } //avoid player claims more tha 10k

        }

        //Post

        public void PostLogin()
        {
            FirebaseDatabase.PostLogin(idb.nama(), idb.token_id(), idb.LastLogin(), gameObject.name,
            "DisplayInfo", "DisplayErrorObject");
        }

        public void PostCurrentScore()
        {
            FirebaseDatabase.PostCurrentScore(idb.nama(), idb.CurrentScore(), /*gameObject.name,*/
            "DisplayInfo", "DisplayErrorObject");
        }

        public void PostTotalScore()
        {
            FirebaseDatabase.PostTotalScore(idb.nama(), idb.TotalScore(), idb.TotalScoreUpdated(), /*gameObject.name,*/
            "DisplayInfo", "DisplayErrorObject");
        }

        public void PostTokensReq()
        {
            FirebaseDatabase.PostTokensReq(idb.nama(), idb.TokensRequested(), gameObject.name,
            "DisplayInfo", "DisplayErrorObject");
        }

        public void PostTokensClaim()
        {
            FirebaseDatabase.PostTokensClaim(idb.nama(), idb.TokensClaimed(), idb.TokensClaimedUpdated(), "",
            "DisplayInfo", "DisplayErrorObject");
        }
    }
}

