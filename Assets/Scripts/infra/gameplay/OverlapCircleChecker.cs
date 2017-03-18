using UnityEngine;

namespace Infra.Gameplay {
public class OverlapCircleChecker : OverlapChecker {
    [Header("Custom")]
    [SerializeField] float radius;

    protected override int CheckHits() {
        return Physics2D.OverlapCircleNonAlloc(checkerPosition.position, radius, colliders, layerMask.value);
    }
}
}
