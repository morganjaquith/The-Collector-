using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathBarrier : MonoBehaviour {


    public void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
