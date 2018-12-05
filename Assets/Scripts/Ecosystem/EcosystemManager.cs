using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class EcosystemManager : MonoBehaviour {

    // UI
    public Text gameTimeText;
    public int allowHumansInBoatsTime = 10;
    public int fishBreedTime = 5;
    // Static variables - sloppy, will change later
    [HideInInspector]
    public static System.Random random;
    [HideInInspector]
    public static double gameTime;
    [HideInInspector]
    public static float radiusOfPond = 500;
    // Simulation specifics
    public float simulationTime = 5; // Minutes

    // Species prefabs
    public GameObject fishPrefab;
    public GameObject birdPrefab;
    public GameObject humanPrefab;

    // Species objects
    public List<Human> humans = new List<Human>();
    public List<Bird> birds = new List<Bird>();
    public List<Fish> fish = new List<Fish>();
    // Species amounts
    private int maxHumans = 5;
    private int maxBirds = 7;
    private int maxFish = 30;
    private int minSpecies = 2;

    private bool letHumansGetInBoat = false;

    private bool canBreedFish = false;
    private double lastTimeFishWasBred;

	// Use this for initialization
	void Start () {
        // Initialize random
        random = new System.Random();
        // Create object pool for all needed species
        PoolAllSpecies();
	}
	
	// Update is called once per frame
	void Update () {
        // Time
        gameTime += Time.deltaTime;
        gameTimeText.text = gameTime.ToString("F2");
        // Let humans get in boat
        if(gameTime > allowHumansInBoatsTime)
        {
            foreach(Human h in humans)
            {
                h.canGoOutInBoat = true;
                letHumansGetInBoat = false;
            }
        }
	}

    private void PoolAllSpecies()
    {
        // Humans
        for (int i = 0; i < maxHumans; i++)
        {
            GameObject aHuman = Instantiate(humanPrefab, GameObject.Find("Canvas").transform);
            aHuman.name = "A_Human";
            humans.Add(aHuman.GetComponent<Human>());
        }
        // Birds
        //for (int i = 0; i < maxBirds; i++)
        //{
        //    GameObject aBird = Instantiate(birdPrefab, GameObject.Find("Canvas").transform);
        //    aBird.name = "A_Bird";
        //    birds.Add(aBird.GetComponent<Bird>());
        //}
        // Fish
        for (int i = 0; i < maxFish; i++)
        {
            GameObject aFish = Instantiate(fishPrefab, GameObject.Find("Canvas").transform);
            aFish.name = "A_Fish";
            fish.Add(aFish.GetComponent<Fish>());
        }
    }

    private void BreedFish()
    {
        int _aliveCount = 0;
        foreach(Fish f in fish)
        {
            if(f.enabled == true)
            {
                _aliveCount += 1;
            }
        }

        if(_aliveCount < fish.Count / 2)
        {
            if(canBreedFish == false)
            {
                canBreedFish = true;
                lastTimeFishWasBred = gameTime;
            }

            if (gameTime - lastTimeFishWasBred >= fishBreedTime && canBreedFish)
            {
                int _fishBred = 0;
                foreach(Fish f in fish)
                {
                    if(_fishBred < 3 && f.enabled == false)
                    {
                        f.enabled = true;
                        _fishBred += 1;
                        Debug.Log("A fish was bred!");
                    }
                }
                canBreedFish = false;
            }
        }
    }
}
