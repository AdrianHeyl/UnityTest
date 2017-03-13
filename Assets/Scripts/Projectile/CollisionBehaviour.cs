using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehaviour : MonoBehaviour {

    public GameObject fireSource;
    public float fireLifetime;

    private bool dead;

	// Use this for initialization
	void Start ()
    {
        dead = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void OnCollisionEnter(Collision collision)
    {
        if (dead)
        {
            return;
        }

        switch (collision.collider.tag)
        {
            case "Terrain":
                GameObject fire = Instantiate(fireSource, gameObject.transform.position, fireSource.transform.rotation);
                fire.GetComponent<ParticleSystem>().Play();

                Light light = gameObject.GetComponentInChildren<Light>();
                light.intensity = 0f;
                dead = true;
                break;
            case "Enemy":
                dead = true;
                break;
        }
    }
}
