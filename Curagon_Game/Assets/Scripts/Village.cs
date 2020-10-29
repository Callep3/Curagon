using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public static Village instance = null;

    Curagon curagon;

    int level;
    float experience;
    float maxExperience;
    
    float health;
    float maxHealth;
    
    public bool working;
    float workSpeedScale;

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

        GetAllComponents();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!UIManager.instance.gamePaused)
        {
            health = Mathf.Clamp(health - Time.deltaTime, 0f, maxHealth);
            UIManager.instance.UpdateVillage(health / maxHealth, experience / maxExperience, level);

            if (working)
            {
                Working();
            }
        }
    }

    void Init()
    {
        working = false;
        maxHealth = 60f;
        health = maxHealth;
        level = 1;
        experience = 0;
        maxExperience = 100;
        workSpeedScale = 1f;
    }

    void GetAllComponents()
    {
        curagon = GameObject.FindGameObjectWithTag("Player").GetComponent<Curagon>();
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
        
        //TODO Make health increase when working
    }

    public void SetWork(bool active)
    {
        working = active;
    }

    public void Restart()
    {
        Init();
    }

    public void SetCuragon(Curagon newCuragon)
    {
        curagon = newCuragon;
    }
}
