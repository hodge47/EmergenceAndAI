using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Species : MonoBehaviour {
    // Public properties
    public EcosystemManager ecosystemManager;
    public bool isAlive = true;                 // Is the species dead?
    // Species properties
    public string speciesName = "No Name";      // Name of the species
    protected float hunger = 0;                 // Hunger of the species
    protected float health = 100;               // Health of the species
    protected bool breed = false;               // Can breed
    protected bool stayMoving = false;          // Does the species need to stay moving?
    protected bool isActive = false;            // Is the species active on the screen?
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
        // Look at target position - used for all species
        Vector3 _dir = targetPosition - thisRectTransform.anchoredPosition;
        float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }

    // Fish only 
    public void KillSpecies()
    {
        isAlive = false;
        this.GetComponent<Image>().enabled = false;
    }

    // Fish only
    public virtual void ResurrectSpecies()
    {
        isAlive = true;
        this.GetComponent<Image>().enabled = true;
    }
}
