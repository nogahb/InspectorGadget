using UnityEngine;
using Infra.Utils;

namespace Gadget {
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    public float timeToLive = 2f;

    protected void OnEnable() {
        CoroutineUtils.DelaySeconds(this, () => gameObject.SetActive(false), timeToLive);
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            return;
        }
        gameObject.SetActive(false);
    }
}
}
