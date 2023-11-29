using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class FoudreFlicker : MonoBehaviour
{
    public GameObject lightningOne;
    public GameObject lightningTwo;
    public GameObject lightningThree;

    // start
    void Start()
    {
        lightningOne.SetActive(false);
        lightningTwo.SetActive(false);
        lightningThree.SetActive(false);

        Invoke("CallLightning", Random.Range(3f, 10f));
    }

    void CallLightning()
    {
        int r = Random.Range(0, 3);
        if (r == 0)
        {
            lightningOne.SetActive(true);
            Invoke("EndLightning", Random.Range(0.8f, 4.2f));
        }
        else if (r == 1)
        {
            lightningTwo.SetActive(true);
            Invoke("EndLightning", Random.Range(1f, 3.2f));
        }
        else if (r == 2)
        {
            lightningThree.SetActive(true);
            Invoke("EndLightning", Random.Range(0.7f, 2.9f));
        }

        Invoke("CallLightning", Random.Range(3f, 10f));
    }


    void EndLightning()
    {
        lightningOne.SetActive(false);
        lightningTwo.SetActive(false);
        lightningThree.SetActive(false);
    }

    // void CallThunder()
    // {
    //     audioOne.SetActive(true);
    // }

    // void EndThouder()
    // {
    //     audioOne.SetActive(false);
    // }
}