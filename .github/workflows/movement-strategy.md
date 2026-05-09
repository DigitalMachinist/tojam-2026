# Movement Strategy — Spec

## Goal

Decouple `ShadowManager` and `PlayerController` from the concrete `Shadow` class so any "moves in relation to a Transform" component can drop into the prefab slot. `Shadow` becomes one of N strategies; consumers only need to know "this thing has an anchor Transform I can read and swap."

## Shape: abstract MonoBehaviour base class — NOT a plain C# `interface`

```csharp
[RequireComponent(typeof(Rigidbody))]
public abstract class AnchoredMover : MonoBehaviour
{
    [SerializeField] protected Transform anchor;

    public Transform Anchor => anchor;
    public virtual void SetAnchor(Transform t) => anchor = t;
}
```

That is the entire contract. Per-strategy fields (speed, easing, endpoints, debug toggles) live on the subclass. No lifecycle hooks, no events, no registry — add those when a second strategy actually needs them.

### Why a base class instead of a C# `interface`

1. **Inspector serialization.** Unity does not serialize plain interface fields. A `Shadow shadowAnchors` field re-typed to `IAnchoredMover` would stop accepting drag-and-drop assignment — you'd need `[SerializeReference]` (plain C# only, not MonoBehaviours), or store a `MonoBehaviour` and cast. Friction with no payoff.
2. **`[RequireComponent(typeof(Rigidbody))]` lives once** on the base and every subclass inherits it.
3. **Polymorphic prefab fields just work.** `[SerializeField] AnchoredMover shadowPrefab;` accepts any subclass in the Inspector.
4. **No duplicated boilerplate** for the `anchor` field, getter, and setter across strategies.

ScriptableObject-injected strategy was considered and rejected for now: only one strategy exists today, runtime per-instance state still has to live on the MonoBehaviour, and SO instances need cloning to avoid shared state. Reserve for when movement params become designer-authored data.

## Concrete implementations

- **`Shadow : AnchoredMover`** — keeps `speed` and the debug-ray fields; `FixedUpdate` becomes the override of an abstract `Tick`-equivalent (or stays as `FixedUpdate` directly — the base doesn't dictate).
- **Future:** `PingPongMover : AnchoredMover` adapted from `PingPongBetweenTransforms.cs`. Other movement scripts in the project (`RotateAroundAxis`, `LeafPivot`, `DebugRayToParent`) are *not* "move toward a target" and stay outside this hierarchy.

## Consumer changes

| File | Change |
|---|---|
| `ShadowManager.cs` line 17 | `[SerializeField] private Shadow shadowPrefab;` → `AnchoredMover shadowPrefab;` |
| `ShadowManager.cs` line 25 | `List<Shadow> shadows` → `List<AnchoredMover> shadows` |
| `ShadowManager.cs` line 48 | `Shadow shadow = Instantiate(...)` → `AnchoredMover shadow = Instantiate(...)` |
| `PlayerController.cs` line 27 | `[SerializeField] private Shadow shadowAnchors;` → `AnchoredMover shadowAnchors;` |

`Anchor` getter and `SetAnchor()` calls in `ShadowManager.RetargetRoutine` (lines 66–67, 74) and `PlayerController.InteractRoutine` (lines 101–102, 106) are unchanged — they hit the base-class members.

## Migration steps

1. Add `Assets/Scripts/AnchoredMover.cs` with the abstract base above.
2. Edit `Shadow.cs`: change `class Shadow : MonoBehaviour` → `class Shadow : AnchoredMover`. Delete the local `anchor`, `Anchor`, `SetAnchor` — they're inherited. Keep `RequireComponent` on the base only, remove from `Shadow`. Keep `speed` and debug fields.
3. Re-type `ShadowManager.shadowPrefab` and `shadows` list to `AnchoredMover`.
4. Re-type `PlayerController.shadowAnchors` to `AnchoredMover`.
5. Open `Assets/Scenes/Test_ShadowMotion.unity` and `Assets/Scripts/Shadow.prefab`. Inspector references should auto-resolve since `Shadow` still satisfies the `AnchoredMover` slot — verify no missing-script warnings.

## Out of scope

- Strategy registry / factory. `Instantiate` on a typed prefab field is enough.
- `OnAnchorChanged` events. Add when a second strategy needs to react to swaps.
- Generalizing `RotateAroundAxis` or `LeafPivot` — different shape, not "anchored mover."
- ScriptableObject strategy data. Revisit when designers want to tune movement without touching prefabs.
