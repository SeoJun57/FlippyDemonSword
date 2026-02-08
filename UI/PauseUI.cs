using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject ESCPanel;
    public Button MuteButton;
    public Sprite UnMuteImage;
    public Sprite MuteImage;

    public void OnClickMuteButton()
    {
        AudioListener.volume = AudioListener.volume == 1 ? 0 : 1;
        MuteButton.image.sprite = AudioListener.volume == 1 ? UnMuteImage : MuteImage;
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Game");
        Destroy(PlayerStatsManager.Instance.gameObject); 
    }
    public void UnPause()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        ESCPanel.SetActive(!ESCPanel.activeSelf);
    }
    public void ToSelectButton()
    {
        SceneManager.LoadScene("Select");
    }
}
