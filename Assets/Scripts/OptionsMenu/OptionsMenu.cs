using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        //Debug.Log(volume);
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void Back() 
    {
        SceneManager.LoadScene("MainMenu");    
        
    }
}
