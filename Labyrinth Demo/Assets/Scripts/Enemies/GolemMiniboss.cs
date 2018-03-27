using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMiniboss : EnemyAI
{
    public GameObject GolemMinibossDirt;
    private GameObject Player;
    public GameObject SpawnedGolem;

    private GameObject spawn1;
    private GameObject spawn2;
    private GameObject spawn3;

    public float speed;
    public int damage;
    private bool canMove;
    private bool alive;
    public bool mercyInvincibility;
    private bool spawned;

    public float dirtCreationRate;
    private float timeSinceDirtCreation;
    public float dirtLifetime;

    private float timeSinceUnburrow;
    private float timeToBurrow;
    public float burrowMin;
    public float burrowMax;

    private float timeSinceBurrow;
    private float timeToUnburrow;
    public float unburrowMin;
    public float unburrowMax;

    private bool burrowed;

    private int countSinceSpawning;
    public int spawnCount;
    private bool canSpawn;
    bool haveSpawned;

    // Use this for initialization
    void Start()
    {
        timeSinceBurrow = 0f;
        timeSinceUnburrow = 0f;
        timeSinceDirtCreation = 0f;
        timeToBurrow = Random.Range(burrowMin, burrowMax);
        timeToUnburrow = Random.Range(unburrowMin, unburrowMax);

        Player = GameObject.Find("MainCharacter");

        an.SetBool("Spawn", false);

        countSinceSpawning = 10000;

        canMove = false;
        burrowed = false;
        alive = true;
        mercyInvincibility = false;
        spawned = false;
        canSpawn = false;
        haveSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!alive)
        {
            return;
        }

        timeSinceUnburrow += Time.deltaTime;

        if (timeSinceUnburrow > timeToBurrow)
        {
            if (!burrowed)
            {
                countSinceSpawning++;
            }
            if (countSinceSpawning >= spawnCount && canSpawn == true && spawned == false)
            {
                an.SetBool("Spawn", true);
                an.SetTrigger("Burrow");
                spawned = true;
                mercyInvincibility = true;
                burrowed = true;
                StartCoroutine(SpawnGolems());
            }
            
            if (spawned == true)
            {
                mercyInvincibility = true;

                if (spawn1 == null && spawn2 == null && spawn3 == null && haveSpawned == true)
                {
                    timeSinceBurrow = 0f;
                    timeSinceUnburrow = 0f;
                    timeSinceDirtCreation = 0f;
                    countSinceSpawning = 0;

                    canMove = false;
                    burrowed = false;
                    spawned = false;
                    haveSpawned = false;
                    mercyInvincibility = false;
                    an.SetBool("Spawn", false);
                    an.SetTrigger("Unburrow");
                    rb.velocity = new Vector2(0, 0);
                    countSinceSpawning = 0;
                }
            }
            else
            {
                timeSinceBurrow += Time.deltaTime;
                timeSinceDirtCreation += Time.deltaTime;

                if (!burrowed)
                {
                    an.SetTrigger("Burrow");
                    StartCoroutine(Burrowing());
                    burrowed = true;
                }

                if (timeSinceBurrow > timeToUnburrow)
                {
                    timeSinceBurrow = 0f;
                    timeSinceUnburrow = 0f;
                    timeSinceDirtCreation = 0f;

                    canMove = false;
                    burrowed = false;
                    an.SetTrigger("Unburrow");
                    rb.velocity = new Vector2(0, 0);
                }

                if (timeSinceDirtCreation > dirtCreationRate)
                {
                    Vector3 dirtPosition = transform.position;
                    dirtPosition.z += 1;

                    GameObject DirtSplat;
                    DirtSplat = Instantiate(
                        GolemMinibossDirt,
                        dirtPosition,
                        transform.rotation) as GameObject;


                    timeSinceDirtCreation = 0f;
                    Destroy(DirtSplat, dirtLifetime);
                }

                if (canMove && Player != null)
                {

                    // Find distance between Player and Golem

                    Vector3 targetPosition = Player.transform.position;
                    Vector3 currentPosition = this.transform.position;
                    Vector3 difference = currentPosition - targetPosition;

                    // Use the location differences to move the golem towards the player
                    // All of the following are conditionals for which direction to move

                    float playerSize = 0.1f;
                    if (difference.x > playerSize && difference.y > playerSize)
                    {
                        rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                    }
                    else if ((difference.x <= playerSize && difference.x >= playerSize * -1) && difference.y > playerSize)
                    {
                        rb.velocity = new Vector2(0, speed * -1);
                    }
                    else if (difference.x < playerSize * -1 && difference.y > playerSize)
                    {
                        rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * -1 * Mathf.Sqrt(2) / 2);
                    }
                    else if (difference.x < playerSize * -1 && (difference.y <= playerSize && difference.y >= playerSize * -1))
                    {
                        rb.velocity = new Vector2(speed, 0);
                    }
                    else if (difference.x < playerSize * -1 && difference.y < playerSize * -1)
                    {
                        rb.velocity = new Vector2(speed * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                    }
                    else if ((difference.x <= playerSize && difference.x >= playerSize * -1) && difference.y < playerSize * -1)
                    {
                        rb.velocity = new Vector2(0, speed);
                    }
                    else if (difference.x > playerSize && difference.y < playerSize * -1)
                    {
                        rb.velocity = new Vector2(speed * -1 * Mathf.Sqrt(2) / 2, speed * Mathf.Sqrt(2) / 2);
                    }
                    else if (difference.x > playerSize && (difference.y <= playerSize && difference.y >= playerSize * -1))
                    {
                        rb.velocity = new Vector2(speed * -1, 0);
                    }
                    else
                    {
                        int xSign;
                        int ySign;

                        if (difference.x > 0)
                        {
                            xSign = 1;
                        }
                        else if (difference.x < 0)
                        {
                            xSign = -1;
                        }
                        else
                        {
                            xSign = 0;
                        }
                        if (difference.y > 0)
                        {
                            ySign = 1;
                        }
                        else if (difference.y < 0)
                        {
                            ySign = -1;
                        }
                        else
                        {
                            ySign = 0;
                        }

                        rb.velocity = new Vector2(speed * xSign * -1, speed * ySign * -1);
                    }
                }
            }
        }
    }

    IEnumerator SpawnGolems()
    {
        yield return new WaitForSecondsRealtime(2);

        Vector3 golemPosition1 = transform.position;
        Vector3 golemPosition2 = transform.position;
        Vector3 golemPosition3 = transform.position;

        golemPosition1.x += 1;
        golemPosition2.x -= 1;
        golemPosition3.y += 1;

        spawn1 = Instantiate(
            SpawnedGolem,
            golemPosition1,
            transform.rotation) as GameObject;
        spawn2 = Instantiate(
            SpawnedGolem,
            golemPosition2,
            transform.rotation) as GameObject;
        spawn3 = Instantiate(
            SpawnedGolem,
            golemPosition3,
            transform.rotation) as GameObject;

        haveSpawned = true;
    }

    // Hurt the player character by running into them
    // Ignore collision with Dirt
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && coll.isTrigger && spawned == false)
        {
            coll.GetComponent<PlayerHealth>().TakeDamage(damage, (Vector2)coll.transform.position - (Vector2)transform.position);

            if (burrowed)
            {
                timeSinceBurrow = 0f;
                timeSinceUnburrow = 0f;
                timeSinceDirtCreation = 0f;

                canMove = false;
                burrowed = false;
                an.SetTrigger("Unburrow");
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    // Take damage
    public override void TakeDamage(int damage, Vector2 dirHit)
    {
        if (mercyInvincibility)
        {
            return;
        }
        if (alive)
        {
            mercyInvincibility = true;
            canSpawn = true;
            StartCoroutine(MercyDamage());
            eh.TakeDamage(damage);

            if (burrowed)
            {
                timeSinceBurrow = 0f;
                timeSinceUnburrow = timeToBurrow - 0.5f;
                timeSinceDirtCreation = 0f;

                canMove = false;
                burrowed = false;
                an.SetTrigger("Unburrow");
                rb.velocity = new Vector2(0, 0);
            }
            else
            {
                timeSinceBurrow = 0f;
                timeSinceUnburrow = timeToBurrow - 0.5f;
                timeSinceDirtCreation = 0f;

                an.SetTrigger("Hurt");
            }
        }
    }

    // Golem death
    public override IEnumerator OnDeath()
    {
        if (burrowed)
        {
            an.SetTrigger("Dead");
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.isKinematic = true;
            canMove = false;
            alive = false;
            yield return new WaitForSecondsRealtime(0.5f);
        }
        else
        {
            an.SetTrigger("Dead");
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.isKinematic = true;
            canMove = false;
            alive = false;
            yield return new WaitForSecondsRealtime(0.5f);
        }

        Destroy(gameObject);
    }

    // Finish hurting
    public void HurtFinished()
    {
        mercyInvincibility = false;
        if (alive)
        {
            canMove = true;
        }
    }

    // Burrowing
    IEnumerator Burrowing()
    {
        yield return new WaitForSeconds(0.8f);
        canMove = true;
    }

    // Disable mercy damage after certain time
    IEnumerator MercyDamage()
    {
        yield return new WaitForSeconds(1.25f);
        mercyInvincibility = false;
    }
}
