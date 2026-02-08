using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button StartButton;
    void Start()
    {
        StartButton.onClick.AddListener(OnClickStartButton);
    }

    void OnClickStartButton()
    {
        SceneManager.LoadScene("Select");
    }
    // TODO : 치트키 나중에 삭제 해야함 --------------
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            PlayerPrefs.SetInt("Clear", 0);
            PlayerPrefs.SetFloat("HardBestScore", 0);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            PlayerPrefs.SetInt("Clear", 1);
            PlayerPrefs.SetFloat("HardBestScore", 1000);
        }
    }
}
