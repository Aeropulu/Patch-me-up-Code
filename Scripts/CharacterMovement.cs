using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using TMPro;
using Yarn.Unity;

[System.Serializable]
public class PositionEvent : UnityEvent<Vector3>
{
}
public class CharacterMovement : MonoBehaviour, IInputReceiver
{
    public PositionEvent OnMove;
    public PositionEvent OnStop;
    public UnityEvent OnInvalidKeyPressed;
    [SerializeField]
    private float maxSpeed = 5.0f;
    [SerializeField]
    private int SQUARE_SIZE = 2;
    [SerializeField]
    private Transform map;

    [SerializeField]
    private GameObject startingTile;

    private Dictionary<string, List<GameObject>> squares = new Dictionary<string, List<GameObject>>();
    private Vector3 targetPos;
    
    bool ismoving = false;

    private float randomRadius = 0.5f;

    private Animator animator;
    private NavMeshAgent agent;
    private GameObject currentTile;
    private GameObject previousTile;
    private GameObject walkingTile;

    private DialogueUIManager dialogueUI;

    //Fmod Events
    FMOD.Studio.EventInstance SFX_Player_Move_Wrong;

    public void KeyPressed(FabricSO fabric)
    {
        
        string k = fabric.key;
        if (ismoving) return;
        
        
        bool moveFound = false;
        GameObject nextTile = null;
        if (dialogueUI.availableFabrics.Contains(fabric))
        {
            foreach (GameObject o in squares[k])
            {

                if (!o.activeSelf)
                    continue;
                float dist = Vector3.Distance(currentTile.transform.position, o.transform.position);


                if (dist > SQUARE_SIZE * 0.5f && dist < SQUARE_SIZE * 1.2f)
                {
                    Debug.Log("moving from " + currentTile.name + " to " + o.name);
                    nextTile = o;
                    moveFound = true;
                }
            }
        }
        if (moveFound)
        {
            
            MoveTo(nextTile);
        }
        else
        {
            Debug.Log("no move found");
            OnInvalidKeyPressed.Invoke();
        }
        
    }

    private void MoveTo(GameObject tile)
    {
        if (ismoving)
            return;
        previousTile = currentTile;
        currentTile = tile;
        ismoving = true;
        
        
        targetPos = tile.transform.position;
        Vector2 random = Random.insideUnitCircle.normalized * randomRadius;
        targetPos += new Vector3(random.x, 0, random.y);
        
        OnMove.Invoke(targetPos);
        //transform.forward = (pos - transform.position).normalized;
        animator.SetTrigger("Move Order");
        
        agent.isStopped = false;
        var cutscene = tile.GetComponent<TileStartCutscene>().cutscene;
        if (cutscene != null && cutscene.gameObject.activeSelf)
            cutscene.Play();
        else
            agent.SetDestination(targetPos);
        

        tile.SendMessage("Raise");
    }

    public void WalkTo(string destination)
    {
        Debug.Log(destination);
    }

    [YarnCommand("back")]
    public void GoBack()
    {
        if (previousTile == null)
            return;
        MoveTo(previousTile);
    }

    public void SetMood(int mood)
    {
        animator.SetInteger("Mood", mood);
    }

    [YarnCommand("behappy")]
    public void BeHappy()
    {
        SetMood(1);
    }

    public void MaxAnalogValue(string k, int value)
    {
        Debug.Log(k + " " + value);
    }

    public void OnInvalidKey()
    {
        SFX_Player_Move_Wrong = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFX_Player/SFX_Player_Move_Wrong");
        SFX_Player_Move_Wrong.start();
        animator.SetTrigger("Impossible");
        //StartCoroutine(FaceTowards(Camera.main.transform.position, 0.3f));
        DoFaceTowards(Camera.main.transform.position, 0.3f);
    }

    private IEnumerator FaceTowards(Vector3 target, float time)
    {
        float startTime = Time.time;
        float endTime = startTime + time;
        float startAngle = transform.eulerAngles.y;
        Vector3 groundTarget = new Vector3(target.x, 0, target.z);
        Vector3 groundPosition = new Vector3(transform.position.x, 0, transform.position.z);
        float targetAngle = Vector3.SignedAngle(Vector3.forward, groundTarget - groundPosition,  Vector3.up);
        
        while (Time.time <= endTime)
        {
            float angle = Mathf.LerpAngle(startAngle, targetAngle, Mathf.InverseLerp(startTime, endTime, Time.time));
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            yield return null;
        }

    }

    public void DoFaceTowards(Vector3 target, float time)
    {
        
        StartCoroutine(FaceTowards(target, time));
    }

    private GameObject GetWalkingTile()
    {
        RaycastHit hit;
        GameObject result = null;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            
            result = hit.collider.transform.gameObject;
            if (result == null)
                return null;
            while (result.GetComponent<TileRaise>() == null)
            {
                if (result.transform.parent == null)
                    return null;
                result = result.transform.parent.gameObject;
            }
        }
        else
            result = walkingTile;

        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueUI = FindObjectOfType<DialogueUIManager>();
        
        foreach (Transform t in map)
        {
            GameObject o = t.gameObject;
            TileLetter tileLetter = o.GetComponent<TileLetter>();
            if (tileLetter.fabric == null)
                continue;

            string letter = tileLetter.fabric.key;

            if (letter.Length < 1) continue;

            letter = letter.Substring(0,1);
            if (!squares.ContainsKey(letter))
                squares.Add(letter, new List<GameObject>());
            squares[letter].Add(o);

            if (Vector3.Distance(t.position, transform.position) <= SQUARE_SIZE * 0.9f)
                currentTile = o;
        }
        if (startingTile != null && currentTile == null)
            currentTile = startingTile;
        walkingTile = currentTile;
        

        
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int mood = animator.GetInteger("Mood");
            animator.SetInteger("Mood", (mood + 1) % 2);
        }
        */


        if (ismoving)
        {
            animator.ResetTrigger("Move Order");
            Vector3 offset = targetPos - transform.position;
            Vector3 movement = offset.normalized * maxSpeed * Time.deltaTime;
            //if (offset.sqrMagnitude < movement.sqrMagnitude)
            if (agent.remainingDistance <= agent.stoppingDistance && agent.hasPath)
            {
                movement = offset;
                ismoving = false;

                OnStop.Invoke(targetPos);
            }
            //transform.position += movement;
            animator.SetFloat("Speed", maxSpeed);
            
        }
        else
            animator.SetFloat("Speed", 0f);

        GameObject o = GetWalkingTile();
        if (o != walkingTile && o != null)
        {
            if (walkingTile)
                walkingTile.SendMessage("Lower");
            
            walkingTile = o;
            
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
