using UnityEngine;
using Infra;
using Infra.Gameplay;
using Infra.Utils;

namespace Gadget {
// This is the C# syntax for adding an Attribute to something.
// Here we make sure that this script requires that its game object will also
// contain an Animator component and a RigidBody2D component. This allows us to
// safely assume these components exist, so we don't have to check the result
// of GetComponent<Animator>() or GetComponent<Rigidbody2D>().
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(OverlapChecker))]
/// <summary>
/// The player controller class.
/// All custom classes that we want to add to a game object must derive from
/// MonoBehaviour. This allows us to see it in the inspector and provides many
/// useful methods to manipulate the component and the game object it lives on.
/// </summary>
public class Player : MonoBehaviour {
    public int health = 1;
    public float jumpHeight = 15f;
    public float movementSpeed = 7f;
    public float gravityScale = 2f;
    public float armAngle = 45f;
    public float bulletSpeed = 15f;

    // KeyCode is an enum of all the keyboard keys that Unity knows to handle.
    // Since it is an enum, the inspector shows it as a drop down menu. Very
    // convenient!

    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode jumpKey;
    public KeyCode shootKey;

    // These parameters' types are Unity components, so in the inspector they
    // are shown as a field that we can drag a suitable reference to it.

    public Transform armGraphic;
    public Transform handGraphic;
    public Rigidbody2D bullet;

    private readonly int isAliveParam = Animator.StringToHash("Alive");
    private readonly int isJumpingParam = Animator.StringToHash("Jump");

    private Animator animator;
    private Rigidbody2D physicsComponent;
    private OverlapChecker overlapChecker;

    /// <summary>
    /// This is a C# property. If in C++ or other languages you'll define private
    /// members and have a simple getter or setter methods to them, in C# you'll
    /// make them a property.
    /// Instead of writing:
    /// private int number;
    /// public int GetNumber() { return number; }
    /// public void SetNumber(int value) { number = value; }
    /// You can simply write:
    /// public int Number { get; set; }
    /// It is also possible to allow only getting a value or settings a value and
    /// adding some code to the get or set operation like you can see in this
    /// property.
    /// Note that if getting a property requires heavy calculation, it is nicer
    /// to define a GetX or CalculateX or GenerateX instead of defining a property.
    /// This will let the user know that this process is costly.
    /// </summary>
    private bool CanJump {
        get {
            return overlapChecker.isOverlapping;
        }
    }

    /// <summary>
    /// Awake is a Unity built-in message.
    /// Unity calls it when the object is loaded into the scene. Think of it like
    /// a constructor.
    /// </summary>
    protected void Awake() {
        animator = GetComponent<Animator>();
        physicsComponent = GetComponent<Rigidbody2D>();
        overlapChecker = GetComponent<OverlapChecker>();

        // Make the player alive
        animator.SetBool(isAliveParam, true);
    }

    /// <summary>
    /// Update is a Unity built-in message.
    /// Unity calls it every frame - every time the screen renders the game view.
    /// </summary>
    protected void Update() {
        var armGraphicAngle = armGraphic.eulerAngles;
        armGraphicAngle.z = armAngle;
        armGraphic.eulerAngles = armGraphicAngle;

        physicsComponent.gravityScale = gravityScale;

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
            if (!bullet.gameObject.activeInHierarchy) {
                bullet.gameObject.SetActive(true);
                bullet.position = handGraphic.position;
                bullet.velocity = Vector2.right.Rotate(Mathf.Deg2Rad * armAngle) * bulletSpeed;
            }
        }
    }

    /// <summary>
    /// OnCollisionEnter2D is a Unity built-in message.
    /// Unity calls it when this game object collides with something in the
    /// physics simulation.
    /// There are other collision messages:
    /// OnCollisionExit2D
    /// OnCollisionStay2D
    /// There are also trigger messages.
    /// Read more here: https://docs.unity3d.com/Manual/CollidersOverview.html
    /// </summary>
    protected void OnCollisionEnter2D(Collision2D collision) {
        if (health <= 0) return;

        if (collision.gameObject.CompareTag("Victory")) {
            DebugUtils.Log("Great Job!");
            return;
        }
        if (!collision.gameObject.CompareTag("Enemy")) return;

        --health;
        if (health > 0) return;

        animator.SetBool(isAliveParam, false);
        physicsComponent.velocity = Vector2.zero;
        physicsComponent.gravityScale = 4f;
        enabled = false;
    }
}
}
