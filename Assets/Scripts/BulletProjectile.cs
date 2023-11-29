using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    [SerializeField] private float speed = 100f;
    private Rigidbody bulletRigidbody;

    public float damageAmount = 10f;
    public float headshotMultiplier = 2f;
    public float bodyshotMultiplier = 1f;
    public float legshotMultiplier = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {

        // if other has a tag that contains "Hitbox" (for example, "Hitbox Head")
        if (other.CompareTag("Hitbox Head"))
        {
            // debug code
            Debug.Log("Headshot");

            ZombieController zombieController = other.GetComponentInParent<ZombieController>();
            if (zombieController != null)
            {
                float damageMultiplier = GetDamageMultiplier(other.tag);
                float damage = damageAmount * damageMultiplier;
                zombieController.TakeDamage(damage);
            }
        }
        if (other.CompareTag("Hitbox Body"))
        {
            // debug code
            Debug.Log("Bodyshot");

            ZombieController zombieController = other.GetComponentInParent<ZombieController>();
            if (zombieController != null)
            {
                float damageMultiplier = GetDamageMultiplier(other.tag);
                float damage = damageAmount * damageMultiplier;
                zombieController.TakeDamage(damage);
            }
        }
        if (other.CompareTag("Hitbox Leg"))
        {
            // debug code
            Debug.Log("Legshot");

            ZombieController zombieController = other.GetComponentInParent<ZombieController>();
            if (zombieController != null)
            {
                float damageMultiplier = GetDamageMultiplier(other.tag);
                float damage = damageAmount * damageMultiplier;
                zombieController.TakeDamage(damage);
            }
        }
        if (other.CompareTag("Hitbox Arm"))
        {
            // debug code
            Debug.Log("Armshot");

            ZombieController zombieController = other.GetComponentInParent<ZombieController>();
            if (zombieController != null)
            {
                float damageMultiplier = GetDamageMultiplier(other.tag);
                float damage = damageAmount * damageMultiplier;
                zombieController.TakeDamage(damage);
            }
        }

        // Destroy the bullet if it hits anything
        if (other != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        if (hitObject.CompareTag("Hitbox"))
        {
            // debug code
            Debug.Log("Hitbox hit!");

            ZombieController zombieController = hitObject.GetComponentInParent<ZombieController>();
            if (zombieController != null)
            {
                float damageMultiplier = GetDamageMultiplier(hitObject.tag);
                float damage = damageAmount * damageMultiplier;
                zombieController.TakeDamage(damage);
            }
        }

        // Destroy the bullet
        Destroy(gameObject);
    }

    private float GetDamageMultiplier(string hitboxTag)
    {
        // Determine the damage multiplier based on the hitbox tag
        // For example, if the tag is "Hitbox Head," return the headshot multiplier

        if (hitboxTag.Contains("Head"))
        {
            return headshotMultiplier;
        }
        else if (hitboxTag.Contains("Body"))
        {
            return bodyshotMultiplier;
        }
        else if (hitboxTag.Contains("Leg"))
        {
            return legshotMultiplier;
        }
        else if (hitboxTag.Contains("Arm"))
        {
            return bodyshotMultiplier;
        }

        // Default multiplier if the hitbox tag doesn't match any specific body part
        return 1f;
    }
}
