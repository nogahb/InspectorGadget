using UnityEngine;
using Infra.UI;

namespace Gadget.UI {
public class HealthCounter : CounterText {
    [SerializeField] Player player;
    protected override int GetTarget() {
        return player.health;
    }
}
}
