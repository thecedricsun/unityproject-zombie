using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        // slider = GetComponentInChildren<Slider>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gameObject.SetActive(false);
    }

    public void UpdateHealthBar(float health, float maxHealth)
    {
        gameObject.SetActive(true);
        slider.value = health / maxHealth;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        // the healthbar rotates to face the camera
        // it now needs to move with the zombie
        transform.position = target.position + offset;
    }
}
