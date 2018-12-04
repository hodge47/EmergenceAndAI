using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Species {

    // Movement - fish specific
    float radiusOfPond = 200;

    // Use this for initialization
    protected override void Start()
    {
        // Base functionality
        base.Start();
        // Species Specific Start()
        speciesName = "Fish";
        stayMoving = true;
        moveSpeed = 0.75f;
        // Create new target for fish
        CreateNewTargetForFish();
    }

    // Update is called once per frame
    void FixedUpdate () {
        // Move the fish around in the pond
        Movement();
	}

    protected void CreateNewTargetForFish()
    {
        Vector2 _targetPosition = new Vector2(random.Next(-(int)radiusOfPond, (int)radiusOfPond), random.Next(-(int)radiusOfPond, (int)radiusOfPond));
        targetPosition = _targetPosition;
    }

    protected override void Movement()
    {
        if(thisRectTransform.anchoredPosition != targetPosition)
        {
            thisRectTransform.anchoredPosition = Vector2.MoveTowards(thisRectTransform.anchoredPosition, targetPosition, moveSpeed);
        } else if(thisRectTransform.anchoredPosition == targetPosition)
        {
            CreateNewTargetForFish();
        }
    }
}
