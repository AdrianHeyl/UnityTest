using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour {
    
    public float lifeTime;
    private float time;
    private float destroyTime;

	// Use this for initialization
	void Start () {
        time = 0;
        destroyTime = lifeTime + 1f;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time >= destroyTime)
        {
            Destroy(gameObject);
        }
        else if (time >= lifeTime)
        {
            gameObject.GetComponent<ParticleSystem>().Stop();
        }
	}
}
