using UnityEngine;
using System.Collections;

public class scrGUI : MonoBehaviour {

    public scrSwarmController sc;
    public scrWoundController wc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 100, 30), "Score: " + scrGameStats.Instance.Score);
        GUI.Label(new Rect(5, 30, 100, 30), "Health: " + scrGameStats.Instance.Health);

        if (scrGameStats.Instance.Health <= 0f)
        {
            // Game Over
            GUI.Label(new Rect(5, 70, 100, 30), "GAME OVER!");
            sc.gameObject.SetActiveRecursively(false);
            wc.gameObject.SetActiveRecursively(false);
            //scrGameStats.Instance.Health = 0f;
        }
    }
}
