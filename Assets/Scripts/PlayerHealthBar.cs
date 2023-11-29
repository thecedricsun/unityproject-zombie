using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    

    private void Awake()
    {
        
        gameObject.SetActive(true);
        // instantiate the healthbar
        slider = GetComponentInChildren<Slider>();
    }

    public void UpdateHealthBar(float health, float maxHealth)
    {
        slider.value = health / maxHealth * 100;
        // debug 
        // Debug.Log(health / maxHealth * 100);
    }

}
