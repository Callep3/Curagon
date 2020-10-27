using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public static Village instance = null;
    int level;
    [SerializeField]
    float experience = 0f;
    [SerializeField]
    float experienceTreshold = 100f;
    [SerializeField]
    float health;
    [SerializeField]
    float maxHealth = 10f;
    [SerializeField]
    bool working;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        health = maxHealth;
    }

    private void Update()
    {
        health -= Time.deltaTime;
        UIManager.instance.UpdateVillage(Mathf.Round(health / maxHealth * 100), experience / experienceTreshold);
        if (working)
        {
            Working();
        }
    }

    private void Working()
    {
        experience += Time.deltaTime;
        if (experience >= experienceTreshold)
        {
            level++;
            experienceTreshold *= 1.5f;
            experience = 0;
        }
    }

    public void SetWork(bool active)
    {
        working = active;
    }
}
