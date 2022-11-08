using UnityEngine;

public static class RigidBody2DExt
{
    //I took this class from https://stackoverflow.com/questions/34250868/unity-addexplosionforce-to-2d, I have no idea how it works but it basically allows you to create a 2D ExplosionForce
    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition,
        float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        Vector2 explosionDir = rb.position - explosionPosition;
        float explosionDistance = explosionDir.magnitude / explosionRadius;

        // Normalize without computing magnitude again
        if (upwardsModifier == 0)
        {
            explosionDir /= explosionDistance;
        }
        else
        {
            // If you pass a non-zero value for the upwardsModifier parameter, the direction
            // will be modified by subtracting that value from the Y component of the centre point.
            explosionDir.y += upwardsModifier;
            explosionDir.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, explosionForce, 1 - explosionDistance) * explosionDir, mode);
    }
}

public class Rocket : MonoBehaviour
{
    //public GameObject explosionEffect;

    public float blastRadius = 5f;
    public float blastForce = 700f;
    public GameObject explosion;

    private float _comboMultiplier = 1f;
    public float comboSpeedBonus = 0f; //Adjust this value to add a change in explosion force based on the player's combo count

    //public GameObject player = GameObject.Find("Player");
    //private ComboCounter comboCounter;

    // Start is called before the first frame update
    private void Start()
    {
        //comboCounter = player.GetComponent<ComboCounter>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) //Triggers when the rocket hits something
    {
        //If the object it hits doesn't have the No Explode tag, trigger an explosion
        if (other.gameObject.tag != "NoExplode" && other.gameObject.layer != 3)
        {
            Explode();
            //Debug.Log("Boom");
        }
        //If the object has the No Explode tag and doesn't belong to the Player layer (layer 3), delete the rocket without triggering an explosion
        else if (other.gameObject.tag == "NoExplode" && other.gameObject.layer != 3)
        {
            Destroy(gameObject);
        }
        //Objects on the player layer will neither delete nor explode the rocket if they touch it
        //-- the rocket will just pass right through them (note: the rocket is on the player layer,
        //so this stops rockets from colliding with each other)
    }

    private void Explode()
    {


        //Debug.Log("Boom");

        //Creates a list of all colliders within the blast radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach(Collider2D nearbyObject in colliders) //Runs for each collider in the collider list
        {
            Rigidbody2D rb = nearbyObject.gameObject.GetComponent<Rigidbody2D>(); //Gets the rigidbody of the collider gameObject

            //Checks to make sure there is a rigidbody and that the collider gameObject isn't tagged No Knockback
            //(rockets are tagged this way, for example, to prevent explosions from moving them)
            if (rb != null && rb.gameObject.tag != "NoKnockback")
            {
                //rb.AddExplosionForce(blastForce, transform.position, blastRadius);
                if (rb.gameObject.layer == 3) //Checks if the collider gameObject is on the player layer
                {
                    //Gets the player's ComboCounter script
                    ComboCounter comboCounter = rb.gameObject.GetComponent<ComboCounter>();
                    //Calls the AddCount function of the ComboCounter script
                    comboCounter.AddCount();
                    //Sets combo multiplier based on the combo speed bonus and the player's current combo count
                    _comboMultiplier = (float)comboCounter.counter * comboSpeedBonus;
                    //Adds explosion force using the blastForce float, and adjusted by the combo multiplier
                    rb.AddExplosionForce(blastForce, transform.position,
                        blastRadius * (1 + _comboMultiplier));
                }
                else //If the collider gameObject is on any other layer
                {
                    //Adds explosion force using the blasForce float
                    rb.AddExplosionForce(blastForce, transform.position, blastRadius);
                }
            }
        }

        //Instantiates the explosion effect at the point where the rocket explodes
        GameObject explosionEffect = Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
        //Destroys the explosion effect after 0.2 seconds
        Destroy(explosionEffect, 0.2f);

        //Destroys the rocket
        Destroy(gameObject);
    }
}
