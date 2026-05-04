using Content.Shared.Speech.EntitySystems;
using Content.Shared.StatusEffectNew;
using Content.Shared.Traits.Assorted;
using Robust.Shared.Prototypes;

namespace Content.Shared._HL.Aphrodisiac;

public abstract class SharedAphrodisiacSystem : EntitySystem
{
    public static EntProtoId Aphrodisiac = "StatusEffectAphrodisiac";

    // I also have no idea, copied this from Drunk system.

    /* I have no clue why this magic number was chosen, I copied it from slur system and needed it for the overlay
    If you have a more intelligent magic number be my guest to completely explode this value.
    There were no comments as to why this value was chosen three years ago. */
    public static float MagicNumber = 1100f;

    [Dependency] protected readonly StatusEffectsSystem Status = default!;

    public override void Initialize()
    {
    }

    public void TryApplyAphrodisiacs(EntityUid uid, TimeSpan aphrodisiacPower)
    {
        var ev = new AphrodisiacEvent(aphrodisiacPower);
        RaiseLocalEvent(uid, ref ev);

        Status.TryAddStatusEffectDuration(uid, Aphrodisiac, ev.Duration);
    }

    public void TryRemoveAphrodisiacs(EntityUid uid)
    {
        Status.TryRemoveStatusEffect(uid, Aphrodisiac);
    }

    public void TryRemoveAphrodisiacsTime(EntityUid uid, TimeSpan aphrodisiacPower)
    {
        Status.TryAddTime(uid, Aphrodisiac, - aphrodisiacPower);
    }

    [ByRefEvent]
    public record struct AphrodisiacEvent(TimeSpan Duration);
}
