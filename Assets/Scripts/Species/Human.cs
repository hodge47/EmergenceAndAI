using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Species {

    // Use this for initialization
    protected override void Start()
    {
        // Base functionality
        base.Start();
        // Species Specific Start()
        speciesName = "Human";
        Debug.Log(speciesName);
        stayMoving = false;
        moveSpeed = 1f;
        // Move me to my home
        ChooseMyHome();
        // Create new target for fish
        CreateNewTargetForHuman();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the fish around in the pond
        Movement();
    }

    protected void ChooseMyHome(){
         // Assign home
        int _xSide = random.Next(0,2);
        int _ySide = random.Next(0,2);
        Debug.Log(_xSide + " " + _ySide);
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
        Debug.Log(home);
    }

    protected void CreateNewTargetForHuman()
    {
        Vector2 _targetPosition = new Vector2(random.Next(-(int)canvasWidth / 2, (int)canvasWidth / 2), random.Next(-(int)canvasHeight / 2, (int)canvasHeight / 2));
        if((_targetPosition.x > 400) && (_targetPosition.x < -400) && (targetPosition.y > 400) && (_targetPosition.y < -400)){
            targetPosition = _targetPosition;
        }
    }

    protected override void Movement()
    {
        if (thisRectTransform.anchoredPosition != targetPosition)
        {
            thisRectTransform.anchoredPosition = Vector2.MoveTowards(thisRectTransform.anchoredPosition, targetPosition, moveSpeed);
        }
        else if (thisRectTransform.anchoredPosition == targetPosition)
        {
            CreateNewTargetForHuman();
        }
    }
}
