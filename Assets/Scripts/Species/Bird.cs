using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Species {

    public bool canCatchFish = false;

    public bool debug = false;

    private double lastCatchTime = 0;
    private float catchCoolDownTime = 20; // Seconds

    private bool hasTarget = false;
    private GameObject preyTarget = null;
    private Vector2 seekOffset;
    private Vector2 startScale;
    private Vector2 swoopScale = new Vector2(75f, 75f);

    private LineRenderer lineRenderer;

    // Use this for initialization
    protected override void Start()
    {
        // Base functionality
        base.Start();
        // Species Specific Start()
        speciesName = "Bird";
        stayMoving = false;
        moveSpeed = 1f;
        canCatchFish = false;
        // Move me to a random position on the screen
        thisRectTransform.anchoredPosition = new Vector2(random.Next(-(int)canvasWidth, (int)canvasWidth), random.Next(-(int)canvasHeight, (int)canvasHeight));
        // Get start scale
        startScale = new Vector2(thisRectTransform.rect.width, thisRectTransform.rect.height);
        // Create new target for fish
        CreateNewTargetForBird();
        // Seek offset
        CreateNewSeekOffset();
        // Set up line renderer for debugging
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.sortingOrder = 10;
        lineRenderer.enabled = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Seek out prey
        SeekPrey();
        // Move the fish around in the pond
        Movement();
        // Check if this bird can catch a fish
        CheckCanCatchFish();
        // Debug
        DebugSpecies();
    }

    protected void CreateNewTargetForBird()
    {
        Vector2 _targetPosition = new Vector2(random.Next(-(int)canvasWidth/2, (int)canvasWidth/2), random.Next(-(int)canvasHeight/2, (int)canvasHeight/2));
        targetPosition = _targetPosition;
    }

    protected override void Movement()
    {
        base.Movement();

        if (thisRectTransform.anchoredPosition != targetPosition)
        {
            thisRectTransform.anchoredPosition = Vector2.MoveTowards(thisRectTransform.anchoredPosition, targetPosition, moveSpeed);
        }
        else if (thisRectTransform.anchoredPosition == targetPosition)
        {
            CreateNewTargetForBird();
        }
    }

    private void SeekPrey()
    {
        if (!hasTarget)
        {
            preyTarget = null;
            float _leastDistance = 999999f;
            foreach(Fish f in ecosystemManager.fish)
            {
                if (f.isAlive)
                {
                    var _distance = Vector2.Distance(thisRectTransform.anchoredPosition, f.GetComponent<RectTransform>().anchoredPosition);
                    if (_distance < _leastDistance)
                    {
                        _leastDistance = _distance;
                        preyTarget = f.gameObject;
                    }
                }
            }
            // Create new seek offset
            CreateNewSeekOffset();
            hasTarget = true;
        }
        if(preyTarget != null && hasTarget)
        {
            // Check to make sure prey is still alive
            if (!preyTarget.GetComponent<Fish>().isAlive)
            {
                preyTarget = null;
                hasTarget = false;
            }
            else if (canCatchFish)
            {
                targetPosition = preyTarget.GetComponent<RectTransform>().anchoredPosition;
            }
            else if(!canCatchFish)
            {
                targetPosition = preyTarget.GetComponent<RectTransform>().anchoredPosition + seekOffset;
            }
        }
        else if(preyTarget == null)
        {
            CreateNewTargetForBird();
        }
    }

    private void CheckCanCatchFish()
    {   
        // canCatchFish check
        if (canCatchFish == false && EcosystemManager.gameTime - lastCatchTime >= catchCoolDownTime)
        {
            canCatchFish = true;
        }
    }

    private void CatchFish(Collider2D _collision)
    {
        if (canCatchFish)
        {
            _collision.GetComponent<Fish>().KillSpecies();
            canCatchFish = false;
            lastCatchTime = EcosystemManager.gameTime;
            hasTarget = false;
        }
    }

    private void CreateNewSeekOffset()
    {
        Vector2 _newSeekOffset = new Vector2();
        int _x = random.Next(0, 2);
        int _y = random.Next(0, 2);
        // X seek offset 
        if(_x == 0)
        {
            _newSeekOffset.x = -100;
        } else if(_x == 1)
        {
            _newSeekOffset.x = 100;
        }
        // Y seek offset
        if (_y == 0)
        {
            _newSeekOffset.x = -100;
        }
        else if (_y == 1)
        {
            _newSeekOffset.x = 100;
        }

        seekOffset = _newSeekOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == preyTarget)
        {
            CatchFish(collision);
        }
    }

    private void DebugSpecies()
    {
        if (debug)
        {
            if(preyTarget != null)
            {
                // Draw line from anchor point to prey anchor point
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, thisRectTransform.position);
                lineRenderer.SetPosition(1, preyTarget.transform.position);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
        else if (!debug)
        {
            lineRenderer.enabled = false;
        }
    }
}
