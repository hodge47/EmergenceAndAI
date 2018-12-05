using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Species {

    [HideInInspector]
    public bool canGoOutInBoat = false;

    private bool canCatchFish = false;
    private bool arrivedAtHome = false;
    private float homeCoolDownTime = 5; // Seconds
    private double timeArrivedAtHome;

    // Use this for initialization
    protected override void Start()
    {
        // Base functionality
        base.Start();
        // Species Specific Start()
        speciesName = "Human";
        stayMoving = false;
        moveSpeed = 1f;
        // Move me to my home
        ChooseMyHome();
        // Create new target for fish
        CreateNewTargetForHuman("new");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canGoOutInBoat)
        {
            // Move the fish around in the pond
            Movement();
        }
    }

    protected void ChooseMyHome(){
         // Assign home
        int _xSide = random.Next(0,2);
        int _ySide = random.Next(0,2);
        float _homeX;
        float _homeY;
        // Random X in border
        if(_xSide == 0){
            _homeX = random.Next(-(int)canvasWidth/2,-(int)canvasWidth / 2 + (int)Mathf.Round(canvasWidth / 2 * 0.1f));
        } else{
            _homeX = random.Next((int)canvasWidth/2 - (int)Mathf.Round(canvasWidth / 2 * 0.1f), (int)canvasWidth / 2);
        }
        // Random Y in border
        if(_ySide == 0){
            _homeY = random.Next(-(int)canvasHeight / 2, -(int)canvasHeight / 2 + (int)Mathf.Round(canvasHeight / 2 * 0.1f));
        } else{
            _homeY = random.Next((int)canvasHeight / 2 - (int)Mathf.Round(canvasHeight / 2 * 0.1f), (int)canvasHeight / 2);
        }
        Vector2 _homeLocation = new Vector2(_homeX, _homeY);
        home = _homeLocation;
        // Put me at my home location
        thisRectTransform.anchoredPosition = home;
    }

    protected void CreateNewTargetForHuman(string _location)
    {
        if(_location == "new")
        {
            Vector2 _targetPosition = new Vector2(random.Next(-(int)EcosystemManager.radiusOfPond, (int)EcosystemManager.radiusOfPond), random.Next(-(int)EcosystemManager.radiusOfPond, (int)EcosystemManager.radiusOfPond));
            targetPosition = _targetPosition;
        }
        else if(_location == "home")
        {
            targetPosition = home;
        }
    }

    protected override void Movement()
    {
        if (thisRectTransform.anchoredPosition != targetPosition && canCatchFish == false)
        {
            thisRectTransform.anchoredPosition = Vector2.MoveTowards(thisRectTransform.anchoredPosition, targetPosition, moveSpeed);
        }
        else if(thisRectTransform.anchoredPosition == targetPosition)
        {
            canCatchFish = true;

            if (thisRectTransform.anchoredPosition == home)
            {
                canCatchFish = false;
                Debug.Log("We are at home");
                if (arrivedAtHome == false)
                {
                    arrivedAtHome = true;
                    timeArrivedAtHome = EcosystemManager.gameTime;
                }
                
                if (arrivedAtHome && EcosystemManager.gameTime - timeArrivedAtHome >= homeCoolDownTime)
                {
                    CreateNewTargetForHuman("new");
                    arrivedAtHome = false;
                }  
            }
        }
        
    }

    private void CatchFish(Collider2D _collision)
    {
        if (canCatchFish)
        {
            _collision.transform.gameObject.SetActive(false);
            canCatchFish = false;
            CreateNewTargetForHuman("home");
            Debug.Log("A fish has been caught!");
        }
    }

    public void GetInBoat()
    {
        canGoOutInBoat = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "A_Fish")
        {
            CatchFish(collision);
        }
    }
}
