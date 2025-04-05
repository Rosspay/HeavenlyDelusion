using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;
    [SerializeField]
    private Tilemap interactebleItemsTilemap;
    [SerializeField]
    private SpriteRenderer grabbedTileImage;
    public UnityEvent MoveEvent;
    public UnityEvent BumpEvent;
    public UnityEvent PickupEvent;
    public UnityEvent FailEvent;
    

    [SerializeField] 
    private GameObject RestartMenu;
    
    private bool isGrabbed = false;
    private Vector2 prevDirection = new Vector2(0, -1);
    private Tile grabbedTile;
    
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
        controls.Main.Grabbing.performed += ctx => Grab();
    }

    private void Grab()
    {
        if (CanGrab(prevDirection))
        {
            isGrabbed = true;
            Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)prevDirection);
            grabbedTile = groundTilemap.GetTile(gridPosition) as Tile;
            groundTilemap.SetTile(gridPosition, null);
            grabbedTileImage.sprite = grabbedTile.sprite;
            PickupEvent.Invoke();
        }
        else if (CanRelease(prevDirection))
        {
            isGrabbed = false;
            Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)prevDirection);
            groundTilemap.SetTile(gridPosition, grabbedTile);
            grabbedTileImage.sprite = null;
            PickupEvent.Invoke();
        }
    }
    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            MoveEvent.Invoke();
            transform.position += (Vector3)direction;
        }
        else
        {
            BumpEvent.Invoke();
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
        
        prevDirection = direction;
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        if (!groundTilemap.HasTile(gridPosition))
        {
            FailEvent.Invoke();
        }

        if (interactebleItemsTilemap.HasTile(gridPosition))
        {
            PlayerPrefs.SetInt("Save", SceneManager.GetActiveScene().buildIndex + 1);
            Effects.FadeScreen(Color.black, 0, 1, 1, () => SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Save").ToString()));;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (collisionTilemap.HasTile(gridPosition))
        {
            return false;
        }
        return true;
    }

    private bool CanGrab(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (groundTilemap.HasTile(gridPosition) && !isGrabbed && !collisionTilemap.HasTile(gridPosition))
        {
            return true;
        }
        return false;
    }

    private bool CanRelease(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) && isGrabbed && !collisionTilemap.HasTile(gridPosition))
        {
            return true;
        }
        return false;
    }
}
