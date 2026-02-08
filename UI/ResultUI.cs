using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public GameObject MainUI;
    public TextMeshProUGUI CurrentMeterText;
    public TextMeshProUGUI TopMeterText;

    private void OnEnable()
    {
        if (GameManager.Instance.ModeSelect == 0)
        {
            TopMeterText.text = $"{Mathf.Floor(PlayerPrefs.GetFloat("NomalBestScore"))} M";
            CurrentMeterText.text = $"{Mathf.Floor(GameManager.Instance.NomalScore)} M";
        }
        else
        {
            TopMeterText.text = $"{GameManager.Instance.HardBestScore} M";
            CurrentMeterText.text = $"{Mathf.Floor(GameManager.Instance.HardScore)} M";
        }
    }
}
