using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public static Village instance = null;

    [SerializeField] Curagon curagon;

    [SerializeField] private int level = 1;
    [SerializeField] float experience = 0f;
    [SerializeField] float maxExperience = 100f;
    [SerializeField] float health;
    [SerializeField] float maxHealth = 60f;
    [SerializeField] public bool working;
    [SerializeField] private float workSpeedScale = 1f;

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

        Restart();
    }

    private void Update()
    {
        health = Mathf.Clamp(health - Time.deltaTime, 0f, maxHealth);
        UIManager.instance.UpdateVillage(health / maxHealth, experience / maxExperience, level);

        if (working)
        {
            Working();
        }
    }

    private void Working()
    {
        if (curagon.GetWorkingCondition() <= 0)
        {
            working = false;
            return;
        }

        experience += Time.deltaTime * workSpeedScale * curagon.GetWorkingCondition();

        if (experience >= maxExperience)
        {
            level++;

            maxExperience *= 1.5f;
            experience = 0;

            maxHealth *= 1.5f;
            health = maxHealth;
        }
    }

    public void SetWork(bool active)
    {
        working = active;
    }

    public void Restart()
    {
        working = false;
        maxHealth = 60f;
        health = maxHealth;
        level = 1;
        experience = 0;
        maxExperience = 100;
        workSpeedScale = 1f;
    }
}
