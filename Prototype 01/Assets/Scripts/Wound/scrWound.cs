using UnityEngine;
using System.Collections;

public class scrWound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Heal(scrSwarm swarm)
    {
        swarm.Working = false;
        Destroy(gameObject);
    }
}
