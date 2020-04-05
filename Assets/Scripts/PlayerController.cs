using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MOVE_TIMESTEP = 0.2f;
    
    Animator animator;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    private AOGameManager GM;
    private PacketManager packetManager;

    float lastSentInputTime = 0;

    void Awake()
    {
        GM = AOGameManager.Instance;

        packetManager = PacketManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (Time.time - lastSentInputTime > MOVE_TIMESTEP)
        {
            if (Input.GetKey("d") || Input.GetKey("right"))
            {
                //move
                Debug.Log("Moving");
                lastSentInputTime = Time.time;
                packetManager.WriteWalk(2);
                transform.position += new Vector3(1, 0, 0);
            }
            else if (Input.GetKey("a") || Input.GetKey("left"))
            {
                //move
                Debug.Log("Moving");
                lastSentInputTime = Time.time;
                packetManager.WriteWalk(4);
                transform.position += new Vector3(-1, 0, 0);
            }
            else if (Input.GetKey("w") || Input.GetKey("up"))
            {
                //move
                Debug.Log("Moving");
                lastSentInputTime = Time.time;
                packetManager.WriteWalk(1);
                transform.position += new Vector3(0, 1, 0);
            }
            else if (Input.GetKey("s") || Input.GetKey("down"))
            {
                //move
                Debug.Log("Moving");
                lastSentInputTime = Time.time;
                packetManager.WriteWalk(3);
                transform.position += new Vector3(0, -1, 0);
            }            
        }
    }

}
