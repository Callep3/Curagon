using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    int level;
    int experience;
    float health;
    float maxHealth = 10f;
    bool working;

    private void Start()
    {
        health = maxHealth;
    }
    private void Update()
    {
        health -= Time.deltaTime;
        UIManager.instance.UpdateVillage(Mathf.RoundToInt(health / maxHealth * 100));
    }


}
