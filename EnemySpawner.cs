using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] NomalEnemies;
    public GameObject[] HardEnemies;
    public GameObject Boss;
    public GameObject Soul;

    private float _time = 0;
    public static bool IsSoulSpwan = false;
    public static bool IsNomalBoss = false;
    void Start()
    {
        if (GameManager.Instance.ModeSelect == 0)
            StartCoroutine(CoNomalSpawnEnemy());
        else
        {
            StartCoroutine(CoHardSpawnEnemy());
        }
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (IsSoulSpwan)
        {
            IsSoulSpwan = false;
            StartCoroutine(CoSpawnSoul());
        }
        if (IsNomalBoss)
        {
            IsNomalBoss = false;
            StartCoroutine(CoSpawnNomalBoss());
        }

        // 치트키  --------------------
        if (Input.GetKeyDown(KeyCode.I))
        {
            Instantiate(Boss, new Vector3(13, 0), Boss.transform.rotation);
        }
    }

    IEnumerator CoHardSpawnEnemy()
    {
        while (true)
        {

            while (true)
            {
                if (_time < 10)
                {
                    float ran = Random.Range(-3.7f, 3.7f);
                    Instantiate(HardEnemies[0], new Vector2(11, ran), Quaternion.identity);
                }
                else if (_time < 20)
                {
                    float ran = Random.Range(-3.7f, 3.7f);
                    int ran2 = Random.Range(0, 3);
                    Instantiate(HardEnemies[ran2], new Vector2(11, ran), Quaternion.identity);
                }
                else
                {
                    float ran = Random.Range(-3.7f, 3.7f);
                    int ran2 = Random.Range(0, HardEnemies.Length);
                    Instantiate(HardEnemies[ran2], new Vector2(11, ran), Quaternion.identity);
                }
                yield return new WaitForSeconds(2f);
            }
        }
    }
    IEnumerator CoNomalSpawnEnemy()
    {
        while (true)
        {
            if (_time < 10)
            {
                float ran = Random.Range(-3.7f, 3.7f);
                Instantiate(NomalEnemies[0], new Vector2(11, ran), Quaternion.identity);
            }
            else if (_time < 20)
            {
                float ran = Random.Range(-3.7f, 3.7f);
                int ran2 = Random.Range(0, 5);
                Instantiate(NomalEnemies[ran2], new Vector2(11, ran), Quaternion.identity);
            }
            else
            {
                float ran = Random.Range(-3.7f, 3.7f);
                int ran2 = Random.Range(0, NomalEnemies.Length);
                Instantiate(NomalEnemies[ran2], new Vector2(11, ran), Quaternion.identity);
            }
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator CoSpawnNomalBoss()
    {
        Instantiate(Boss, new Vector3(13, 0), Boss.transform.rotation);
        yield return null;
    }
    // 소울 스폰 로직
    IEnumerator CoSpawnSoul()
    {
        for (int i = 0; i < 10; i++)
        {
            float ran = Random.Range(-3.7f, 3.7f);
            Instantiate(Soul, new Vector2(11, ran), Quaternion.identity);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
