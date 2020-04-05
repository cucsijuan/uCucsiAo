using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    AOGameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
       GameManager = AOGameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(GameManager.playerObject.transform.position.x, GameManager.playerObject.transform.position.y, transform.position.z); 
    }
}
