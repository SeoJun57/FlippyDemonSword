using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // 모드에 따라 플레이어 소환
    public GameObject[] PlayerObjects;
    // 실제 궁극기 버튼
    public Button UltButton;
    // 궁극기 이미지
    public Image UltImage;
    // 플레이어 정보에 따른 궁극기 스프라이트
    public Sprite[] UltSprites;
    // 궁극기 활성화 여부를 알려주는 텍스트
    public TextMeshProUGUI UltText;
    // 패널 오브젝트
    public GameObject ESCPanel;
    public GameObject LevelUpPanel;
    public GameObject ResultPanel;
    public GameObject ClearPanel;
    // 미터 기록 측정용
    public TextMeshProUGUI MetersText;
    // 플레이어 체력 및 경험치
    public Image CurrentHpImage;
    public Image CurrentExpImage;
    // 레벨업 사운드
    public AudioClip LevelUpSound;
    private AudioSource _as;

    public static bool IsClear = false;
    // 기록 측정 및 증가량
    private float _meters = 1;
    private float _meterSpeed = 10;
    // 게임 플레이 여부
    private bool _isPlaying = true;
    public static bool IsSpawnedBoss = false;
    private float _minusHp = 2;
    private void Start()
    {   // 컴포넌트 가져오기
        _as = GetComponent<AudioSource>();
        // 플레이어 및 모드 세팅
        PlayModeSetting();
        Time.timeScale = 1;
        _isPlaying = true;
        IsClear = false;
        ESCPanel.SetActive(false);
        ResultPanel.SetActive(false);
        UltButton.image.fillAmount = 0;
    }
    void PlayModeSetting()
    {
        // 플레이어 선택 여부에 따른 플레이어 소환
        Instantiate(PlayerObjects[GameManager.Instance.PlayerSelect], new Vector2(-6.5f, -2), PlayerObjects[GameManager.Instance.PlayerSelect].transform.localRotation);
        // 플레이어 정보에 따른 궁극기 이미지 할당
        UltImage.sprite = GameManager.Instance.PlayerSelect == 0 ? UltSprites[0] : UltSprites[1];
        // 플레이어 정보에 따른 체력 감소량 설정
        _minusHp = GameManager.Instance.PlayerSelect == 0 ? 2 : 4;
        // 하드모드는 레벨업 시 필요 경험치가 2.5배 더 높음
        PlayerStatsManager.Instance.MaxExp = GameManager.Instance.ModeSelect == 0 ? PlayerStatsManager.Instance.MaxExp : PlayerStatsManager.Instance.MaxExp * 2.5f;
        UltButton.onClick.AddListener(OnClickUltButton);
    }
    void Update()
    {
        // 현재 미터(스코어) 표시
        _meters += _meterSpeed * Time.deltaTime;
        // 노말모드면 보스 소환
        if(GameManager.Instance.ModeSelect == 0 && _meters >= 5000 && IsSpawnedBoss == false)
        {
            IsSpawnedBoss = true;
            EnemySpawner.IsNomalBoss = true;
        }
        if(GameManager.Instance.ModeSelect == 1 &&  (int)_meters % 1000 == 0 && IsSpawnedBoss == false)
        {
            IsSpawnedBoss = true;
            EnemySpawner.IsNomalBoss = true;
        }
        // 기록 표시용
        MetersText.text = $"{Mathf.Floor(_meters)}M";
        // 체력 감소 로직
        PlayerStatsManager.Instance.CurrentHp -= _minusHp * Time.deltaTime * PlayerStatsManager.Instance.HpDecrease;
        // 체력바 업데이트
        CurrentHpImage.fillAmount = PlayerStatsManager.Instance.CurrentHp / PlayerStatsManager.Instance.MaxHp;
        // 경험치바 업데이트
        CurrentExpImage.fillAmount = PlayerStatsManager.Instance.CurrentExp / PlayerStatsManager.Instance.MaxExp;
        LevelUp();
        Dead();
        Clear();
        // 플레이 중이 아니면 궁극기 사용 불가
        if (_isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            UltButton.interactable = false;
            Pause();
        }
        else
        {
            UltButton.interactable = true;
        }
        // 궁극기 쿨타임 설정
        UltText.text = UltButton.image.fillAmount == 0 ? 1.ToString() : 0.ToString();
        if (UltButton.image.fillAmount != 0)
        {
            UltButton.image.fillAmount -= 0.1f * PlayerStatsManager.Instance.CoolDown * Time.deltaTime;
        }


        // 치트키 나중에 삭제 해야함 **************
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerStatsManager.Instance.CurrentExp = PlayerStatsManager.Instance.MaxExp;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerStatsManager.Instance.CurrentHp -= 50;
        }
    }

    void Clear()
    {
        if(IsClear)
        {
            _isPlaying = false;
            Time.timeScale = 0;
            if (GameManager.Instance.ModeSelect == 0)
            {
                GameManager.Instance.NomalScore = _meters;
            }

            ClearPanel.SetActive(true);
        }
    }

    // 일시정지 로직 -------------------------
    public void Pause()
    {   // ESC나 일시정지 버튼을 눌렀을 때 실행
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        ESCPanel.SetActive(!ESCPanel.activeSelf);
    }
    // 궁극기 클릭하면 쿨타임 표시
    void OnClickUltButton()
    {   // 궁극기 쿨타임 표시
        if (UltButton.image.fillAmount <= 0)
        {
            UltButton.image.fillAmount = 1;
            if (GameManager.Instance.PlayerSelect == 0)
            {
                StartCoroutine(CoNomalUlt());
            }
            else
            {
                StartCoroutine(CoHardUlt());
            }
        }
    }
    // 레벨업 로직
    void LevelUp()
    {
        // 레벨업 로직
        if (PlayerStatsManager.Instance.CurrentExp >= PlayerStatsManager.Instance.MaxExp)
        {   // 레벨업 시 필요한 총 경험치 빼주기
            PlayerStatsManager.Instance.CurrentExp -= PlayerStatsManager.Instance.MaxExp;
            // 총 레벨업 시 필요한 경험치 요구량 증가
            PlayerStatsManager.Instance.MaxExp += 30;
            Time.timeScale = 0;
            _as.PlayOneShot(LevelUpSound);
            LevelUpPanel.SetActive(true);
        }
    }
    // 죽었을때 로직
    void Dead()
    {    
        if (PlayerStatsManager.Instance.CurrentHp <= 0)
        {
            _isPlaying = false;
            Destroy(GameObject.FindWithTag("Player"));
            Time.timeScale = 0;
            if (GameManager.Instance.ModeSelect == 0)
            {
                GameManager.Instance.NomalScore = _meters;
            }
            else
            {
                GameManager.Instance.HardScore = _meters;
            }
            ResultPanel.SetActive(true);
        }
    }
    // 노말모드 궁극기 사용 시 로직
    IEnumerator CoNomalUlt()
    {   // 플레이어의 로테이션 속도, 크기, 적 속도, 배경 이동 속도, 진행 스코어 속도 증가
        float currentRotationSpeed = PlayerStatsManager.Instance.CurrentRotationSpeed;
        float currentScale = PlayerStatsManager.Instance.CurrentScale;
        float currentEnemySpeed = PlayerStatsManager.Instance.EnemySpeed;
        float currentBackGroundSpeed = 0.5f;
        float currentMeterSpeed = _meterSpeed;
        BackGround.BackGroundSpeed = 1.5f;
        _meterSpeed *= 2;
        UltText.text = 0.ToString();
        Player.IsHit = false;
        PlayerStatsManager.Instance.CurrentRotationSpeed = 18;
        PlayerStatsManager.Instance.EnemySpeed *= 2;
        PlayerStatsManager.Instance.CurrentScale = PlayerStatsManager.Instance.UltScale;
        // 3초동안만 유지
        yield return new WaitForSeconds(3f);
        // 시간 끝나면 다시 원래대로
        Player.IsHit = true;
        PlayerStatsManager.Instance.CurrentRotationSpeed = currentRotationSpeed;
        PlayerStatsManager.Instance.EnemySpeed = currentEnemySpeed;
        PlayerStatsManager.Instance.CurrentScale = currentScale;
        _meterSpeed = currentMeterSpeed;
        BackGround.BackGroundSpeed = currentBackGroundSpeed;
    }
    // 2번 플레이어 궁극기
    IEnumerator CoHardUlt()
    {
        Player.IsHit = false;
        PlayerStatsManager.Instance.HardUlt = true;
        yield return new WaitForSeconds(3f);
        Player.IsHit = true;
    }
}
