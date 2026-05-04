using Content.Shared._HL.Aphrodisiac;
using Content.Shared.StatusEffectNew;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client._HL.Aphrodisiac;

public sealed class AphrodisiacSystem : SharedAphrodisiacSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private AphrodisiacOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AphrodisiacStatusEffectComponent, StatusEffectAppliedEvent>(OnStatusApplied);
        SubscribeLocalEvent<AphrodisiacStatusEffectComponent, StatusEffectRemovedEvent>(OnStatusRemoved);

        SubscribeLocalEvent<AphrodisiacStatusEffectComponent, StatusEffectRelayedEvent<LocalPlayerAttachedEvent>>(OnPlayerAttached);
        SubscribeLocalEvent<AphrodisiacStatusEffectComponent, StatusEffectRelayedEvent<LocalPlayerDetachedEvent>>(OnPlayerDetached);

        _overlay = new();
    }

    private void OnStatusApplied(Entity<AphrodisiacStatusEffectComponent> entity, ref StatusEffectAppliedEvent args)
    {
        if (!_overlayMan.HasOverlay<AphrodisiacOverlay>())
            _overlayMan.AddOverlay(_overlay);
    }

    private void OnStatusRemoved(Entity<AphrodisiacStatusEffectComponent> entity, ref StatusEffectRemovedEvent args)
    {
        if (Status.HasEffectComp<AphrodisiacStatusEffectComponent>(args.Target))
            return;

        if (_player.LocalEntity != args.Target)
            return;

        _overlay.CurrentAphrodisiacPower = 0;
        _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnPlayerAttached(Entity<AphrodisiacStatusEffectComponent> entity, ref StatusEffectRelayedEvent<LocalPlayerAttachedEvent> args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(Entity<AphrodisiacStatusEffectComponent> entity, ref StatusEffectRelayedEvent<LocalPlayerDetachedEvent> args)
    {
        _overlay.CurrentAphrodisiacPower = 0;
        _overlayMan.RemoveOverlay(_overlay);
    }
}
