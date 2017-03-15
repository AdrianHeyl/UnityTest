using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class FireBomb : MonoBehaviour {
    
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

    private ScreenOverlay overlay;
    private float white;

    private bool debug;
    private float distance;
    private float dot;

    // Use this for initialization
    void Start ()
    {
        light = gameObject.GetComponentInChildren<Light>();
        dead = false;
        radiating = false;
        range = 40f;
        rangeLerp = Mathf.Pow(range, 1f / 7f) * 7f;
        intensity = 8f;
        intensityLerp = Mathf.Pow(intensity, 1f / 7f) * 7f;
        time = 0f;

        overlay = Camera.main.GetComponent<ScreenOverlay>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (radiating)
        {
            time += Time.deltaTime;
            float t = time / projectileLifetime * 7f;
            /*
            float lerp = Mathf.Min(Mathf.Lerp(rangeLerp, 0f, t), range);
            light.range = Mathf.Pow(lerp, 7f);
            */
            float lerp = Mathf.Min(Mathf.Lerp(intensityLerp, 0f, t), intensity);
            light.intensity = Mathf.Pow(lerp, 7f);

            float f1 = Mathf.Max(white - Mathf.Pow(2, t - 1), 0f);
            //float f2 = Mathf.Lerp(white / 2, 0f, t / 5f);
            float f2 = Mathf.Max(white / (t / 1f) - white / 3, 0f);
            overlay.intensity = f1 + f2;

            if (debug && overlay.intensity < 0.3f)
            {
                Debug.Log("distance: " + distance + ", dot: " + dot + "\nwhite: " + white + ", time:" + time);
                debug = false;
            }
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
            case "Enemy":
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
            case "Terrain":
                GameObject fire = Instantiate(fireSource, gameObject.transform.position, fireSource.transform.rotation);
                fire.GetComponent<ParticleSystem>().Play();
                
                light.intensity = 0f;

                dead = true;
                break;
            case "Fire":
                gameObject.GetComponent<Rigidbody>().velocity *= 0.1f;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 600f * gameObject.GetComponent<Rigidbody>().mass, 0f));

                Vector3 ray = gameObject.transform.position - Camera.main.transform.position;
                distance = ray.magnitude;
                ray.Normalize();
                dot = Mathf.Max(Vector3.Dot(Camera.main.transform.forward, ray), 0f);

                //white = Mathf.Lerp(2f, 0f, (Camera.main.transform.position - gameObject.transform.position).magnitude / 20f);
                white = Mathf.Pow(Mathf.Max(2f - (Camera.main.transform.position - gameObject.transform.position).magnitude / 10, 0f), 1f);
                //white *= dot * Mathf.Abs(dot);
                //white *= (Mathf.Lerp(dot, 0f, 2 * (dot - 0.5f)) + dot + Mathf.Lerp(0f, dot, 2 * (dot + 0.5f))) * dot;
                white *= 0.5f * Mathf.Pow(2 * dot - 1f, 3) + .5f;

                light.range = range;
                light.intensity = intensity;
                light.color = Color.white;
                radiating = true;

                debug = true;

                dead = true;
                break;
        }
    }
}
