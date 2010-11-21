using UnityEngine;
using System.Collections;

public class scrBodyPart : MonoBehaviour {

    public Material matOrganic;
    public Material matMechanic;
    public Material matOrganicWounded;
    public Material matMechanicWounded;

    bool organic; // Right now a body part is either organic or mechanic. In the future a body part may be x % organic and (1 - x) % mechanic.
    bool wounded;

    public bool Organic { get { return organic; } set { ; } }
    public bool Wounded { get { return wounded; } set { ; } }

	// Use this for initialization
	void Start () {
        organic = true;
        wounded = false;
        renderer.material = matOrganic;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Wound()
    {
        wounded = true;

        if (Organic)
            renderer.material = matOrganicWounded;
        else
            renderer.material = matMechanicWounded;
    }

    public void Heal()
    {
        wounded = false;

        if (Organic)
            renderer.material = matOrganic;
        else
            renderer.material = matMechanic;
    }
}
