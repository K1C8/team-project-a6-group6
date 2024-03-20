using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
   public void MultiModeButton()
    {
        SceneManager.LoadScene("MultiMode");
    }

    public void SingleModeButton ()
    {
        SceneManager.LoadScene("SingleMode");
    }
}
