using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

[RequireComponent(typeof(AudioSource))]
public class ZombieController : MonoBehaviour
{
    private Transform player;
    public float followDistance = 999f;
    [SerializeField] public float attackDistance = 2.3f;
    private NavMeshAgent agent;

    private bool isAttacking = false;
    private float attackCooldown = 5f; // Cooldown time between attacks
    private float lastAttackTime = 0f; // Time of the last attack
    [SerializeField] Transform rightArm;
    [SerializeField] Transform playerBody;

    [SerializeField] float health, maxHealth;
    [SerializeField] FloatingHealthBar floatingHealthBar;

    [SerializeField] PlayerHealth playerHealth;
    // [SerializeField] is used to make private variables visible in the inspector

    private WaveSpawner waveSpawner;
    [SerializeField] private int scoreValue = 100;
    // [SerializeField] private ScoreScript scoreScript;
    private GameLogic gameLogic;

    private Animator animator;
    // private NavMeshAgent Agent;
    // public float Velocity;
    public int Arm_Position;
    public int Leg_Position;

    private AudioSource soundSource;
    public AudioClip[] screamSounds;
    public AudioClip deathSound;

    // slider to set up max and min Velocity
    // as a public variable, it will show up in the inspector
    [Range(0.5f, 8f)] public float Velocity;
    public float minAnimatorSpeed = 1f;
    public float maxAnimatorSpeed = 8f;
    public float minNavMeshSpeed = 2f;
    public float maxNavMeshSpeed = 4.8f;
    public float waveNumberModifier = 0.1f;
    public float speedProbabilityExponent = 2f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerBody = GameObject.FindGameObjectWithTag("PlayerBody").transform;

        // score script
        // find tag ScoreScript
        // scoreScript = GameObject.FindGameObjectWithTag("ScoreScript");

        GameObject GameLogicObject = GameObject.FindGameObjectWithTag("GameLogic");

        if (GameLogicObject != null)
        {
            gameLogic = GameLogicObject.GetComponent<GameLogic>();
        }
        else
        {
            Debug.Log("Cannot find Game Logic Script");
        }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waveSpawner = GetComponentInParent<WaveSpawner>();
        animator = GetComponent<Animator>();
        soundSource  = GetComponent<AudioSource>();

        // Velocity = RandomNumber(0.5f, 8f);
        // Velocity = Random.Range(0.5f, 8f);
        // we calculate Velocity using GenerateRandomSpeed method
        Velocity = GenerateRandomSpeed();

        // Arm_Position = RandomNumber(1, 5);
        Arm_Position = Random.Range(1, 5);

        // Leg Position
        Leg_Position = Random.Range(1, 4);

        // assign animator Velocity using RandomNumber method
        animator.SetFloat("Velocity", Velocity);
        // set random int 1 to 3 for animator SetInteger
        animator.SetInteger("Arm_Position", Arm_Position);
        animator.SetInteger("Leg_Position", Leg_Position);
        // change navmesh agent speed to match animator velocity
        agent.speed = MapValue(Velocity, minAnimatorSpeed, maxAnimatorSpeed, minNavMeshSpeed, maxNavMeshSpeed);

        // playerhealth
        playerHealth = FindObjectOfType<PlayerHealth>();

        // sound effects
        InvokeRepeating("NormalSound", 0.001f, 5f);
    }

    private void Update()
    {
        // if player is within followDistance, set destination to player
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= followDistance)
        {
            agent.SetDestination(player.position);
            // keep velocity
            animator.SetFloat("Velocity", Velocity);
            // we change velocity using mapValue method
            agent.speed = MapValue(Velocity, minAnimatorSpeed, maxAnimatorSpeed, minNavMeshSpeed, maxNavMeshSpeed);
        }
        // if the target player is not reachable, stop moving
        if (agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            agent.SetDestination(transform.position);
            // set Velocity to 0
            animator.SetFloat("Velocity", 0);
            agent.speed = 0;
        }

        // attack function
        AttackPlayer();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        floatingHealthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    public void NormalSound()
    {
        int n = Random.Range(1, screamSounds.Length);
        soundSource.clip = screamSounds[n];
        soundSource.PlayOneShot(soundSource.clip);

        screamSounds[n] = screamSounds[0];
        screamSounds[0] = soundSource.clip;
    }

    // do dmg to player
    // to do dmg we need to detect when the player is in range, if is in range, bool isAttacking = true
    // if isAttacking = true, do dmg to player after a certain amount of time, to take animation into account
    // after the animation is done, isAttacking = false
    public void AttackPlayer()
    {
        if (isAttacking)
        {
            // Already attacking, wait for the animation to finish
            // debug
            // Debug.Log("Already attacking");
            return;
        }

        // Compare attack distance with the distance between the zombie and the player

        float distance = Vector3.Distance(rightArm.position, playerBody.position);

        if (distance <= attackDistance && Time.time > lastAttackTime + attackCooldown)
        {
            // Player in range and cooldown period has passed, initiate attack
            StartCoroutine(AttackCoroutine());
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);

        Destroy(gameObject);
        // add score
        // scoreScript.AddScore(scoreValue);
        gameLogic.AddScore(scoreValue);
        if (waveSpawner != null)
        {
            waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
        }
    }

    // return the current wave number from any WaveSpawner in the scene
    private float GetWaveNumber()
    {
        // Use the current wave index from the WaveSpawner as the wave number
        WaveSpawner waveSpawner = FindObjectOfType<WaveSpawner>();
        if (waveSpawner != null)
        {
            // debug
            Debug.Log("Wave number: " + (waveSpawner.currentWaveIndex + 1));
            return waveSpawner.currentWaveIndex + 1; // Add 1 since wave index is 0-based
        }

        // If the WaveSpawner script cannot be found, default to 1
        // debug
        Debug.LogWarning("WaveSpawner script not found. Defaulting to wave number 1.");
        return 1f;
    }

    // return float value corresponding to the proportion between in and out for value
    private float MapValue(float value, float inMin, float inMax, float outMin, float outMax)
    {
        // Map a value from one range to another
        return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
    }

    private IEnumerator AttackCoroutine()
    {
        // Set isAttacking to true to prevent multiple attacks at the same time
        isAttacking = true;
        // animator boolean isAttacking = true
        animator.SetBool("isAttacking", true);

        // speed of zombie is 0 during attack
        // navmeshagent.stop
        agent.Stop();
        agent.velocity = Vector3.zero;
        agent.speed = 0;
        

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(1f);

        // Check if the player is still in range
        float distance = Vector3.Distance(rightArm.position, playerBody.position);
        if (distance > attackDistance)
        {
            // Player moved out of attack range, do not deal damage
            // Debug.Log("Player dodged the attack!");
        }
        else
        {
            // Player still in range, do damage
            playerHealth.TakeDamage(10f);
            // debug
            // Debug.Log("Player health: " + playerHealth.health);
        }

        // another delkay for the attack animation to finish
        yield return new WaitForSeconds(1.3f);
        // Set the last attack time to the current time
        lastAttackTime = Time.time;

        // Set isAttacking to false to allow for another attack
        isAttacking = false;
        // animator boolean isAttacking = false
        animator.SetBool("isAttacking", false);

        // Restore the speed after the attack animation
        agent.Resume();
        agent.speed = MapValue(Velocity, minAnimatorSpeed, maxAnimatorSpeed, minNavMeshSpeed, maxNavMeshSpeed);

        // debug
        // Debug.Log("false");
    }


    // return a random float value between min and max
    // the random is weighted towards the min value for the first waves and towards the max value for the later waves
    private float GenerateRandomSpeed()
    {
        // Calculate the probability of a specific speed based on the wave number and the speedProbabilityExponent
        float probability = Mathf.Pow(Random.value, speedProbabilityExponent);
        float normalizedWaveNumber = Mathf.Clamp01(waveNumberModifier * GetWaveNumber());
        float speedRange = maxAnimatorSpeed - minAnimatorSpeed;
        float randomAnimatorSpeed = minAnimatorSpeed + (normalizedWaveNumber * speedRange * probability);

        return randomAnimatorSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rightArm.position, attackDistance);
    }
}
