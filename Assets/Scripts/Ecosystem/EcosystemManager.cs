using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class EcosystemManager : MonoBehaviour {

    // UI
    public Text gameTimeText;
    public Text fishCountText;
    public Text gameSpeedText;
    public Button startButton;
    public Button stopButton;
    public Button exitButton;
    public Button debugButton;
    public Button raiseGameSpeedButton;
    public Button lowerGameSpeedButton;
    public int allowPredatorsTime = 10;
    public int fishBreedTime = 60; // Seconds
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
    private int maxPoolFish = 100;
    private int minSpecies = 2;

    private bool letHumansGetInBoat = false;

    private bool canBreedFish = true;
    private double lastFishBreed = 0;
    private double lastTimeFishWasBred;

    private bool debugSimulation = false;

    private float currentGameSpeed = 1;
    private float lowestGameSpeed = 0.4f;
    private float highestGameSpeed = 4;

	// Use this for initialization
	void Start () {
        // Button delegates
        startButton.onClick.AddListener(delegate { StartSimulation(); });
        stopButton.onClick.AddListener(delegate { StopSimulation(); });
        exitButton.onClick.AddListener(delegate { ExitSimulation(); });
        debugButton.onClick.AddListener(delegate { DebugSimulation(); });
        raiseGameSpeedButton.onClick.AddListener(delegate { RaiseGameSpeed(); });
        lowerGameSpeedButton.onClick.AddListener(delegate { ReduceGameSpeed(); });
        // Hide exit button if WebGL
        #if UNITY_WEBGL
                exitButton.enabled = false;
        #endif
        // Initialize random
        random = new System.Random();
        // Create object pool for all needed species
        PoolAllSpecies();
	}
	
	// Update is called once per frame
	void Update () {
        // Game speed
        Time.timeScale = currentGameSpeed;
        // Time
        gameTime += Time.deltaTime;
        string _minutes = Mathf.Floor((float)gameTime / 60).ToString("00");
        string _seconds = (gameTime % 60).ToString("00");
        gameTimeText.text = _minutes + ":" + _seconds;
        // Fish count
        int _aliveFishCount = 0;
        foreach(Fish f in fish)
        {
            if(f.isAlive == true)
            {
                _aliveFishCount++;
            }
        }
        fishCountText.text = "Fish in pond: " + _aliveFishCount.ToString();
        // Let humans get in boat
        if (gameTime > allowPredatorsTime)
        {
            foreach(Human h in humans)
            {
                h.gameObject.SetActive(true);
                h.canGoOutInBoat = true;
                letHumansGetInBoat = false;
            }
            foreach(Bird b in birds)
            {
                b.gameObject.SetActive(true);
            }
        }
        // Breed fish
        BreedFish();
	}

    private void PoolAllSpecies()
    {
        // Humans
        for (int i = 0; i < maxHumans; i++)
        {
            GameObject aHuman = Instantiate(humanPrefab, GameObject.Find("Canvas").transform);
            aHuman.name = "A_Human";
            var _component = aHuman.GetComponent<Human>();
            _component.ecosystemManager = this;
            humans.Add(_component);
            aHuman.SetActive(false);
        }
        // Birds
        for (int i = 0; i < maxBirds; i++)
        {
            GameObject aBird = Instantiate(birdPrefab, GameObject.Find("Canvas").transform);
            aBird.name = "A_Bird";
            var _component = aBird.GetComponent<Bird>();
            _component.ecosystemManager = this;
            birds.Add(_component);
            aBird.SetActive(false);
        }
        // Fish
        for (int i = 0; i < maxPoolFish; i++)
        {
            GameObject aFish = Instantiate(fishPrefab, GameObject.Find("Canvas").transform);
            aFish.name = "A_Fish";
            var _component = aFish.GetComponent<Fish>();
            _component.ecosystemManager = this;
            if (i > maxFish)
            {
                _component.KillSpecies();
            }
            fish.Add(_component);
        }
    }

    private void BreedFish()
    {
        int _totalBredFish = 0;

        int _aliveCount = 0;
        foreach(Fish f in fish)
        {
            if(f.isAlive == true)
            {
                _aliveCount += 1;
            }
        }

        // Check lastBredTime is > 5
        if(gameTime - lastFishBreed > 5)
        {
            canBreedFish = true;
        }

        // Set all dead fish active 
        float _roundedGameTime = Mathf.Round((float)gameTime);
        if (_roundedGameTime % fishBreedTime == 0 && gameTime > 50)
        {
            if (canBreedFish)
            {
                // Breed every two fish
                int _breed = 0;
                for (int i = 0; i < _aliveCount; i++)
                {
                    _breed++;
                    if (_breed >= 2)
                    {
                        _breed = 0;
                        int _newGeneration = random.Next(1, 5);
                        // Check we can enable _newGeneration of fish
                        int _availableFishToResurrect = 0;
                        foreach (Fish f in fish)
                        {
                            if (!f.isAlive)
                            {
                                _availableFishToResurrect++;
                            }
                        }
                        // Resurrect new fish if can
                        if (_newGeneration < _availableFishToResurrect)
                        {
                            int _resurrectedFishCount = 0;
                            foreach (Fish f in fish)
                            {
                                if (!f.isAlive && _resurrectedFishCount < _newGeneration)
                                {
                                    f.ResurrectSpecies();
                                    _resurrectedFishCount++;
                                    _totalBredFish++;
                                }
                            }
                        }
                    }
                }
                lastFishBreed = gameTime;
                canBreedFish = false;
                Debug.Log("There were a total of " + _totalBredFish + " fish bred!");
            }
        }
    }

    private void StartSimulation()
    {
        Time.timeScale = 1;
    }

    private void StopSimulation()
    {
        Time.timeScale = 0;
    }

    private void ExitSimulation()
    {
        Application.Quit();
    }

    private void DebugSimulation()
    {
        if(debugSimulation == false)
        {
            debugSimulation = true;
            foreach(Bird b in birds)
            {
                b.debug = true;
            }
            debugButton.GetComponentInChildren<Text>().text = "Debug: <color=#00B315>True</color>";
        }
        else if(debugSimulation == true)
        {
            debugSimulation = false;
            foreach(Bird b in birds)
            {
                b.debug = false;
            }
            debugButton.GetComponentInChildren<Text>().text = "Debug: <color=#ff0000>False</color>";
        }
    }

    private void ReduceGameSpeed()
    {
        if(currentGameSpeed > lowestGameSpeed)
        {
            currentGameSpeed -= 0.2f;
            gameSpeedText.text = "Game Speed: " + currentGameSpeed.ToString();
        }
    }

    private void RaiseGameSpeed()
    {
        if (currentGameSpeed < highestGameSpeed)
        {
            currentGameSpeed += 0.2f;
            gameSpeedText.text = "Game Speed: " + currentGameSpeed.ToString();
        }
    }
}
