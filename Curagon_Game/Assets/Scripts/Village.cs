using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public static Village instance = null;
    
    [SerializeField] private int level = 1;
    [SerializeField] float experience = 0f;
    [SerializeField] float maxExperience = 100f;
    [SerializeField] float health;
    [SerializeField] float maxHealth = 60f;
    [SerializeField] bool working;
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

        health = maxHealth;
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
        experience += Time.deltaTime * workSpeedScale;
        
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
}
