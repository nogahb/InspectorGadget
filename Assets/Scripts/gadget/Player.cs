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
    public float param02 = 15f;
    public float param03 = 7f;
    public float param04 = 2f;
    public float armAngle = 45f;
    public float param06 = 15f;

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

    private readonly int param14 = Animator.StringToHash("Alive");
    private readonly int param15 = Animator.StringToHash("Jump");

    private Animator param16;
    private Rigidbody2D param17;
    private OverlapChecker param18;

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
    private bool Property01 {
        get {
            return param18.isOverlapping;
        }
    }

    /// <summary>
    /// Awake is a Unity built-in message.
    /// Unity calls it when the object is loaded into the scene. Think of it like
    /// a constructor.
    /// </summary>
    protected void Awake() {
        param16 = GetComponent<Animator>();
        param17 = GetComponent<Rigidbody2D>();
        param18 = GetComponent<OverlapChecker>();

        param16.SetBool(param14, true);
    }

    /// <summary>
    /// Update is a Unity built-in message.
    /// Unity calls it every frame - every time the screen renders the game view.
    /// </summary>
    protected void Update() {
        var armGraphicAngle = armGraphic.eulerAngles;
        armGraphicAngle.z = armAngle;
        armGraphic.eulerAngles = armGraphicAngle;

        param17.gravityScale = param04;

        var var02 = param17.velocity;
        if (Input.GetKeyDown(jumpKey) && Property01) {
            var02.y = param02;
            param17.velocity = var02;
            param16.SetTrigger(param15);
        } else if (Input.GetKey(rightKey)) {
            var02.x = param03;
            param17.velocity = var02;
        } else if (Input.GetKey(leftKey)) {
            var02.x = -param03;
            param17.velocity = var02;
        } else if (Input.GetKey(shootKey)) {
            if (!bullet.gameObject.activeInHierarchy) {
                bullet.gameObject.SetActive(true);
                bullet.position = handGraphic.position;
                bullet.velocity = Vector2.right.Rotate(Mathf.Deg2Rad * armAngle) * param06;
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

        param16.SetBool(param14, false);
        param17.velocity = Vector2.zero;
        param17.gravityScale = 4f;
        enabled = false;
    }
}
}
