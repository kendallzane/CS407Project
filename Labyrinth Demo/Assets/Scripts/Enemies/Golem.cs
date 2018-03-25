using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : EnemyAI
{
    public GameObject GolemDirt;
    private GameObject Player;

    public float speed;
    public int damage;
    private bool canMove;
    private bool alive;
    public bool mercyInvincibility;

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

    // Use this for initialization
    void Start()
    {
        timeSinceBurrow = 0f;
        timeSinceUnburrow = 0f;
        timeSinceDirtCreation = 0f;
        timeToBurrow = Random.Range(burrowMin, burrowMax);
        timeToUnburrow = Random.Range(unburrowMin, unburrowMax);

        Player = GameObject.Find("MainCharacter");

        canMove = false;
        burrowed = false;
        alive = true;
        mercyInvincibility = false;
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
                GameObject DirtSplat;
                DirtSplat = Instantiate(
                    GolemDirt,
                    transform.position,
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

    // Hurt the player character by running into them
    // Ignore collision with Dirt
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && coll.isTrigger)
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
        an.SetTrigger("Dead");
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
        canMove = false;
        alive = false;
        yield return new WaitForSecondsRealtime(1);

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
        yield return new WaitForSeconds(1);
        canMove = true;
    }

    // Disable mercy damage after certain time
    IEnumerator MercyDamage()
    {
        yield return new WaitForSeconds(2);
        mercyInvincibility = false;
    }
}
