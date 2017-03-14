using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehaviour : MonoBehaviour {

    public GameObject fireSource;
    public float fireLifetime;
    public float projectileLifetime;

    private bool dead;

    private Light light;
    private bool radiating;
    private float range;
    private float rangeLerp;
    private float intensity;
    private float intensityLerp;
    private float time;

	// Use this for initialization
	void Start ()
    {
        light = gameObject.GetComponentInChildren<Light>();
        dead = false;
        radiating = false;
        range = 50f;
        rangeLerp = Mathf.Pow(range, 1f / 7f) * 7f;
        intensity = 8f;
        intensityLerp = Mathf.Pow(intensity, 1f / 7f) * 7f;
        time = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        if (radiating)
        {
            float t = time / projectileLifetime * 7f;
            /*
            float lerp = Mathf.Min(Mathf.Lerp(rangeLerp, 0f, t), range);
            light.range = Mathf.Pow(lerp, 7f);
            */
            float lerp = Mathf.Min(Mathf.Lerp(intensityLerp, 0f, t), intensity);
            light.intensity = Mathf.Pow(lerp, 7f);
        }
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

                Debug.Log("TERRAIN!");

                GameObject fire = Instantiate(fireSource, gameObject.transform.position, fireSource.transform.rotation);
                fire.GetComponent<ParticleSystem>().Play();

                //light = gameObject.GetComponentInChildren<Light>();
                light.intensity = 0f;

                dead = true;
                break;
            case "Enemy":

                Debug.Log("ENEMY!");

                dead = true;
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (dead)
        {
            return;
        }

        switch (collider.tag)
        {
            case "Fire":

                Debug.Log("FIRE!");

                gameObject.GetComponent<Rigidbody>().velocity *= 0.1f;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 600f * gameObject.GetComponent<Rigidbody>().mass, 0f));
                
                light.range = range;
                light.intensity = intensity;
                light.color = Color.white;
                radiating = true;

                dead = true;
                break;
        }
    }
}
