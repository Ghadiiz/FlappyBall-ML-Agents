using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BallAgent : Agent
{
    [Header("Agent Components")]
    private Rigidbody2D rb;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    
    [Header("Environment References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PipeSpawner pipeSpawner;
    
    [Header("Rewards")]
    [SerializeField] private float survivalReward = 0.001f;
    [SerializeField] private float passPipeReward = 1f;
    [SerializeField] private float deathPenalty = -1f;
    
    private float timeSinceLastPipe = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Called at the beginning of each episode
    public override void OnEpisodeBegin()
    {
        // Reset agent position
        transform.position = spawnPoint.position;
        
        // Reset velocity
        rb.velocity = Vector2.zero;
        
        // Clear all pipes
        if (pipeSpawner != null)
        {
            pipeSpawner.ClearPipes();
        }
        
        timeSinceLastPipe = 0f;
    }

    // Collect observations for the agent
    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent's Y position (normalized)
        sensor.AddObservation(transform.position.y / 5f);
        
        // Agent's Y velocity
        sensor.AddObservation(rb.velocity.y / 10f);
    }

    // Receive actions from the neural network
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Action 0: Jump (discrete action, 0 = don't jump, 1 = jump)
        int jumpAction = actions.DiscreteActions[0];
        
        if (jumpAction == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
        // Small survival reward to encourage staying alive
        AddReward(survivalReward);
        
        timeSinceLastPipe += Time.fixedDeltaTime;
    }

    // For manual testing with keyboard (heuristic mode)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        
        // Press Space to jump
        if (Input.GetKey(KeyCode.Space))
        {
            discreteActions[0] = 1;
        }
        else
        {
            discreteActions[0] = 0;
        }
    }

    // Collision detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If hit ground, ceiling, wall, or pipe - episode ends
        if (collision.gameObject.CompareTag("Ground") || 
            collision.gameObject.CompareTag("Ceiling") ||
            collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("Pipe"))
        {
            AddReward(deathPenalty);
            EndEpisode();
        }
    }

    // Trigger detection for passing pipes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScoreTrigger"))
        {
            AddReward(passPipeReward);
            timeSinceLastPipe = 0f;
        }
    }
}
