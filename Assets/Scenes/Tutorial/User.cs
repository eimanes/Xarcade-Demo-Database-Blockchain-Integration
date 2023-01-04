using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


//Create a class that calls all the parameters to save in db
//Every parameters will be called from 'public static' datatype.
[Serializable]
public class User
{
    public string a_Username;
    public string b_TokenID;
    public string c_LoginTime;
    public int d_CurrentScore;
    public int e_TotalScore;
    public string f_LastTotalScoreUpdated;
    public int g_TokensClaimed;
    public string h_LastTokensClaimedUpdated;

    public User()
    {
        a_Username = Database.nama();
        b_TokenID = Database.token_id();
        c_LoginTime = Database.LastLogin();
        d_CurrentScore = Database.CurrentScore();
        e_TotalScore = Database.TotalScore();
        g_TokensClaimed = Database.TokensClaimed();
        f_LastTotalScoreUpdated = Database.LastTotalScoreUpdated();
        h_LastTokensClaimedUpdated = Database.LastTokensClaimedUpdated();
    }
}

