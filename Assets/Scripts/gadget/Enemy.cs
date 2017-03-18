using UnityEngine;
using System;

namespace Gadget {
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    public float patrolSize;
    public float movementSpeed = 3f;

    private float fromPoint;
    private float toPoint;
    private Rigidbody2D body;

    protected void Awake() {
        body = GetComponent<Rigidbody2D>();

        fromPoint = transform.position.x;
        toPoint = fromPoint + patrolSize;

        if (fromPoint > toPoint) {
            var temp = fromPoint;
            fromPoint = toPoint;
            toPoint = temp;
        }
    }

    protected void FixedUpdate() {
        var x = transform.position.x;
        var v = body.velocity;
        if (v.x >= 0f && toPoint <= x) {
            v.x = -movementSpeed;
            body.velocity = v;
        } else if (v.x <= 0f && x <= fromPoint) {
            v.x = movementSpeed;
            body.velocity = v;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            gameObject.SetActive(false);
        } else if (collision.gameObject.CompareTag("Player")) {
            enabled = false;
        }
    }
}
}
