using UnityEngine;
using System.Collections;

public class scrWoundController : MonoBehaviour {

    int SpawnedWounds = 0;
    float time;

    public scrPath Path;
    public GameObject WoundPrefab;

	// Use this for initialization
	void Start () {
        time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    // simply add a wound each tenth second
        if (Time.time - time > 9)
        {
            time = Time.time;
            int i;
            switch (Random.Range(0, 6))
            {
                case 0:
                    i = Random.Range(0, Path.Head.Length);
                    Instantiate(WoundPrefab, Path.Head[i].transform.position, Path.Head[i].transform.rotation);
                    break;
                case 1:
                    i = Random.Range(0, Path.Torso.Length);
                    Instantiate(WoundPrefab, Path.Torso[i].transform.position, Path.Torso[i].transform.rotation);
                    break;
                case 2:
                    i = Random.Range(0, Path.LeftArm.Length);
                    Instantiate(WoundPrefab, Path.LeftArm[i].transform.position, Path.LeftArm[i].transform.rotation);
                    break;
                case 3:
                    i = Random.Range(0, Path.RightArm.Length);
                    Instantiate(WoundPrefab, Path.RightArm[i].transform.position, Path.RightArm[i].transform.rotation);
                    break;
                case 4:
                    i = Random.Range(0, Path.LeftLeg.Length);
                    Instantiate(WoundPrefab, Path.LeftLeg[i].transform.position, Path.LeftLeg[i].transform.rotation);
                    break;
                case 5: 
                    i = Random.Range(0, Path.RightLeg.Length);
                    Instantiate(WoundPrefab, Path.RightLeg[i].transform.position, Path.RightLeg[i].transform.rotation);
                    break;
            }
        }
	}
}
