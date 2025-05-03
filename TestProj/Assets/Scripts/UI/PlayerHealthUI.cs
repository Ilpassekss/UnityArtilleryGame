using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Image healthbarForeground;
    TMP_Text valueText;

    void Start()
    {
        valueText = GetComponentInChildren<TMP_Text>();
        healthbarForeground = transform.Find("Foreground").GetComponent<Image>();
    }

    void Update()
    {
        valueText.text = $"{PlayerManager.instance.health.currentHealth}";
        healthbarForeground.fillAmount = (float)PlayerManager.instance.health.currentHealth / (float)PlayerManager.instance.health.maxHealth;
    }
}
