using UnityEngine;
using System.Collections;

public class scrBodyPart : MonoBehaviour {

    public Material matOrganic;
    public Material matMechanic;

    bool organic; // Right now a body part is either organic or mechanic. In the future a body part may be x % organic and (1 - x) % mechanic.

    public bool Organic { get { return organic; } set { ;} }

	// Use this for initialization
	void Start () {
        organic = true;
        renderer.material = matOrganic;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
