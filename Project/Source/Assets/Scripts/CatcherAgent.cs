using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using TMPro;

public class CatcherAgent : Agent
{
    // Various Components
    public GameObject targetToCatch;
    public GameObject ground;
    private Rigidbody2D rb;
    private Vector3 startPos;
    public AudioSource source;

    // Sound Components
    public AudioClip caught, splat;
    
    // Keep track of the score
    private int score = 0;
    public float MoveSpeed = 3.0f;

    // Initialize some components
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    // Initialize the agent
    public override void OnEpisodeBegin()
    {
        // Set the position
        transform.position = startPos;

        // Reset the score
        score = 0;
        StatisticsManager.instance.ScoreDisplay.text = score.ToString();
        
        // Reset the ball
        ResetObject();
    }
   
    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect position information of the agent position and the ball position for processing
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetToCatch.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the input value from the agent and adjust velocity accordingly.
        Vector3 move = Vector3.zero;
        move.x = actions.ContinuousActions[0];
        rb.velocity = move.normalized * MoveSpeed;

        // If the ball falls to the ground, end the episode and reset.
        if (targetToCatch.transform.localPosition.y <= ground.transform.localPosition.y)
        {
            // Punish the agent
            AddReward(-1f);

            // Reset the ball
            ResetObject();

            // Play SFX
            source.PlayOneShot(splat);

            // Award a win to the player
            StatisticsManager.instance.PlayerWins++;
            StatisticsManager.instance.UpdateStatisticsDisplay();

            // Reset
            EndEpisode();
        }

        // If the agent gets 10 points in a row, it wins the game
        if (score >= 10)
        {
            // Reward the agent
            AddReward(10f);

            // Award a win to AI
            StatisticsManager.instance.AiWins++;
            StatisticsManager.instance.UpdateStatisticsDisplay();

            // Reset
            EndEpisode();            
        }
    }

    // For when we want to take control of the agent ourselves (DEBUGGING)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        continuousAction[0] = Input.GetAxis("Horizontal");
    }

    // Reset the ball
    private void ResetObject()
    {
        // Randomize the position of the ball for training
        //float x = Random.Range(-16f, 12f);
        //float y = 17f;
        //targetToCatch.transform.localPosition = new Vector3(x, y, 0);
        
        // Reset the ball so player can spawn it again
        targetToCatch.transform.localPosition = new Vector3(0, 12, 0);
        targetToCatch.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        targetToCatch.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        // If the agent catches the ball
        if (other.gameObject.CompareTag("Pellet"))
        {
            // Reward the agent
            AddReward(1f);

            // Play SFX
            source.PlayOneShot(caught);

            // Update Score
            score++;
            StatisticsManager.instance.ScoreDisplay.text = score.ToString();
            
            // Reset the ball
            ResetObject();
        }
    }
}
