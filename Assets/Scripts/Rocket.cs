using UnityEngine;

public static class RigidBody2DExt
{
    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
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
    public float comboSpeedBonus;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "NoExplode")
        {
            Explode();
            //Debug.Log("Boom");
        }
    }

    private void Explode()
    {


        //Debug.Log("Boom");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);

        foreach(Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null && rb.gameObject.tag != "NoKnockback")
            {
                //rb.AddExplosionForce(blastForce, transform.position, blastRadius);
                if (rb.gameObject.layer == 3)
                {
                    ComboCounter comboCounter = rb.gameObject.GetComponent<ComboCounter>();
                    comboCounter.AddCount();
                    _comboMultiplier = (float)comboCounter.counter * comboSpeedBonus;
                    rb.AddExplosionForce(blastForce, transform.position, blastRadius *(1 + _comboMultiplier));
                }
                else
                {
                    rb.AddExplosionForce(blastForce, transform.position, blastRadius);
                }
            }
        }

        GameObject explosionEffect = Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(explosionEffect, 0.2f);

        Destroy(gameObject);
    }
}
