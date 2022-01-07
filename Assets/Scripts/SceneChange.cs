using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour
{
    //change the scences
    public void ChangeFirstStageScene()
    {
        SceneManager.LoadScene("Project3");
    }
    public void ChangeSecondStageScene()
    {
        SceneManager.LoadScene("Project3.1");
    }
    public void ChangeThridStageScene()
    {
        SceneManager.LoadScene("Project3.2");
    }
    public void Restart()
    {
        SceneManager.LoadScene("Opening");
    }
}
