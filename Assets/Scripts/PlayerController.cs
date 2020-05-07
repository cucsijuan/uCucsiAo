using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MOVE_TIMESTEP = 0.15f;

    [SerializeField] GameObject bodyLayer;
    [SerializeField] GameObject weaponLayer;
    [SerializeField] GameObject headLayer;
    [SerializeField] GameObject helmetLayer;
    [SerializeField] bool isNpc = false; //TODO: Refactor this, we need to make an npc controller

    public float movementSpeed = 1f;

    Animator _bodyAnimator;
    public Animator BodyAnimator { get { return _bodyAnimator; } }
    Animator _headAnimator;
    public Animator HeadAnimator { get { return _headAnimator; } }
    Animator _helmetAnimator;
    public Animator HelmetAnimator { get { return _helmetAnimator; } }
    Animator _weaponAnimator;
    public Animator WeaponAnimator { get { return _weaponAnimator; } }

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    private AOGameManager GM;
    private PacketManager packetManager;

    private EHeading _heading;
    public EHeading Heading
    {
        get { return _heading; }
    }

    private bool _moving = false;
    public bool Moving
    {
        get { return _moving; }
    }

    private float lastSentInputTime = 0f;
    private float movementStep = 0f;

    List<KeyCode> keyMovementQueue = new List<KeyCode>();

    void Awake()
    {
        GM = AOGameManager.Instance;

        packetManager = PacketManager.Instance;

        _bodyAnimator = bodyLayer.GetComponent<Animator>();
        _weaponAnimator = weaponLayer.GetComponent<Animator>();
        _headAnimator = headLayer.GetComponent<Animator>();
        _helmetAnimator = helmetLayer.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponentInChildren<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (_moving == false && !isNpc)
        {
            CheckKeys();
        }
        
        if (_moving)
        {
            movementStep += Time.deltaTime * movementSpeed;

            Vector3 targetPos = GM.currentAccount.userPos.MapPositionToVector3();

            transform.position = Vector3.Lerp(transform.position, targetPos, movementStep);

            if (Vector3.Distance(transform.position, targetPos) <= 0.1)
            {
                movementStep = 0f;
                transform.position = targetPos;
                _moving = false;

            }
        }

        _bodyAnimator.SetLayerWeight(1, System.Convert.ToSingle(_moving));
        _bodyAnimator.SetFloat("Heading", (float)_heading);

        _weaponAnimator.SetLayerWeight(1, System.Convert.ToSingle(_moving));
        _weaponAnimator.SetFloat("Heading", (float)_heading);

        _headAnimator.SetFloat("Heading", (float)_heading);

        _helmetAnimator.SetFloat("Heading", (float)_heading);

    }

    private void CheckKeys()
    {
        AddMovementToKeysMovementPressedQueue();

        if (keyMovementQueue.Count != 0)
        {
            switch (keyMovementQueue[keyMovementQueue.Count - 1])
            {
                case KeyCode.D:
                    Debug.Log("Moving");
                    lastSentInputTime = Time.time;
                    packetManager.WriteWalk(2);
                    MoveUser(EHeading.EAST);
                    break;
                case KeyCode.A:
                    //move
                    Debug.Log("Moving");
                    lastSentInputTime = Time.time;
                    packetManager.WriteWalk(4);
                    MoveUser(EHeading.WEST);
                    break;
                case KeyCode.W:
                    //move
                    Debug.Log("Moving");
                    lastSentInputTime = Time.time;
                    packetManager.WriteWalk(1);
                    MoveUser(EHeading.NORTH);
                    break;
                case KeyCode.S:
                    //move
                    Debug.Log("Moving");
                    lastSentInputTime = Time.time;
                    packetManager.WriteWalk(3);
                    MoveUser(EHeading.SOUTH);
                    break;

            }
        }
    }

    private void AddMovementToKeysMovementPressedQueue()
    {

        if (Input.GetKey(KeyCode.W) == true)
        {
            if (!keyMovementQueue.Contains(KeyCode.W) && keyMovementQueue.Count < 4)
            {
                keyMovementQueue.Add(KeyCode.W);
            }
        }
        else
        {
            if (keyMovementQueue.Contains(KeyCode.W))
            {
                keyMovementQueue.Remove(KeyCode.W);
            }
        }

        if (Input.GetKey(KeyCode.S) == true)
        {
            if (!keyMovementQueue.Contains(KeyCode.S) && keyMovementQueue.Count < 4)
            {
                keyMovementQueue.Add(KeyCode.S);
            }
        }
        else
        {
            if (keyMovementQueue.Contains(KeyCode.S))
            {
                keyMovementQueue.Remove(KeyCode.S);
            }
        }

        if (Input.GetKey(KeyCode.A) == true)
        {
            if (!keyMovementQueue.Contains(KeyCode.A) && keyMovementQueue.Count < 4)
            {
                keyMovementQueue.Add(KeyCode.A);
            }
        }
        else
        {
            if (keyMovementQueue.Contains(KeyCode.A))
            {
                keyMovementQueue.Remove(KeyCode.A);
            }
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            if (!keyMovementQueue.Contains(KeyCode.D) && keyMovementQueue.Count < 4)
            {
                keyMovementQueue.Add(KeyCode.D);
            }
        }
        else
        {
            if (keyMovementQueue.Contains(KeyCode.D))
            {
                keyMovementQueue.Remove(KeyCode.D);
            }
        }


    }
    


    private void FixedUpdate()
    {


        
    }

    public void MoveUser(EHeading heading)
    {
        AOPosition directionVector;

        switch (heading)
        {
            case EHeading.NORTH:
                directionVector = AOPosition.up;
                break;
            case EHeading.EAST:
                directionVector = AOPosition.right;
                break;
            case EHeading.SOUTH:
                directionVector = AOPosition.down;
                break;
            case EHeading.WEST:
                directionVector = AOPosition.left;
                break;
            default:
                directionVector = AOPosition.zero;
                break;
        }

        _heading = heading;

        AOPosition posToCheck = AOPosition.ParseVector3(Vector3Int.CeilToInt(transform.position)) + directionVector;

        if (!IsValidPos(posToCheck))
        {
            return;
        }

        GM.currentAccount.userPos = posToCheck;
        _moving = true;
    }

    public void MoveChar(EHeading heading)
    {
        AOPosition directionVector;

        switch (heading)
        {
            case EHeading.NORTH:
                directionVector = AOPosition.up;
                break;
            case EHeading.EAST:
                directionVector = AOPosition.right;
                break;
            case EHeading.SOUTH:
                directionVector = AOPosition.down;
                break;
            case EHeading.WEST:
                directionVector = AOPosition.left;
                break;
            default:
                directionVector = AOPosition.zero;
                break;
        }

        AOPosition posToCheck = AOPosition.ParseVector3(Vector3Int.CeilToInt(transform.position)) + directionVector;

        if (!IsValidPos(posToCheck))
        {
            return;
        }

        Character tempChar = GM.charList[GM.currentAccount.userCharIndex];
        tempChar.pos = posToCheck;
        GM.charList[GM.currentAccount.userCharIndex] = tempChar;

        _moving = true;
    }

    private bool IsValidPos(AOPosition posToCheck)
    {
        if (posToCheck.x > AOGameManager.MAPMAX_X || posToCheck.y > AOGameManager.MAPMAX_Y || posToCheck.x < 0 || posToCheck.y < 0)
        {
            return false;
        }

        try
        {
            if (GM.mapData[posToCheck].blocked == 1)
            {
                return false;
            }
            return true;
        }
        catch (KeyNotFoundException ex)
        {
            Debug.LogError("IsValidPos: map position not found: " + posToCheck + ". Error:" + ex.Message);
            throw;
        }
    }

    private T GetChildComponentByName<T>(string name) where T : Component
    {
        foreach (T component in GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }

}
