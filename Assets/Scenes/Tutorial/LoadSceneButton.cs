using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string SceneName = "";

   

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
