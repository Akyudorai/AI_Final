using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject ballObject;
    public Vector2 motionLimitations = Vector2.zero;
    public float pos;
    public float paddleSpeed = 10.0f;
    public AudioSource source;
    public AudioClip release;

    private void Start()
    {
        // Initialize the player position
        pos = transform.localPosition.x;

        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        HandleMotion();
        HandleInput();
    }

    public void HandleMotion()
    {
        // Get the input being applied by moving the mouse on the horizontal axis.
        float input = Input.GetAxis("Mouse X");
        pos += input;

        // Generate a new position for the player dropper based on input multiplied by paddle speed and deltaTime
        Vector3 newPosition = transform.localPosition + new Vector3(input * paddleSpeed, 0, 0) * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, motionLimitations.x, motionLimitations.y);
        transform.localPosition = newPosition;        
        
    }

    public void HandleInput()
    {
        // If the player presses the left mouse button and there is no active ball, drop one.
        if (Input.GetKeyDown(KeyCode.Mouse0) && !ballObject.activeInHierarchy)
        {
            // We re use the same ball rather than instantiate to save on memory.
            ballObject.transform.localPosition = transform.localPosition;
            ballObject.SetActive(true);
            
            // Play SFX
            source.PlayOneShot(release, 0.5f);
        }
    }
}
