using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcosystemManager : MonoBehaviour {

    // Static variables - sloppy, will change later
    [HideInInspector]
    public static System.Random random;

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
    private int maxFish = 25;
    private int minSpecies = 2;

    // Canvas
    private Canvas canvas;
    private RectTransform canvasRectTransform;
    private float canvasWidth;
    private float canvasHeight;

	// Use this for initialization
	void Start () {
        random = new System.Random();
        // Get the canvas
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        canvasWidth = canvasRectTransform.rect.width;
        canvasHeight = canvasRectTransform.rect.height;
        // Create object pool for all needed species
        PoolAllSpecies();
	}
	
	// Update is called once per frame
	void Update () {
		
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
        for (int i = 0; i < maxBirds; i++)
        {
            GameObject aBird = Instantiate(birdPrefab, GameObject.Find("Canvas").transform);
            aBird.name = "A_Bird";
            birds.Add(aBird.GetComponent<Bird>());
        }
        // Fish
        for (int i = 0; i < maxFish; i++)
        {
            GameObject aFish = Instantiate(fishPrefab, GameObject.Find("Canvas").transform);
            aFish.name = "A_Fish";
            fish.Add(aFish.GetComponent<Fish>());
        }
        
        // 
    }
}
