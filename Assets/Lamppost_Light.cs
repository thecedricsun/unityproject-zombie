using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamppost_Light : MonoBehaviour
{
    public GameObject spotlight; // Reference to the Spotlight GameObject
    private float randomInterval;
    private float randomDuration;

    void Start()
    {
        // Set initial values for randomInterval and randomDuration
        randomInterval = Random.Range(1f, 5f);
        randomDuration = Random.Range(2f, 6f);

        // Call a coroutine to control the spotlight behavior
        StartCoroutine(RandomizeSpotlight());
    }

    IEnumerator RandomizeSpotlight()
    {
        while (true)
        {
            // Turn on the spotlight
            spotlight.SetActive(true);

            // Wait for a random duration
            yield return new WaitForSeconds(randomDuration);

            // assign new random
            randomDuration = Random.Range(1f, 30f);

            // Turn off the spotlight
            spotlight.SetActive(false);

            // Wait for a random interval before starting again
            yield return new WaitForSeconds(randomInterval);

            // assign new random
            randomInterval = Random.Range(0.01f, 0.3f);
        }
    }
}
