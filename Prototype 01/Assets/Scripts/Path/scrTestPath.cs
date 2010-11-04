using UnityEngine;
using System.Collections;

public class scrTestPath : MonoBehaviour
{
    public scrPath PathController;

    Transform[] path;

    // Use this for initialization
    void Start()
    {
        path = PathController.FindPath(scrPath.Parts.LeftArm, 1, scrPath.Parts.RightLeg, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Gizmo
    void OnDrawGizmos()
    {
        if (path != null)
            iTween.DrawPath(path, Color.magenta);
    }
}
