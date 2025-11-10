using UnityEngine;
using System.Collections.Generic;

public class PipeSpawner : MonoBehaviour
{
    [Header("Pipe Settings")]
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float pipeSpeed = 2f;
    [SerializeField] private float despawnX = -12f;
    
    [Header("Spawn Position Settings")]
    [SerializeField] private float gapSize = 2.5f;
    [SerializeField] private float minGapY = -2f;
    [SerializeField] private float maxGapY = 2f;
    [SerializeField] private float spawnX = 10f;
    
    [Header("Boundary Settings")]
    [SerializeField] private float ceilingY = 4.5f;
    [SerializeField] private float groundY = -4.5f;
    
    private List<GameObject> activePipes = new List<GameObject>();
    private float nextSpawnTime = 0f;

    void Update()
    {
        // Spawn pipes at intervals
        if (Time.time >= nextSpawnTime)
        {
            SpawnPipe();
            nextSpawnTime = Time.time + spawnInterval;
        }
        
        // Move and despawn pipes
        MovePipes();
    }

    void SpawnPipe()
    {
        // Random Y position for the CENTER of the gap
        float gapCenterY = Random.Range(minGapY, maxGapY);
        
        // Calculate top and bottom positions of the gap
        float gapTopY = gapCenterY + (gapSize / 2f);
        float gapBottomY = gapCenterY - (gapSize / 2f);
        
        // Create LOCAL position for the pipe pair
        Vector3 localSpawnPosition = new Vector3(spawnX, gapCenterY, 0f);
        Vector3 worldSpawnPosition = transform.TransformPoint(localSpawnPosition);
        
        // Instantiate the pipe pair
        GameObject newPipe = Instantiate(pipePrefab, worldSpawnPosition, Quaternion.identity);
        newPipe.transform.SetParent(transform, true);
        
        // Get the top and bottom pipe children
        Transform topPipe = newPipe.transform.Find("PipeTop");
        Transform bottomPipe = newPipe.transform.Find("PipeBottom");
        
        if (topPipe != null && bottomPipe != null)
        {
            // Calculate heights needed to reach ceiling/ground
            float topPipeHeight = ceilingY - gapTopY;
            float bottomPipeHeight = gapBottomY - groundY;
            
            // Position and scale the top pipe (extends from gap to ceiling)
            topPipe.localPosition = new Vector3(0, (gapTopY - gapCenterY) + (topPipeHeight / 2f), 0);
            topPipe.localScale = new Vector3(topPipe.localScale.x, topPipeHeight, topPipe.localScale.z);
            
            // Position and scale the bottom pipe (extends from ground to gap)
            bottomPipe.localPosition = new Vector3(0, (gapBottomY - gapCenterY) - (bottomPipeHeight / 2f), 0);
            bottomPipe.localScale = new Vector3(bottomPipe.localScale.x, bottomPipeHeight, bottomPipe.localScale.z);
        }
        
        activePipes.Add(newPipe);
    }

    void MovePipes()
    {
        for (int i = activePipes.Count - 1; i >= 0; i--)
        {
            if (activePipes[i] != null)
            {
                // Move pipe left in LOCAL space
                activePipes[i].transform.localPosition += Vector3.left * pipeSpeed * Time.deltaTime;
                
                // Despawn if off screen (check local X position)
                if (activePipes[i].transform.localPosition.x < despawnX)
                {
                    Destroy(activePipes[i]);
                    activePipes.RemoveAt(i);
                }
            }
        }
    }

    public void ClearPipes()
    {
        // Destroy all active pipes
        foreach (GameObject pipe in activePipes)
        {
            if (pipe != null)
            {
                Destroy(pipe);
            }
        }
        activePipes.Clear();
        nextSpawnTime = 0f;
    }
}
