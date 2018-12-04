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
        stayMoving = false;
        moveSpeed = 2f;
        // Create new target for fish
        CreateNewTargetForHuman();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the fish around in the pond
        Movement();
    }

    protected void CreateNewTargetForHuman()
    {
        Vector2 _targetPosition = new Vector2(random.Next(-(int)canvasWidth / 2, (int)canvasWidth / 2), random.Next(-(int)canvasHeight / 2, (int)canvasHeight / 2));
        targetPosition = _targetPosition;
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
