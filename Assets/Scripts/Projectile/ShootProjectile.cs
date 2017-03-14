using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {
    
    public GameObject projectileSource;
    public float spawnOffset;

    public float force;
    public float timePerShot;
    public float lifeTime;
    
    private float time;

    // Use this for initialization
    void Start ()
    {
        time = timePerShot;
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (time >= timePerShot)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);

                ray.direction.Normalize();

                GameObject projectile = Instantiate(projectileSource, transform.position + spawnOffset * transform.forward, transform.rotation);
                projectile.GetComponent<Rigidbody>().AddForce(ray.direction * force);
                Destroy(projectile.gameObject, lifeTime);
                time = time % timePerShot;
            }
        }

	}
}
