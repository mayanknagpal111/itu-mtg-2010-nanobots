using UnityEngine;
using System.Collections;

public class scrSwarm : MonoBehaviour {

    public enum SwarmType
    {
        Organic,
        Mechanic
    }

    // Swarm properties
    public float DefaultSize = 10f;
    public SwarmType Type;
    public Material SwarmMaterial;
    public Material SelectedSwarmMaterial;

    // Bot properties
    public float Health = 1f;
    public float Healing = 1f;
    public float Speed = 1f;
    public float SpawnRate = 1f;

    private float size = -1f;
    private bool working; // If the swarm isn't working - it will start spawning new bots
    private bool selected = false;

    public float Size {
        get { return size; }
        set { if (value >= 0f) { size = value; SizeChanged(); } } 
    }

    public bool Working { get { return working; } set { working = value; } }

    public bool Selected { get { return selected; } set { selected = value; renderer.material = selected ? SelectedSwarmMaterial : SwarmMaterial; } }

	// Use this for initialization
	void Start () {
        if (size <= 0f)
        {
            Size = DefaultSize;
            Working = false;
            Selected = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SizeChanged() {
        Debug.Log("SizeChanged() Called");
        transform.localScale = new Vector3(Size, Size, Size);
    }

    public void StopWorking()
    {
        Working = false;
    }

    public void Merge(scrSwarm other)
    {
        other.Size += Size;
        Destroy(gameObject);
    }
}
