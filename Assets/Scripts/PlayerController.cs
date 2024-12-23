using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;
    
    private PlayerMovement controls;
    
    private Animator animator;

    private void Awake()
    {
        controls = new PlayerMovement();
        animator = GetComponent<Animator>();    
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    

    void Start()
    {
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            transform.position += (Vector3)direction;
        }

        if (direction.x == -1 && direction.y == 0)
        {
            animator.SetInteger("Direction", 3);
        }
        else if (direction.x == 1 && direction.y == 0)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (direction.x == 0 && direction.y == 1)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (direction.x == 0 && direction.y == -1)
        {
            animator.SetInteger("Direction", 0);
        }
        
        direction.Normalize();
        animator.SetBool("isMoving", direction.magnitude > 0);
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
        {
            return false;
        }
        return true;
    }
}
