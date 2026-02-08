using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMode : MonoBehaviour
{
    public Button ToMainButton;
    public Button NomalModeButton;
    public Button HardModeButton;

    public Image ModeLockedImage;
    public Image PlayerLockedImage;

    public Button BluePlayerButton;
    public Button RedPlayerButton;
    void Start()
    { 
        if(PlayerStatsManager.Instance != null)
        {
            Destroy(PlayerStatsManager.Instance.gameObject);
        }
        ToMainButton.onClick.AddListener(OnClickToMainButton);
        NomalModeButton.onClick.AddListener(OnClickNomalModeButton); 
        BluePlayerButton.onClick.AddListener (OnClickBluePlayerButton);
        Check();
    }
    void Check()
    {
        if(PlayerPrefs.GetInt("Clear") == 1)
        {
            ModeLockedImage.gameObject.SetActive(false);
            HardModeButton.onClick.AddListener(OnClickHardModeButton);
        }
        if (PlayerPrefs.GetFloat("HardBestScore") >= 1000)
        {
            PlayerLockedImage.gameObject.SetActive(false);
            RedPlayerButton.onClick.AddListener(OnClickRedPlayerButton);
        }
        
    }
    void OnClickNomalModeButton()
    {
        GameManager.Instance.ModeSelect = 0;
        NomalModeButton.gameObject.SetActive(false);
        HardModeButton.gameObject.SetActive(false);
        BluePlayerButton.gameObject.SetActive(true);
        RedPlayerButton.gameObject.SetActive(true);
    }
    void OnClickHardModeButton()
    {
        GameManager.Instance.ModeSelect = 1;
        NomalModeButton.gameObject.SetActive(false);
        HardModeButton.gameObject.SetActive(false);
        BluePlayerButton.gameObject.SetActive(true);
        RedPlayerButton.gameObject.SetActive(true);
    }
    void OnClickBluePlayerButton()
    {
        GameManager.Instance.PlayerSelect = 0;
        SceneManager.LoadScene("Game");
    }
    void OnClickRedPlayerButton()
    {
        GameManager.Instance.PlayerSelect = 1;
        SceneManager.LoadScene("Game");
    }
    void OnClickToMainButton()
    {
        SceneManager.LoadScene("Main");
    }
}
