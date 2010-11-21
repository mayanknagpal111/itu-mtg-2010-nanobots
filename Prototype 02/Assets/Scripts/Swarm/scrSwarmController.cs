using UnityEngine;
using System.Collections.Generic;

public class scrSwarmController : MonoBehaviour
{

    scrSwarm selectedSwarm;
    GameObject[] CPs;
    LineRenderer line;

    public float NearestCPTreshold = 0.5f;
    public scrPath Path;
    public GameObject OrganicPrefab;
    public GameObject MechanicPrefab;

    public float SwipeTreshold = 0.1f;

    private Vector3 swipeStart;
    private Vector3 swipeEnd;

    // Use this for initialization
    void Start()
    {
        selectedSwarm = null;
        CPs = GameObject.FindGameObjectsWithTag("ControlPoint");
        line = (LineRenderer)(this.GetComponent("LineRenderer"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Button Down
        {
            RaycastHit rayHit;

            if (selectedSwarm != null)
                selectedSwarm.Selected = false;
            selectedSwarm = null;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit)) //TODO: brug layer mask
            {
                Component s = rayHit.collider.gameObject.GetComponent("scrSwarm");
                if (s != null)
                {
                    selectedSwarm = (scrSwarm)s;
                    Debug.Log("Swarm Selected");
                    selectedSwarm.Selected = true;
                }
            }

        }

        if (Input.GetMouseButtonDown(1)) // Right Mouse Button Pressed
        {
            swipeStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            swipeStart.z = -1;
            swipeEnd = swipeStart;
        }

        if (Input.GetMouseButton(1)) // Right Mouse Button Down
        {
            swipeEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            swipeEnd.z = -1;

            line.SetPosition(0, swipeStart);
            line.SetPosition(1, swipeEnd);
        }

        if (Input.GetMouseButtonUp(1)) // Right Mouse Button Released
        {
            swipeEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            swipeEnd.z = -1;

            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);

            if (selectedSwarm != null)
            {
                if (!selectedSwarm.Working && Vector3.Distance(swipeStart, swipeEnd) < SwipeTreshold)
                {
                    scrControlPoint nearestSwarm = FindNearestCP(selectedSwarm.transform.position);
                    scrControlPoint nearestMouse = FindNearestCP(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    List<Transform> path = new List<Transform>();
                    path.Add(selectedSwarm.transform);
                    Debug.Log(nearestSwarm.Part.ToString() + "(" + nearestSwarm.Index.ToString() + "), " + nearestMouse.Part.ToString() + "(" + nearestMouse.Index.ToString() + ")");
                    path.AddRange(Path.FindPath(nearestSwarm.Part, (uint)nearestSwarm.Index, nearestMouse.Part, (uint)nearestMouse.Index));
                    //path.Add(new Transform(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                    scrSwarm other;
                    scrWound wound;
                    if (ShouldMerge(out other))
                    {
                        iTween.MoveTo(selectedSwarm.gameObject, iTween.Hash("path", path.ToArray(), "speed", selectedSwarm.Speed, "easetype", iTween.EaseType.easeInOutCubic, "looptype", iTween.LoopType.none, "oncomplete", "Merge", "oncompletetarget", selectedSwarm.gameObject, "oncompleteparams", other));
                    }
                    else if (GonnaHeal(out wound))
                    {
                        iTween.MoveTo(selectedSwarm.gameObject, iTween.Hash("path", path.ToArray(), "speed", selectedSwarm.Speed, "easetype", iTween.EaseType.easeInOutCubic, "looptype", iTween.LoopType.none, "oncomplete", "Heal", "oncompletetarget", wound.gameObject, "oncompleteparams", selectedSwarm));
                    }
                    else
                    {
                        iTween.MoveTo(selectedSwarm.gameObject, iTween.Hash("path", path.ToArray(), "speed", selectedSwarm.Speed, "easetype", iTween.EaseType.easeInOutCubic, "looptype", iTween.LoopType.none, "oncomplete", "StopWorking", "oncompletetarget", selectedSwarm.gameObject));
                    } 
                    selectedSwarm.Working = true;
                }
                else
                {
                    // Swipe
                    if (LineCircleIntersection(new Vector2(selectedSwarm.transform.position.x, selectedSwarm.transform.position.y),
                        selectedSwarm.Size * 0.5f,
                        new Vector2(swipeStart.x, swipeStart.y),
                        new Vector2(swipeEnd.x, swipeEnd.y)))
                    {
                        Debug.Log("Line collides with circle");
                        
                        Vector3 p1, p2;
                        float radius = selectedSwarm.Size * 0.5f;
                        GetLineCircleIntersections(radius,
                            swipeStart - selectedSwarm.transform.position,
                            swipeEnd - selectedSwarm.transform.position,
                            out p1, out p2);

                        //split swarm in two using (x1, y1) & (x2, y2) 
                        float b = Mathf.Sqrt(Mathf.Pow((p2.x - p1.x), 2) + Mathf.Pow((p2.y - p1.y), 2));
                        float h = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(b / 2f, 2));
                        float theta = 2 * Mathf.Asin(b / selectedSwarm.Size);
                        float A = 0.5f * Mathf.Pow(radius, 2) * theta - 0.5f * b * h;
                        float cA = Mathf.PI * Mathf.Pow(radius, 2);
                        float ratio = A / cA;

                        //Change the size of current selected swarm an instantiate an additional swarm
                        scrSwarm newSwarm;

                        switch (selectedSwarm.Type)
                        {
                            case scrSwarm.SwarmType.Organic:
                                newSwarm = (scrSwarm)((GameObject)Instantiate(OrganicPrefab, selectedSwarm.transform.position, selectedSwarm.transform.rotation)).GetComponent("scrSwarm");
                                break;
                            case scrSwarm.SwarmType.Mechanic:
                                newSwarm = (scrSwarm)((GameObject)Instantiate(MechanicPrefab, selectedSwarm.transform.position, selectedSwarm.transform.rotation)).GetComponent("scrSwarm");
                                break;
                            default:
                                Debug.Log("Update() Error - Wrong Swarm Type");
                                return;
                        }

                        newSwarm.transform.parent = transform;
                        newSwarm.Size = selectedSwarm.Size * ratio;
                        selectedSwarm.Size = selectedSwarm.Size - newSwarm.Size;
                        Debug.Log(newSwarm.Size);
                        Debug.Log(selectedSwarm.Size);
                        newSwarm.Working = false;
                        selectedSwarm.Working = false;
                        // TODO: Better solution form seperating swarms
                        iTween.MoveAdd(newSwarm.gameObject, new Vector3(selectedSwarm.Size / 2f, selectedSwarm.Size / 2f, 0f), 0.1f);
                        iTween.MoveAdd(selectedSwarm.gameObject, new Vector3(-newSwarm.Size / 2f, -newSwarm.Size / 2f, 0f), 0.1f);
                    }
                    else
                        Debug.Log("Line DOESN'T collides with circle");
                }
            }

        }
    }

    scrControlPoint FindNearestCP(Vector3 point)
    {
        float minDist = Mathf.Infinity;
        GameObject currentObj = CPs[0];

        foreach (GameObject CP in CPs)
        {
            float dist = Vector3.Distance(CP.transform.position, point);

            if (dist < minDist)
            {
                minDist = dist;
                currentObj = CP;
            }
        }

        return (scrControlPoint)(currentObj.GetComponent("scrControlPoint"));
    }

    //http://www.gamedev.net/community/forums/topic.asp?topic_id=304578
    //http://doswa.com/blog/2009/07/13/circle-segment-intersectioncollision/
    bool LineCircleIntersection(Vector2 center, float radius, Vector2 p1, Vector2 p2)
    {
        //Debug.Log("LC Int Check Start");
        //Debug.Log(center);
        //Debug.Log(radius);
        //Debug.Log(p1);
        //Debug.Log(p2);
        //Debug.Log("LC Int Check End");
        Vector2 dir = p2 - p1;
        Vector2 diff = center - p1;
        float t = Vector2.Dot(diff, dir) / Vector2.Dot(dir, dir);
        //float t = diff.Dot(dir) / dir.Dot(dir);
        if (t < 0.0f)
            t = 0.0f;
        if (t > 1.0f)
            t = 1.0f;
        Vector2 closest = p1 + t * dir;
        Vector2 d = center - closest;
        float distsqr = Vector2.Dot(d, d);
        // float distsqr = d.Dot(d);
        return distsqr <= radius * radius;
    }

    // http://mathworld.wolfram.com/Circle-LineIntersection.html
    void GetLineCircleIntersections(float radius, Vector3 start, Vector3 end, out Vector3 p1, out Vector3 p2)
    {
        p1 = Vector3.zero;
        p2 = Vector3.zero;

        float dx, dy, dr, D;
        dx = end.x - start.x;
        dy = end.y - start.y;
        dr = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));
        D = start.x * end.y - end.x * start.y;

        float discrim = Mathf.Pow(radius, 2) * Mathf.Pow(dr, 2) - Mathf.Pow(D, 2);

        float Ddy = D * dy;
        float sgndx = (dy < 0f) ? -1f : 1f;
        sgndx *= dx;
        float dr2 = Mathf.Pow(dr, 2);
        float nDdx = -D * dx;
        float ady = Mathf.Abs(dy);

        p1.x = (Ddy + sgndx * Mathf.Sqrt(discrim)) / dr2;
        p2.x = (Ddy - sgndx * Mathf.Sqrt(discrim)) / dr2;
        p1.y = (nDdx + ady * Mathf.Sqrt(discrim)) / dr2;
        p2.y = (nDdx - ady * Mathf.Sqrt(discrim)) / dr2;
    }

    bool ShouldMerge(out scrSwarm other)
    {
        RaycastHit rayHit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit)) //TODO: brug layer mask
        {
            scrSwarm s = (scrSwarm)(rayHit.collider.gameObject.GetComponent("scrSwarm"));
            if (s != null && s.Type == selectedSwarm.Type && s != selectedSwarm)
            {
                other = s;
                return true;
            }
        }

        other = null;
        return false;
    }

    bool GonnaHeal(out scrWound wound)
    {
        RaycastHit rayHit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit)) //TODO: brug layer mask
        {
            scrWound s = (scrWound)(rayHit.collider.gameObject.GetComponent("scrWound"));
            if (s != null)
            {
                wound = s;
                return true;
            }
        }

        wound = null;
        return false;
    }
}
