using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species : MonoBehaviour {

    // Species properties
    public string speciesName = "No Name";   // Name of the species
    protected float hunger = 0;                 // Hunger of the species
    protected float health = 100;               // Health of the species
    protected bool breed = false;               // Can breed
    protected bool stayMoving = false;          // Does the species need to stay moving?
    protected bool isActive = false;            // Is the species active on the screen?
    protected bool isAlive = true;              // Is the species dead?
    protected Vector2 home = new Vector2(0,0);

    // Species movement properties
    protected Vector2 originPosition;
    protected Vector2 targetPosition = new Vector2(0,0);
    protected float moveSpeed = 5f;

    // Random from the EcosystemManager
    protected System.Random random = EcosystemManager.random;

    // This transform properties
    protected RectTransform thisRectTransform;

    // Canvas
    protected Canvas canvas;
    protected RectTransform canvasRectTransform;
    protected float canvasWidth;
    protected float canvasHeight;

    // Use this for initialization
    protected virtual void Start () {
        // Get this transform properties
        thisRectTransform = this.GetComponent<RectTransform>();
        // Get the canvas
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        canvasWidth = canvasRectTransform.rect.width;
        canvasHeight = canvasRectTransform.rect.height;
        // Set the origin of the screen
        originPosition = new Vector2(0, 0);
        // Create an initial target position
        CreateNewTargetPosition();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void CreateNewTargetPosition()
    {
        targetPosition = new Vector2(random.Next(-Mathf.RoundToInt(canvasWidth/2), Mathf.RoundToInt(canvasWidth/2)), random.Next(-Mathf.RoundToInt(canvasHeight/2), Mathf.RoundToInt(canvasHeight/2)));
    }

    protected virtual void Movement()
    {
        thisRectTransform.transform.position = Vector2.MoveTowards(thisRectTransform.transform.position, targetPosition, moveSpeed);
    }
}
