using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    #region Singleton
    private static PlayerSkillManager s_instance = null;

    public static PlayerSkillManager Instance
    {
        get
        {
            if (s_instance == null)
                return null;
            return s_instance;
        }
    }
#endregion
    private void Awake()
    {
        #region Singleton
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }
    private bool _isEffect = false;

    public bool IsEffect
    {
        get { return _isEffect; }
        set { _isEffect = value; }
    }

    private Dictionary<int, int> _skillList = new();

    public Dictionary<int, int> SkillList
    {
        get { return _skillList; }
        set { _skillList = value; }
    }
    public void SkillLevelUp(int SkillKey)
    {
        SkillList[SkillKey]++;
    }

}
