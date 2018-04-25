using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueShoot : MonoBehaviour {
	public GameObject Bullet;
	private float timerGoal;
	private float timer;
	public float shootRate = 0.025f;
	public float shootSpeed = 75f;
	public float shootAngle = 0f;
	public float baseX;
	public float baseY;
	public bool active = true;
	// Use this for initialization
	void Start () {
		baseX = transform.position.x;
		baseY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			timer += Time.deltaTime;
			if (timer >= timerGoal) {
				StartCoroutine(FireProjectile(shootSpeed, shootAngle));
				timerGoal = shootRate;
				timer = 0f;
			}
		}
	}
	
	void ButtonPressed () {
		active = false;
	}
	
	IEnumerator FireProjectile(float speed, float angle)
    {
        // Wait for one second before firing projectiles (animation)
        //yield return new WaitForSecondsRealtime(1);

        // The Projectile instantiation happens here
		
		angle += Random.Range(-5, 6);
        GameObject Projectile;
        Projectile = Instantiate(
            Bullet,
            transform.position + new Vector3(0f, -0.05f, 0f),
            transform.rotation) as GameObject;
		
		transform.position = new Vector3(baseX + Random.Range(-0.01f, 0.01f), baseY + Random.Range(-0.01f, 0.01f), 0);
		Projectile.transform.position += new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f), 0);
		Projectile.transform.localScale += new Vector3(Random.Range(0.3f, 0.6f), Random.Range(0.3f, 0.6f), Random.Range(0.3f, 0.6f));
		Projectile.transform.Rotate(1 - 2 * Random.Range (0f, 1f), 1 - 2 * Random.Range (0f, 1f), Random.Range (0, 360));

        // Set the owner of the new Projectile to this gameObject

        // Retrieve the Rigidbody component from the instantiated Projectile and control it
        Rigidbody2D ProjectileRigidBody;
        ProjectileRigidBody = Projectile.GetComponent<Rigidbody2D>();

        // Tell the Projectile to be "pushed" toward the player by an amount set by ProjectileSpeed
        // Each case is the direction of one projectile
		
		Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
		ProjectileRigidBody.AddForce(dir * speed);
		

        // Wait for one second before firing projectiles (animation)
        //yield return new WaitForSecondsRealtime(1);

        // Allow the flier to move again
       // canMove = true;

        // Basic clean up, set the Projectile to self destruct after 2 seconds.
        Destroy(Projectile, 4.0f);
		
		yield return new WaitForSecondsRealtime(0.3f);
	}
}
