using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;

    private int maxValueofHealth = 100;

    public void SetHeath(int valueOfHealth)
    {
        healthBar.fillAmount = valueOfHealth / (float)maxValueofHealth;
    }

    public void SetMaxHealth(int maxValueofHealth)
    {
        this.maxValueofHealth = maxValueofHealth;
    }
}
