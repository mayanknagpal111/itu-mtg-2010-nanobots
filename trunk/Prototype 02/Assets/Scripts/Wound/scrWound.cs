using UnityEngine;
using System.Collections;

public class scrWound : MonoBehaviour
{

    scrBodyPart body;
    int points;

    public float Size = 2f;

    public scrBodyPart Body { get { return body; } set { body = value; } }
    public int Points { get { return points; } set { points = value; } }

    scrSwarm swarm;
    bool healing;

    // Use this for initialization
    void Start()
    {
        healing = false;
    }

    // Update is called once per frame
    void Update()
    {
        scrGameStats.Instance.Health -= Size * Time.deltaTime;

        if (healing)
        {
            if (swarm.Size - swarm.Healing * Time.deltaTime <= 0f)
                healing = false;

            scrGameStats.Instance.Health += Size * Time.deltaTime;
            swarm.Size -= swarm.Healing * Time.deltaTime;
            Size -= Time.deltaTime;

            if (Size <= 0f)
            {
                Body.Heal();
                scrGameStats.Instance.Score += Points;
                scrGameStats.Instance.Health += 2f;
                
                swarm.StopWorking();
                Destroy(gameObject);
            }
        }
    }

    public void Heal(scrSwarm swarm)
    {
        this.swarm = swarm;
        healing = true;
    }
}
