using UnityEngine;
using System.Collections;

public class scrGameStats : MonoBehaviour {

    private int score = 0;
    private float health = 100f;

    public int Score { get { return score; } set { score = value; } }
    public float Health { get { return health; } set { health = value; } }

    private static scrGameStats instance;

    public static scrGameStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("scrGameStats").AddComponent<scrGameStats>();
            }

            return instance;
        }
    }

    public void OnApplicationQuit()
    {
        instance = null;
    }

	// Use this for initialization
	void Start () {
	
	}

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
