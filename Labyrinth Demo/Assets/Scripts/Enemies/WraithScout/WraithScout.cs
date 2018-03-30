using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithScout : Entity {

    public float spinSpeed = 1.0f;      //scalar to change the speed the enemy moves during the spin animation
    public float pounceSpeed = 2.5f;    //scalar to change the speed the enemy moves during the pounce animation
    public float wanderTime = 3.0f;     //time inbetween samples of points to wander to
    public int circleTime = 10;         //time spent running around player
    public int circleRadius = 5;        //distance to run around player at
    public int groupedDistanceRange = 7;    //distance required for another wraithScout to be determined as "grouped"
    [HideInInspector] public bool isAlpha = false;          //flag to denote which wraithscout communicates
    [HideInInspector] public bool stateOverride = false;    //set externally to bypass the circle counter
    [HideInInspector] public int attackTurn = 0;            //order of attack set by alpha
    [HideInInspector] public List<GameObject> wraithScouts = new List<GameObject>();    //list of wraith scouts in the current scene
    private float timeSample = 0.0f;
    private float wanderTimeSample = 0.0f;
    private bool attacked = false;                      //flag set if the attack FSM state was performed
    public Path p;


    // Use this for initialization
    void Start () {
		//p = new Path(PathFinding.ReturnAStarPath(this.gameObject, gc.player, new List<string> { "Entity", "Player" }, gc.nodelist, 0.6f), 0.2f);
    }

    protected override void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        rb = GetComponentInParent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        //animation logic
        SetDirection();
        CheckFlipSprite(EDir.Right);
        an.SetInteger("Direction", (int)direction);
        if (attacking != false)
        {
            an.SetTrigger("Attack");
            attacking = false;
        }
		rb.AddForce (AASeek (gc.player.transform.position));
    }

    private void FixedUpdate()
    {
       // Debug.Log("currentState = " + currentState.ToString());
        if (Time.timeSinceLevelLoad > (wanderTime + wanderTimeSample))
        {
            wanderTimeSample = Time.timeSinceLevelLoad;
        }
        if (Time.timeSinceLevelLoad > (circleTime + timeSample))
        {
            timeSample = Time.timeSinceLevelLoad;
        }

        if (!animationMovementOverride && isEnabled)
        {
            if (wanderTimeSample == Time.timeSinceLevelLoad)
            {
                aaParams.wanderTrigger = true;
            }
            FSM();
        }
        Debug.Log("current state = " + currentState.ToString());
        rb.AddForce(AAFollow(p));
    }

    #region Finite State Machine
    protected override void FSM()
    {
        switch (currentState)
        {
            case EStateID.AttackPlayer:
                if (attacked)
                {
                    attacked = false;
                    currentState = EStateID.CirclePlayer;
                }
                break;
            case EStateID.CirclePlayer:
                if (timeSample == Time.timeSinceLevelLoad || stateOverride)
                {
                    stateOverride = false;
                    if (wraithScouts.Count > 0) //will need a trigger for groups
                    {
                        currentState = EStateID.GroupAttackPlayer;
                    }
                    else
                    {
                        currentState = EStateID.AttackPlayer;
                    }
                }
                break;
            case EStateID.GroupAttackPlayer:
                if (attackTurn <= 0)
                {
                    currentState = EStateID.AttackPlayer;
                }
                break;
            case EStateID.GroupUp:
                if (Vector3.Distance(gc.player.transform.position, this.transform.position) <= groupedDistanceRange)
                {
                    currentState = EStateID.CirclePlayer;
                }
                break;
            default:    //Idle
                if (wraithScouts.Count > 0 && stateOverride && Vector3.Distance(gc.player.transform.position, this.transform.position) > groupedDistanceRange)
                {
                    currentState = EStateID.GroupUp;
                }
                else if (Vector3.Distance(gc.player.transform.position, this.transform.position) <= groupedDistanceRange)
                {
                    currentState = EStateID.CirclePlayer;
                }
                else
                {
                    //rb.AddForce(AAWander());
                }
                break;
        }
    }

    void AttackPlayer()
    {

    }

    void CirclePlayer()
    {

    }

    void GroupAttackPlayer()
    {

    }

    void GroupUp()
    {

    }
    #endregion

    #region AnimationFunctions
    public void PounceWrapper()
    {
        StartCoroutine(Pounce());
    }

    public void SpinWrapper()
    {
        StartCoroutine(Spin());
    }

    IEnumerator Pounce()
    {
        animationMovementOverride = true;
        float speed = pounceSpeed;
        if (an.GetInteger("Direction") == 0)
        {
            speed = -speed;
        }
        this.rb.velocity = speed * new Vector3(0.0f, 1.0f, 0.0f);
        yield return new WaitForSeconds(0.6f);
        this.rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(1.0f);
        animationMovementOverride = false;
    }

    IEnumerator Spin()
    {
        animationMovementOverride = true;
        float speed = spinSpeed;
        if (an.GetInteger("Direction") == 1)
        {
            speed = -speed;
        }
        this.rb.velocity = speed * new Vector3(0.1f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.05f);
        this.rb.velocity = speed * new Vector3(1.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.05f);
        this.rb.velocity = speed * new Vector3(3.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.2f);
        this.rb.velocity = speed * new Vector3(4.5f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.2f);
        this.rb.velocity = speed * new Vector3(3.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.2f);
        this.rb.velocity = speed * new Vector3(1.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.05f);
        this.rb.velocity = speed * new Vector3(0.1f, 0.0f, 0.0f);
        yield return new WaitForSeconds(0.6f);
        this.rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        animationMovementOverride = false;
    }
    #endregion
}
