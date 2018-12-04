using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Species {

    // Predator avoidance
    private float maxPredatorDistance = 20f;
    private RaycastHit2D hit;

    // Use this for initialization
    protected override void Start()
    {
        // Base functionality
        base.Start();
        // Species Specific Start()
        speciesName = "Bird";
        stayMoving = false;
        moveSpeed = 2f;
        // Create new target for fish
        CreateNewTargetForBird();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the fish around in the pond
        Movement();
        // Predator Avoidance
        PredatorAvoidance();
    }

    protected void CreateNewTargetForBird()
    {
        Vector2 _targetPosition = new Vector2(random.Next(-(int)canvasWidth/2, (int)canvasWidth/2), random.Next(-(int)canvasHeight/2, (int)canvasHeight/2));
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
            CreateNewTargetForBird();
        }
    }

    private void PredatorAvoidance()
    {
        hit = Physics2D.Raycast(this.thisRectTransform.anchoredPosition, Vector2.up, maxPredatorDistance);
        if (hit.collider.name == "A_Human")
        {
          Debug.Log("We are too close to a human!");
        }
    } 
}
