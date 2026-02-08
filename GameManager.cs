using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region
    private static GameManager s_instance = null;
    public static GameManager Instance
    {
        get
        {
            if(s_instance == null) return null;
            return s_instance;
        }
    }
    private void Awake()
    {
        if(s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private int _playerSelect = 0;
    private int _modeSelect = 0;

    private float _nomalScore = 0;
    private float _hardScore = 0;

    private int _isClear = 0;
    public int IsClear
    {
        get { return _isClear; }
        set 
        {
            _isClear = value; 
            if( _isClear == 1)
            {
                PlayerPrefs.SetInt("Clear", 1);
            }
        }
    }

    // 노말모드 베스트 스코어
    public float NomalScore
    {   // 기록 표시를 위해 현재 스코어 기록 가져옴
        get { return _nomalScore; }
        set 
        {   // 스코어 기록세팅
            _nomalScore = value;
            // 스코어가 저장된 기록보다 높으면 베스트스코어로 변경
            if (_nomalScore > PlayerPrefs.GetFloat("NomalBestScore"))
            {
                PlayerPrefs.SetFloat("NomalBestScore", _nomalScore);
            }
        }
    }
    // 노말모드 베스트 스코어 가져오기
    public float NomalBestScore
    {
        get => PlayerPrefs.GetFloat("NomalBestScore"); 
    }
    // 하드모드 베스트 스코어 -----------------
    public float HardScore
    {   // 기록 표시를 위해 현재 스코어 기록 가져옴
        get { return _hardScore; }
        set
        {   // 스코어 기록세팅
            _hardScore = value;
            // 스코어가 저장된 기록보다 높으면 베스트스코어로 변경
            if (_hardScore > PlayerPrefs.GetFloat("HardBestScore"))
            {
                PlayerPrefs.SetFloat("HardBestScore", _hardScore);
            }
        }
    }
    // 하드모드 베스트 스코어 가져오기
    public float HardBestScore
    {
        get => Mathf.Floor(PlayerPrefs.GetFloat("HardBestScore"));
    }
    // 플레이어 및 모드 선택 
    public int ModeSelect
    {
        get { return _modeSelect; }
        set { _modeSelect = value; }
    }
    public int PlayerSelect
    {
        get { return _playerSelect; }
        set { _playerSelect = value; }
    }

}
