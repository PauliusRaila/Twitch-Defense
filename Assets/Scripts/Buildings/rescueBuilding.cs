using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rescueBuilding : MonoBehaviour {

    public float startHealth = 200;
    private float buildingHealth;
    public Image healthBar;

    void Start () {
        buildingHealth = startHealth;
    }

    public void decreaseHealth(float dmg)
    {
        buildingHealth -= dmg;

        healthBar.fillAmount = buildingHealth / startHealth;

        if (buildingHealth <= 0)
        {
            WaveManager.instance.GameOver();
        }

    }
}
