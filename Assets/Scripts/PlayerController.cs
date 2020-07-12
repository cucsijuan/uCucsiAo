using System;
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

        _heading = EHeading.SOUTH;

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

        //TODO: keeping the character data just for legacy reasons, but we have to keep that information in the player/npc object
        Character tempChar = GM.charList[GM.currentAccount.userCharIndex];
        tempChar.characterData.pos = posToCheck;
        GM.charList[GM.currentAccount.userCharIndex] = tempChar;

        _moving = true;
    }

    public void UpdateCharPaperdoll(SCharacterData charInfo)
    {
        if (charInfo.body == 0)
        {
            bodyLayer.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            UpdateBody(charInfo.body);
        }

        if (charInfo.head == 0)
        {
            headLayer.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            UpdateHead(charInfo.head);
        }
       
        if (charInfo.weapon == 0)
        {
            weaponLayer.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            UpdateWeapon(charInfo.weapon);
        }

        if (charInfo.helmet == 0)
        {
            helmetLayer.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            UpdateHelmet(charInfo.helmet);
        }


    }

    private void UpdateWeapon(int weapon)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(WeaponAnimator.runtimeAnimatorController);
        var clips = aoc.animationClips;

        var idleAnims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < GM.bodyData[weapon].Bodies.Length; i++)
        {
            AnimationClip tempAnim = GM.AnimCache.GetAnim(GM.weaponAnimData[weapon].WeaponAnims[i].grhIndex);

            AnimationClip tempIdleAnim = GM.AnimCache.GetIdleAnim(GM.weaponAnimData[weapon].WeaponAnims[i].grhIndex);

            if (tempAnim == null)
            {
                Debug.LogError("Animation: " + GM.weaponAnimData[weapon].WeaponAnims[i] + " does not found while loading weapon " + weapon);
                return;
            }

            if (tempIdleAnim == null)
            {
                Debug.LogError("Animation: IDLE_" + GM.weaponAnimData[weapon].WeaponAnims[i] + " does not found while loading weapon " + weapon);
                return;
            }

            idleAnims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
        }

        idleAnims.AddRange(anims);
        aoc.ApplyOverrides(idleAnims);

        WeaponAnimator.runtimeAnimatorController = aoc;
    }

    private void UpdateHelmet(int helmet)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(HelmetAnimator.runtimeAnimatorController);
        var clips = aoc.animationClips;

        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < GM.headData[helmet].Heads.Length; i++)
        {
            AnimationClip tempAnim = GM.AnimCache.GetHelmetAnim(GM.helmetAnimData[helmet].Heads[i].grhIndex);

            if (tempAnim == null)
            {
                Debug.LogError("Animation: " + GM.helmetAnimData[helmet].Heads[i] + " does not found while loading helmet " + helmet);
                return;
            }

            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
        }

        aoc.ApplyOverrides(anims);

        HelmetAnimator.runtimeAnimatorController = aoc;
    }

    private void UpdateHead(int head)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(HeadAnimator.runtimeAnimatorController);
        var clips = aoc.animationClips;

        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < GM.headData[head].Heads.Length; i++)
        {
            AnimationClip tempAnim = GM.AnimCache.GetHeadAnim(GM.headData[head].Heads[i].grhIndex);

            if (tempAnim == null)
            {
                Debug.LogError("Animation: " + GM.headData[head].Heads[i] + " does not found while loading head " + head);
                return;
            }

            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
        }

        aoc.ApplyOverrides(anims);

        HeadAnimator.runtimeAnimatorController = aoc;
    }

    private void UpdateBody(int body)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(BodyAnimator.runtimeAnimatorController);
        var clips = aoc.animationClips;

        var idleAnims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < GM.bodyData[body].Bodies.Length; i++)
        {
            AnimationClip tempAnim = GM.AnimCache.GetAnim(GM.bodyData[body].Bodies[i].grhIndex);

            AnimationClip tempIdleAnim = GM.AnimCache.GetIdleAnim(GM.bodyData[body].Bodies[i].grhIndex);

            if (tempAnim == null)
            {
                Debug.LogError("Animation: " + GM.bodyData[body].Bodies[i] + " does not found while loading body " + body);
                return;
            }

            if (tempIdleAnim == null)
            {
                Debug.LogError("Animation: IDLE_" + GM.bodyData[body].Bodies[i] + " does not found while loading body " + body);
                return;
            }

            idleAnims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[i], tempAnim));
        }

        idleAnims.AddRange(anims);
        aoc.ApplyOverrides(idleAnims);

        BodyAnimator.runtimeAnimatorController = aoc;
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
