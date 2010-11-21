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
            scrControlPoint CP;
            switch (Random.Range(0, 6))
            {
                case 0:
                    i = Random.Range(0, Path.Head.Length);
                    CP = Path.Head[i].gameObject.GetComponent<scrControlPoint>();
                    //Instantiate(WoundPrefab, Path.Head[i].transform.position, Path.Head[i].transform.rotation);
                    break;
                case 1:
                    i = Random.Range(0, Path.Torso.Length);
                    CP = Path.Torso[i].gameObject.GetComponent<scrControlPoint>();
                    //Instantiate(WoundPrefab, Path.Torso[i].transform.position, Path.Torso[i].transform.rotation);
                    break;
                case 2:
                    i = Random.Range(0, Path.LeftArm.Length);
                    CP = Path.LeftArm[i].gameObject.GetComponent<scrControlPoint>();
                    //Instantiate(WoundPrefab, Path.LeftArm[i].transform.position, Path.LeftArm[i].transform.rotation);
                    break;
                case 3:
                    i = Random.Range(0, Path.RightArm.Length);
                    CP = Path.RightArm[i].gameObject.GetComponent<scrControlPoint>();
                    //Instantiate(WoundPrefab, Path.RightArm[i].transform.position, Path.RightArm[i].transform.rotation);
                    break;
                case 4:
                    i = Random.Range(0, Path.LeftLeg.Length);
                    CP = Path.LeftLeg[i].gameObject.GetComponent<scrControlPoint>();
                    //Instantiate(WoundPrefab, Path.LeftLeg[i].transform.position, Path.LeftLeg[i].transform.rotation);
                    break;
                case 5: 
                    i = Random.Range(0, Path.RightLeg.Length);
                    CP = Path.RightLeg[i].gameObject.GetComponent<scrControlPoint>();
                    //Instantiate(WoundPrefab, Path.RightLeg[i].transform.position, Path.RightLeg[i].transform.rotation);
                    break;
                default:
                    return;
            }

            Wound(CP);
        }
	}

    void Wound(scrControlPoint CP)
    {
        if (!(CP.BodyPart.Wounded)) // We don't want to inflict a wound on a already wounded body part
        {
            GameObject wound = (GameObject)Instantiate(WoundPrefab, CP.transform.position, CP.transform.rotation);
            wound.GetComponent<scrWound>().Body = CP.BodyPart;
            wound.GetComponent<scrWound>().Points = 100;
            wound.transform.parent = gameObject.transform;

            CP.BodyPart.Wound();
            // Todo: Do some calculation and variation in wounds
        }
    }
}
