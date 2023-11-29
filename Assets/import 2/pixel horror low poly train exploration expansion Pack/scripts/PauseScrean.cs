using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScrean : MonoBehaviour
{
 
    public bool isPaused;
    public static bool CanMove = true;
    public GameObject crossHair;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("im on");
    }
 
// Update is called once per frame
void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
 
            if (isPaused == false)
            {
                isPaused = true;
                CanMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isPaused = false;
                CanMove = true;
                Time.timeScale = 1;
            }
            crossHair.SetActive(!isPaused);

        }
    }
}
