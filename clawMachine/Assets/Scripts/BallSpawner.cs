using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour 
{
    private ClawMachine machine;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Material plainMat;
    [SerializeField] private Material prizeMat;
    [SerializeField] private GameObject ballBucket;
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform rightSpawn;
    [SerializeField] private bool usePrizeBallTexture;
    public bool useOdds = false;
    public float odds = 0.075f;
    [System.NonSerialized] public int attempt = 0;
    public int maxAttempts = 3;
    private bool spawnRight = false;
    [SerializeField] private int totalPrizes = 1;
    private int prizesOut = 0;
    private int prizesWon = 0;
    private float prizeChance = 0.1f;
    private float spawnShootPower = 1f;

    // Rapid Spawn
    private bool massSpawn = false;
    private int amountToSpawn;
    private float spawnInterval = 0.2f;
    private float timer = 0;

    private void Awake()
    {
        machine = GetComponent<ClawMachine>();
    }
	
	private void Update () 
	{
        if (massSpawn)
        {
            if (amountToSpawn > 0)
            {
                timer += Time.deltaTime;
                if (timer >= spawnInterval)
                {
                    if (totalPrizes - prizesOut == amountToSpawn)
                    {
                        Spawn(true);
                    }
                    else
                    {
                        Spawn();
                    }
                    timer = 0;
                    amountToSpawn--;
                }
            }
            else
            {
                massSpawn = false;
                machine.SetState(ClawMachine.GameState.Playing);
            }
        }
	}

    public void Spawn(int amount)
    {
        massSpawn = true;
        amountToSpawn = amount;
    }

    public void Spawn(bool prizeBall = false)
    {
        Transform spawnPoint = (!spawnRight) ? (leftSpawn) : (rightSpawn);
        spawnRight = !spawnRight;
        GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity, ballBucket.transform);

        if (!useOdds)
        {
            float roll = ((prizesOut - prizesWon) == 0) ? (0.0f) : (Random.Range(0.0f, 1.0f));
            if ((roll < prizeChance && prizesOut < totalPrizes) || prizeBall)
            {
                newBall.GetComponent<Ball>().ballType = Ball.eBallType.prize;
                if (usePrizeBallTexture)
                {
                    newBall.GetComponentInChildren<MeshRenderer>().material = prizeMat;
                }
                prizesOut++;
            }
        }
        else
        {
            newBall.GetComponent<Ball>().ballType = Ball.eBallType.plain;
        }
        newBall.GetComponent<Rigidbody>().AddForce(Vector3.down * spawnShootPower, ForceMode.VelocityChange);
    }

    public void PrizeWon()
    {
        prizesWon++;
    }

    public int GetPrizeTotal()
    {
        return totalPrizes;
    }

    public int GetPrizesWon()
    {
        return prizesWon;
    }
}
