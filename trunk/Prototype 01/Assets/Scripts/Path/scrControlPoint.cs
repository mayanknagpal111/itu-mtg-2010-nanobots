using UnityEngine;
using System.Collections;

public class scrControlPoint : MonoBehaviour {

    scrPath.Parts part;
    int index;

    public int Index { get { return index; } set { index = (value < 0) ? 0 : value ; } }
    public scrPath.Parts Part { get { return part; } set { part = value; } }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Gizmo
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 0.1f);
	}
}
