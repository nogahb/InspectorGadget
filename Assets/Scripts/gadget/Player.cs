using UnityEngine;
using Infra;
using Infra.Gameplay;
using Infra.Utils;

namespace Gadget {
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(OverlapChecker))]
/// <summary>
/// The player controller class.
/// </summary>
public class Player : MonoBehaviour {
    public int health = 1;
    public float jumpHeight = 15f;
    public float movementSpeed = 7f;
    public float gravityScale = 2f;
    public float armAngle = 45f;
    public float bulletSpeed = 15f;

    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode jumpKey;
    public KeyCode shootKey;


    public Transform armGraphic;
    public Transform handGraphic;
    public Rigidbody2D bullet;

    // Parameters used to toggle different animations
    private readonly int isAliveParam = Animator.StringToHash("Alive");
    private readonly int isJumpingParam = Animator.StringToHash("Jump");

    private Animator animator;
    private Rigidbody2D physicsComponent;
    private OverlapChecker overlapChecker;

    private bool CanJump {
        get {
            return overlapChecker.isOverlapping;
        }
    }

    /// <summary>
    /// Create the player instance
    /// </summary>
    protected void Awake() {
        animator = GetComponent<Animator>();
        physicsComponent = GetComponent<Rigidbody2D>();
        overlapChecker = GetComponent<OverlapChecker>();

        // Make the player alive
        animator.SetBool(isAliveParam, true);
    }

    /// <summary>
    /// Update player movement
    /// </summary>
    protected void Update() {
        // Change the shooting arm angle. This also effects the shot direction
        var armGraphicAngle = armGraphic.eulerAngles;
        armGraphicAngle.z = armAngle;
        armGraphic.eulerAngles = armGraphicAngle;

        // Prevents changing the gravity scale using the inspector
        physicsComponent.gravityScale = gravityScale;

        // Handle user inputs
        var movement = physicsComponent.velocity;
        if (Input.GetKeyDown(jumpKey) && CanJump) {
            movement.y = jumpHeight;
            physicsComponent.velocity = movement;
            animator.SetTrigger(isJumpingParam);
        } else if (Input.GetKey(rightKey)) {
            movement.x = movementSpeed;
            physicsComponent.velocity = movement;
        } else if (Input.GetKey(leftKey)) {
            movement.x = -movementSpeed;
            physicsComponent.velocity = movement;
        } else if (Input.GetKey(shootKey)) {
            // Shoot bullet from hand but only allow one bullet on screen at all times
            if (!bullet.gameObject.activeInHierarchy) {
                bullet.gameObject.SetActive(true);
                bullet.position = handGraphic.position;
                bullet.velocity = Vector2.right.Rotate(Mathf.Deg2Rad * armAngle) * bulletSpeed;
            }
        }
    }

    /// <summary>
    /// Checks collisions
    /// </summary>
    protected void OnCollisionEnter2D(Collision2D collision) {
        if (health <= 0) return;

        // Reached finish block
        if (collision.gameObject.CompareTag("Victory")) {
            DebugUtils.Log("Great Job!");
            return;
        }
        if (!collision.gameObject.CompareTag("Enemy")) return;
        
        // Player hit an enemy
        --health;
        if (health > 0) return;

        // Player died - prevent movement and change animation
        animator.SetBool(isAliveParam, false);
        physicsComponent.velocity = Vector2.zero;
        physicsComponent.gravityScale = 4f;
        enabled = false;
    }
}
}
