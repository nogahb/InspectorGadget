using UnityEngine;

namespace Infra.Gameplay {
public abstract class OverlapChecker : MonoBehaviour {
    [SerializeField] protected Transform checkerPosition;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] bool excludeTriggers;

    protected RaycastHit2D[] hits = new RaycastHit2D[4];
    protected Collider2D[] colliders = new Collider2D[4];

    [Header("Read Only")]
    public bool isOverlapping;
    public Collider2D overlappedCollider;
    public int overlappedColliderLayerValue;

    protected void Awake() {
        isOverlapping = false;
    }

    protected abstract int CheckHits();

    protected void Update() {
        var hitCount = CheckHits();
        if (hitCount > 0) {
            for (int i = 0; i < hitCount; i++) {
                if (colliders[i].gameObject != gameObject && (!excludeTriggers || !colliders[i].isTrigger)) {
                    if (overlappedCollider != colliders[i]) {
                        overlappedCollider = colliders[i];
                        overlappedColliderLayerValue = 1 << overlappedCollider.gameObject.layer;
                    }
                    isOverlapping = true;
                    return;
                }
            }
        }
        isOverlapping = false;
    }
}
}
