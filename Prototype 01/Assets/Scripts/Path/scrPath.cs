using UnityEngine;
using System.Collections.Generic;

public class scrPath : MonoBehaviour {

    public enum Parts { Head, Torso, LeftArm, RightArm, LeftLeg, RightLeg };

	public Transform[] Torso;
	public Transform[] Head;
	public Transform[] LeftArm;
	public Transform[] RightArm;
	public Transform[] LeftLeg;
	public Transform[] RightLeg;
	
	// Use this for initialization
	void Start () {
        for (int i = 0; i < Torso.Length; i++)
        {
            scrControlPoint CP = (scrControlPoint)(Torso[i].gameObject.GetComponent("scrControlPoint"));
            CP.Part = Parts.Torso;
            CP.Index = i;
        }

        for (int i = 0; i < Head.Length; i++)
        {
            scrControlPoint CP = (scrControlPoint)(Head[i].gameObject.GetComponent("scrControlPoint"));
            CP.Part = Parts.Head;
            CP.Index = i;
        }

        for (int i = 0; i < LeftArm.Length; i++)
        {
            scrControlPoint CP = (scrControlPoint)(LeftArm[i].gameObject.GetComponent("scrControlPoint"));
            CP.Part = Parts.LeftArm;
            CP.Index = i;
        }

        for (int i = 0; i < RightArm.Length; i++)
        {
            scrControlPoint CP = (scrControlPoint)(RightArm[i].gameObject.GetComponent("scrControlPoint"));
            CP.Part = Parts.RightArm;
            CP.Index = i;
        }

        for (int i = 0; i < LeftLeg.Length; i++)
        {
            scrControlPoint CP = (scrControlPoint)(LeftLeg[i].gameObject.GetComponent("scrControlPoint"));
            CP.Part = Parts.LeftLeg;
            CP.Index = i;
        }

        for (int i = 0; i < RightLeg.Length; i++)
        {
            scrControlPoint CP = (scrControlPoint)(RightLeg[i].gameObject.GetComponent("scrControlPoint"));
            CP.Part = Parts.RightLeg;
            CP.Index = i;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Gizmos - Draw Skeleton
	void OnDrawGizmos() {
		iTween.DrawLine(Torso, Color.blue);
		iTween.DrawLine(Head, Color.blue);
		iTween.DrawLine(LeftArm, Color.blue);
		iTween.DrawLine(RightArm, Color.blue);
		iTween.DrawLine(LeftLeg, Color.blue);
		iTween.DrawLine(RightLeg, Color.blue);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Head[0].transform.position, Torso[0].transform.position);
        Gizmos.DrawLine(Torso[0].transform.position, LeftArm[0].transform.position);
        Gizmos.DrawLine(Torso[0].transform.position, RightArm[0].transform.position);
        Gizmos.DrawLine(Torso[Torso.Length - 1].transform.position, LeftLeg[0].transform.position);
        Gizmos.DrawLine(Torso[Torso.Length - 1].transform.position, RightLeg[0].transform.position);
	}

    public Transform[] FindPath(Parts StartPart, uint StartIndex, Parts EndPart, uint EndIndex)
    {
        if (StartPart == EndPart)
        {
            List<Transform> path = new List<Transform>();
            int inc = (StartIndex < EndIndex) ? 1 : -1;
            for (int i = (int)StartIndex; i != EndIndex + inc; i += inc)
            {
                switch (StartPart)
                {
                    case Parts.Head:
                        path.Add(Head[i]);
                        break;
                    case Parts.Torso:
                        path.Add(Torso[i]);
                        break;
                    case Parts.LeftArm:
                        path.Add(LeftArm[i]);
                        break;
                    case Parts.RightArm:
                        path.Add(RightArm[i]);
                        break;
                    case Parts.LeftLeg:
                        path.Add(LeftLeg[i]);
                        break;
                    case Parts.RightLeg:
                        path.Add(RightLeg[i]);
                        break;
                    default:
                        Debug.Log("In FindPath() where (StartPart == EndPart) the 'switch' statement triggered 'default'. Have you changed the enum Parts?");
                        break;
                }
            }

            return path.ToArray();
        }
        else
        {
            List<Transform> path = new List<Transform>();
            int i;

            switch (StartPart)
            {
                case Parts.Head:
                    i = (int)StartIndex;
                    path.Add(Head[i]);
                    while (i != 0)
                    {
                        --i;
                        path.Add(Head[i]);
                    }

                    path.AddRange(FindPath(Parts.Torso, 0, EndPart, EndIndex));
                    return path.ToArray();
                case Parts.Torso:
                    int goal;
                    switch (EndPart)
                    {
                        case Parts.Head:
                            goal = 0;
                            break;
                        case Parts.Torso:
                            Debug.Log("FindPath() -> (StartPart != EndPart) -> StartPart == EndPart == Parts.Torso => Something went wrong");
                            return null;
                        case Parts.LeftArm:
                            goal = 0;
                            break;
                        case Parts.RightArm:
                            goal = 0;
                            break;
                        case Parts.LeftLeg:
                            goal = Torso.Length - 1;
                            break;
                        case Parts.RightLeg:
                            goal = Torso.Length - 1;
                            break;
                        default:
                            Debug.Log("FindPath() -> (StartPart != EndPart) -> EndPart == default => Something went wrong");
                            return null;
                    }

                    int inc = (StartIndex < goal) ? 1 : -1;
                    for (i = (int)StartIndex; i != goal; i += inc)
                    {
                        path.Add(Torso[i]);
                    }

                    path.Add(Torso[i]);

                    path.AddRange(FindPath(EndPart, 0, EndPart, EndIndex));
                    return path.ToArray();
                case Parts.LeftArm:
                    i = (int)StartIndex;
                    path.Add(LeftArm[i]);
                    while (i != 0)
                    {
                        --i;
                        path.Add(LeftArm[i]);
                    }

                    path.AddRange(FindPath(Parts.Torso, 0, EndPart, EndIndex));
                    return path.ToArray();
                case Parts.RightArm:
                    i = (int)StartIndex;
                    path.Add(RightArm[i]);
                    while (i != 0)
                    {
                        --i;
                        path.Add(RightArm[i]);
                    }

                    path.AddRange(FindPath(Parts.Torso, 0, EndPart, EndIndex));
                    return path.ToArray();
                case Parts.LeftLeg:
                    i = (int)StartIndex;
                    path.Add(LeftLeg[i]);
                    while (i != 0)
                    {
                        --i;
                        path.Add(LeftLeg[i]);
                    }

                    path.AddRange(FindPath(Parts.Torso, (uint)(Torso.Length - 1), EndPart, EndIndex));
                    return path.ToArray();
                case Parts.RightLeg:
                    i = (int)StartIndex;
                    path.Add(RightLeg[i]);
                    while (i != 0)
                    {
                        --i;
                        path.Add(RightLeg[i]);
                    }

                    path.AddRange(FindPath(Parts.Torso, (uint)(Torso.Length - 1), EndPart, EndIndex));
                    return path.ToArray();
                default:
                    Debug.Log("In FindPath() where (StartPart != EndPart) the 'switch' statement triggered 'default'. Have you changed the enum Parts?");
                    break;
            }
        }

        return null;
    }
}
