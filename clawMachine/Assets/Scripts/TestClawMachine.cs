using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestClawMachine : MonoBehaviour
{
    private enum eSteps { Idle = 0, XMove = 1, ZMove = 2, ClawDown = 3, ClawUp = 4, CenterX = 5, ResetZ = 6, LowerToDrop = 7, ResetClaw = 8, ResetX = 9 }

    [Header("Grabbing")]
    [SerializeField] private BetterBtn clawBtn;
    [SerializeField] private GameObject xTrack;
    [SerializeField] private GameObject clawParent;
    [SerializeField] private GameObject claw;
    [SerializeField] private Transform holdPoint;
    private GameObject currentObject;
    private float speed = 0.5f;
    private float plungeMultiplier = 3;
    private Vector3 currentDestination;
    [SerializeField] private GameObject endPoint;
    private Vector3 bounds = new Vector3(0.45f, 1.1f, 0.45f); // test Machine Bounds = private Vector3 bounds = new Vector3(0.45f, 1.1f, 0.45f);
    private eSteps step = 0;
    private bool holding = false;
    private bool automating = false;
    private bool success = false;
    private GameObject target;
    private List<System.Action> currentCallbacks;
    private float explodeStrength = 50;
    private float explodeRadius = 5;

    [Header("Spawning")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Material plainMat;
    [SerializeField] private Material prizeMat;
    [SerializeField] private GameObject ballBucket;
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform rightSpawn;
    private bool spawnRight = false;
    private int totalPrizes = 20;
    private int prizesOut = 1;
    private int prizesWon = 0;
    private float prizeChance = 0.05f;

    [Header("Animation")]
    [SerializeField] private Animator clawAnim;
    [SerializeField] private GameObject rope;
    private Vector3 baseRopeScale;
    private float ropeLength;



    private void Awake()
    {
        clawBtn.onClick += OnClawButtonDown;
        clawBtn.onRelease += OnClawButtonUp;
        currentCallbacks = new List<System.Action>();
        baseRopeScale = rope.transform.localScale;
        ropeLength = Vector3.Distance(rope.transform.position, claw.transform.position);
    }

    void Update()
    {
        if (holding || automating)
        {
            ClawBehaviour();
        }
    }

    public void OnClawButtonDown()
    {
        if (!automating)
        {
            holding = true;
            NextStep();
        }
    }

    public void OnClawButtonUp()
    {
        if (!automating)
        {
            holding = false;
            FireCallbacks();
        }
    }

    private void ClawBehaviour()
    {
        if (step != eSteps.ClawDown && step != eSteps.ClawUp)
        {
            currentObject.transform.localPosition = Vector3.MoveTowards(currentObject.transform.localPosition, currentDestination, Time.deltaTime * speed);
        }
        else
        {
            currentObject.transform.localPosition = Vector3.MoveTowards(currentObject.transform.localPosition, currentDestination, Time.deltaTime * (speed * plungeMultiplier));
        }

        RopeStretch();

        if (currentObject.transform.localPosition == currentDestination)
        {
            FireCallbacks();
        }
    }

    private void RopeStretch()
    {
        float length = Vector3.Distance(rope.transform.position, claw.transform.position) - ropeLength;
        rope.transform.localScale = new Vector3(baseRopeScale.x, baseRopeScale.y, baseRopeScale.z + (baseRopeScale.z * length));
    }

    private void AquireTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(clawParent.transform.position, Vector3.down, out hit, LayerMask.NameToLayer("Ball")))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                target = hit.collider.gameObject;
            }
        }
    }

    private void GrabBall()
    {
        if (target != null)
        {
            target.transform.parent = holdPoint;
            target.transform.position = holdPoint.position;
            Explode(target);
            target.GetComponent<Rigidbody>().isKinematic = true;
            target.GetComponent<Ball>().grabbed = true;
            if (target.GetComponent<Ball>().ballType == Ball.eBallType.prize)
            {
                prizesWon++;
            }
            success = true;
        }
    }

    private void DropBall()
    {
        if (target != null)
        {
            target.transform.parent = null;
            target.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OpenClaw()
    {
        clawAnim.SetBool("open", true);
    }

    private void CloseClaw()
    {
        clawAnim.SetBool("open", false);
    }

    private void ClawColliderOff()
    {
        Collider[] cols = claw.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.isTrigger = false;
        }
    }

    private void ClawColliderOn()
    {
        Collider[] cols = claw.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.isTrigger = true;
        }
    }

    private void BeginAutomate()
    {
        automating = true;
        holding = false;
    }

    private void SetStep(eSteps _step)
    {
        switch (step)
        {
            case eSteps.XMove:
                currentObject = clawParent;
                currentDestination = new Vector3(bounds.x, clawParent.transform.localPosition.y, clawParent.transform.localPosition.z);
                break;
            case eSteps.ZMove:
                currentObject = xTrack;
                currentDestination = new Vector3(xTrack.transform.localPosition.x, xTrack.transform.localPosition.y, bounds.z);
                currentCallbacks.Add(AquireTarget);
                currentCallbacks.Add(OpenClaw);
                currentCallbacks.Add(BeginAutomate);
                currentCallbacks.Add(NextStep);
                break;
            case eSteps.ClawDown:
                currentObject = claw;
                currentDestination = new Vector3(0, claw.transform.localPosition.y - bounds.y, 0);
                currentCallbacks.Add(ClawColliderOff);
                currentCallbacks.Add(GrabBall);
                currentCallbacks.Add(CloseClaw);
                currentCallbacks.Add(NextStep);
                break;
            case eSteps.ClawUp:
                currentObject = claw;
                currentDestination = new Vector3(0, 0, 0);
                currentCallbacks.Add(NextStep);
                break;
            case eSteps.CenterX:
                currentObject = clawParent;
                currentDestination = new Vector3(0, clawParent.transform.localPosition.y, clawParent.transform.localPosition.z);
                currentCallbacks.Add(NextStep);
                break;
            case eSteps.ResetZ:
                currentObject = xTrack;
                currentDestination = new Vector3(xTrack.transform.localPosition.x, xTrack.transform.localPosition.y, -bounds.z);
                currentCallbacks.Add(NextStep);
                break;
            case eSteps.LowerToDrop:
                currentObject = claw;
                currentDestination = new Vector3(0, claw.transform.InverseTransformPoint(endPoint.transform.TransformPoint(-endPoint.transform.position)).y, 0);
                currentCallbacks.Add(OpenClaw);
                currentCallbacks.Add(DropBall);
                currentCallbacks.Add(NextStep);
                break;
            case eSteps.ResetClaw:
                currentObject = claw;
                currentDestination = new Vector3(0, 0, 0);
                currentCallbacks.Add(NextStep);
                break;
            case eSteps.ResetX:
                currentObject = clawParent;
                currentDestination = new Vector3(-bounds.x, clawParent.transform.localPosition.y, clawParent.transform.localPosition.z);
                currentCallbacks.Add(CloseClaw);
                currentCallbacks.Add(Reset);
                break;
        }
    }

    private void NextStep()
    {
        SetStep(step++);
    }

    private void FireCallbacks()
    {
        if (currentCallbacks.Count > 0)
        {
            List<System.Action> actions = currentCallbacks;
            currentCallbacks = new List<System.Action>();
            foreach (System.Action cb in actions)
            {
                cb.Invoke();
            }
        }
    }

    private void Reset()
    {
        automating = false;
        step = 0;
        target = null;
        currentObject = null;
        currentCallbacks = new List<System.Action>();
        currentDestination = default(Vector3);
        ClawColliderOn();
        if (success)
        {
            Spawn();
        }
        success = false;
    }

    private void Explode(GameObject _gameObject)
    {
        Collider[] hits = Physics.OverlapSphere(_gameObject.transform.position, explodeRadius, ~LayerMask.NameToLayer("Ball"));
        foreach (Collider col in hits)
        {
            if (col.gameObject != _gameObject)
            {
                col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explodeStrength, _gameObject.transform.position, explodeRadius, explodeStrength * 0.25f, ForceMode.Impulse);
            }
        }
    }

    // Spawn Logic
    private void Spawn()
    {
        Transform spawnPoint = (!spawnRight) ? (leftSpawn) : (rightSpawn);
        spawnRight = !spawnRight;
        GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity, ballBucket.transform);

        float roll = ((prizesOut - prizesWon) == 0) ? (0.0f) : (Random.Range(0.0f, 1.0f));
        if (roll < prizeChance && prizesOut < totalPrizes)
        {
            newBall.GetComponent<Ball>().ballType = Ball.eBallType.prize;
            newBall.GetComponentInChildren<MeshRenderer>().material = prizeMat;
            prizesOut++;
        }
        else
        {
            newBall.GetComponent<Ball>().ballType = Ball.eBallType.plain;
        }
    }

}
