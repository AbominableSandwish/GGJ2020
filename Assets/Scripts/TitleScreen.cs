using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>()
            .PlayMusic(MotherFuckingAudioManager.MusicList.MAIN, true);
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>()
            .PlayMusic(MotherFuckingAudioManager.MusicList.MENU, true);
        SceneManager.LoadScene(0);

    }

    public void Credit()
    {
        SceneManager.LoadScene(3);
    }
}
