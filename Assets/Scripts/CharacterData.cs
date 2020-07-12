using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] SpriteRenderer headSprite;

    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator headAnimator;
    [SerializeField] Animator helmetAnimator;
    [SerializeField] Animator weaponAnimator;

    AOGameManager GM;
    PlayerController _playerController;

    private short _body = 0;
    private short _head = 0;
    private short _weapon = 0;
    private short _shield = 0;
    private short _helmet = 0;

    void Awake()
    {
        GM = AOGameManager.Instance;
        _playerController = gameObject.GetComponent<PlayerController>();

        if (_playerController == null)
        {
            Debug.LogError("Player controller script not found on Player Prefab.");
        }
    }

    public void SetupPaperdoll(short body, short head, short weapon, short shield, short helmet)
    {
        this._body = body;
        this._head = head;
        this._weapon = weapon;
        this._shield = shield;
        this._helmet = helmet;

        headSprite.sprite = GM.spriteCache.GetSprite(_head);
        
    }

    private void Update()
    {
        
    }
}
