using Content.Shared.Interaction.Events;
using Robust.Shared.Audio.Systems;

namespace Content.Shared.NRFD14.BoomBox;

public sealed class BoomBoxSystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<BoomBoxComponent, UseInHandEvent>(OnUseInHand);
    }

    private void OnUseInHand(EntityUid uid, BoomBoxComponent comp, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (!comp.Playing)
        {
            if (comp.Tracks.Count == 0)
                return;

            _audio.PlayPvs(comp.Tracks[comp.CurrentTrack], uid);
            comp.Playing = true;
        }
        else
        {
            if (comp.StopSound != null)
                _audio.PlayPvs(comp.StopSound, uid);

            comp.Playing = false;

            comp.CurrentTrack++;

            if (comp.CurrentTrack >= comp.Tracks.Count)
                comp.CurrentTrack = 0;
        }
    }
}
