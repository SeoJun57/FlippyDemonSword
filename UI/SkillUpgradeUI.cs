using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradeUI : MonoBehaviour
{   // 스킬 정보가 담긴 스크립터블 오브젝트
    public SkillSO SSO;
    // 힐 스프라이트
    public Sprite HealImage;
    // 정보가 적용될 버튼과 텍스트, 이미지
    public Button[] SkillButtons;
    public TextMeshProUGUI[] SkillNameTexts;
    public TextMeshProUGUI[] SkillDescriptionTexts;
    public Image[] SkillImages;
    

    enum RewardType { Skill, Heal }
    struct RewardData
    {
        public RewardType Type;
        public Skill _Skill;
    }
    // 후보 데이터를 담을 임시 리스트
    private List<Skill> _skillsData = new();
    // 실제 보상 정보가 담긴 리워드 리스트
    private List<RewardData> _rewardData = new();

    // - 랜덤하게 스킬을 뽑기 위한 셔플 알고리즘
    // 리스트를 받아 함수 사용
    public List<T> Shuffle<T>(List<T> list)
    {   // 리스트의 카운트만큼 반복
        for (int i = list.Count - 1; i > 0; i--)
        {   // 임시 랜덤 숫자 정하기(리스트의 인덱스 수 초과하지 않게 리스트 카운트값 + 1로 최댓값 설정)
            int j = Random.Range(0, i + 1);
            // T temp로 위치를 바꿀 값을 정한 후
            T temp = list[i];
            // 정한 값을 랜덤한 인덱스에 넣는다
            list[i] = list[j];
            // 이제 바뀐 값을 아까 정한 임시 인덱스에 넣어주면 둘의 위치가 바뀐다 이걸 반복해 셔플 
            list[j] = temp;
        }
        // 셔플 끝나면 리스트 리턴해주기
        return list;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        DataCreat();
        UISet();
    }

    void DataCreat()
    {   // 임시 데이터 생성 전 이전 데이터 초기화
        _skillsData.Clear();
        _rewardData.Clear();
        // 현재 가지고 있는 스킬의 수만큼 반복
        for (int i = 0; i < SSO.Skills.Length; i++)
        {   // 만약 가지고 있는 스킬일 경우
            if (PlayerSkillManager.Instance.SkillList.ContainsKey(i))
            {   // 만랩이 아니라면 후보데이터에 추가해준다.
                if (SSO.Skills[i].LevelValue.Length - 1 > PlayerSkillManager.Instance.SkillList[i])
                {
                    _skillsData.Add(SSO.Skills[i]);
                }
            }
            else // 보유하지 않은 스킬이라면 바로 추가해준다.
            {
                _skillsData.Add(SSO.Skills[i]);
            }
        }
        if (_skillsData.Count > 1)
        {   // 만약 후보 데이터가 2개 이상이라면 셔플해준다.
            Shuffle(_skillsData);
        }
        // 실제 보상 데이터 생성
        for (int i = 0; i < SkillButtons.Length; i++)
        {   // 만약 스킬 후보가 1개 이상이라면
            if(_skillsData.Count > 0)
            {   // 랜덤한 수를 뽑는다.
                int randomNum = Random.Range(0, 101);
                // 90% 확률로 스킬 추가 10% 확률로 힐 추가
                // 단 힐은 한번만 추가하도록 하기 위해 이미 힐이 추가되었으면 추가 금지
                if (randomNum > 90 && !_rewardData.Contains(new RewardData { Type = RewardType.Heal }))
                {   // 10% 확률로 힐 추가
                    _rewardData.Add(new RewardData
                    {
                        Type = RewardType.Heal
                    });
                }
                else
                {   // 임시 후보 스킬중 랜덤으로 하나 뽑기
                    int ran = Random.Range(0, _skillsData.Count);
                    _rewardData.Add(new RewardData
                    {   // 랜덤 스킬을 보상 데이터에 추가
                        Type = RewardType.Skill,
                        _Skill = _skillsData[ran]
                    });
                    // 추가된 데이터는 후보에서 삭제
                    _skillsData.RemoveAt(ran);
                }     
            }
            else
            {   // 만약 후보 스킬 데이터가 없다면 힐을 추가해준다.
                _rewardData.Add(new RewardData
                {
                    Type = RewardType.Heal
                });
            }
        }
    }

    void UISet()
    {   // 아이템 버튼의 수만큼 반복
        for (int i = 0; i < SkillButtons.Length; i++)
        {   // 추가할 버튼의 모든 기능 삭제
            SkillButtons[i].onClick.RemoveAllListeners();
            // 리워드 타입 구분
            switch (_rewardData[i].Type)
            {   // 스킬일 경우
                case RewardType.Skill:
                    Skill tempSkill = _rewardData[i]._Skill; // 임시로 해당 스킬을 배정
                    int skillKey = tempSkill.Key; // 해당 스킬의 키값 가져오기
                    // 만약 해당하는 스킬을 보유중이라면
                    if (PlayerSkillManager.Instance.SkillList.ContainsKey(tempSkill.Key))
                    {   // 다음 레벨 표시해주기
                        SkillNameTexts[i].text = $"{tempSkill.Name} {PlayerSkillManager.Instance.SkillList[tempSkill.Key] + 2}";
                        // 버튼을 누르면 레벨업 함수 실행되는 기능 추가
                        SkillButtons[i].onClick.AddListener(() => PlayerSkillManager.Instance.SkillLevelUp(skillKey));
                        // 텍스트를 다음 레벨 설명 텍스트로 표시해주기
                        SkillDescriptionTexts[i].text = tempSkill.Descriptions[PlayerSkillManager.Instance.SkillList[tempSkill.Key] + 1];
                    }
                    else // 만약 보유중인 스킬이 아니라면
                    {   // 확득 스킬 레벨인 1로 설정
                        SkillNameTexts[i].text = $"{tempSkill.Name} 1";
                        // 버튼을 누르면 스킬 추가 함수 실행되는 기능 추가
                        SkillButtons[i].onClick.AddListener(() => PlayerSkillManager.Instance.SkillList.Add(skillKey, 0));
                        // 설명은 첫 번째 설명으로 표기
                        SkillDescriptionTexts[i].text = tempSkill.Descriptions[0];
                    }
                    // 해당하는 아이템 스프라이트로 변경
                    SkillImages[i].sprite = tempSkill.Sprite;
                    break;
                case RewardType.Heal: // 힐일 경우
                    // 힐 텍스트, 스프라이트, 설명 변경
                    SkillNameTexts[i].text = "Wandering Soul";
                    SkillImages[i].sprite = HealImage;
                    SkillDescriptionTexts[i].text = "Summens a soul for 5seconds.";
                    // 버튼 클릭 시 힐 로직 기능 추가해주기
                    SkillButtons[i].onClick.AddListener(OnClickHeal);
                    break;
                default:
                    break;
            }

        }
    }
    // 힐 로직
    void OnClickHeal()
    {   // 소울 소환 로직 true로 변경
        EnemySpawner.IsSoulSpwan = true;
    }
    // 어떤 버튼을 누르던 공통으로 해주어야 하는 로직
    public void OnClickUpgradeButton()
    {   // 레벨업 패널 비활성화 및 게임 재개를 위한 시간 설정
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
