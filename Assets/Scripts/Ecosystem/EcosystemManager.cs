using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemManager : MonoBehaviour {

    public float simulationTime = 5; // Minutes

    // Species objects
    public List<Species> humans = new List<Species>();
    public List<Species> birds = new List<Species>();
    public List<Species> fish = new List<Species>();
    // Species amounts
    private int maxHumans = 5;
    private int maxBirds = 15;
    private int maxFish = 25;
    private int minSpecies = 2;

	// Use this for initialization
	void Start () {
        // Create object pool for all needed species
        PoolAllSpecies();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PoolAllSpecies()
    {
        // Humans
        for(int i = 0; i < maxHumans; i++)
        {
            GameObject aHuman = new GameObject();
            aHuman.name = "A Human";
            aHuman.AddComponent<Species>();
            humans.Add(aHuman.GetComponent<Species>());
        }
        // Birds
        for (int i = 0; i < maxBirds; i++)
        {
            GameObject aBird = new GameObject();
            aBird.name = "A Human";
            aBird.AddComponent<Species>();
            humans.Add(aBird.GetComponent<Species>());
        }
        // Fish
        for (int i = 0; i < maxHumans; i++)
        {
            GameObject aFish = new GameObject();
            aFish.name = "A Human";
            aFish.AddComponent<Species>();
            humans.Add(aFish.GetComponent<Species>());
        }
    }
}
