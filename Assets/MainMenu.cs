using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text title;
    public Slider volumeSlider;
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(volumeSlider.value);
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");

    }
}
