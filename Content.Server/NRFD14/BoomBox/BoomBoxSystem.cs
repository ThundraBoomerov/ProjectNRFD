using Content.Shared.Interaction.Events;
using Content.Shared.NRFD14.BoomBox;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Server.NRFD14.BoomBox;

public sealed class BoomBoxSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BoomBoxComponent, UseInHandEvent>(OnUseInHand);
    }

    private void OnUseInHand(EntityUid uid, BoomBoxComponent comp, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (comp.Tracks.Count == 0)
            return;

        if (!comp.Playing)
        {
            var stream = _audio.PlayPvs(
                comp.Tracks[comp.CurrentTrack],
                uid,
                AudioParams.Default.WithLoop(true)
            );

            comp.AudioStream = stream?.Entity;
            comp.Playing = true;
            return;
        }

        if (comp.AudioStream != null)
        {
            QueueDel(comp.AudioStream.Value);
            comp.AudioStream = null;
        }

        if (comp.StopSound != null)
            _audio.PlayPvs(comp.StopSound, uid);

        comp.Playing = false;
        comp.CurrentTrack = (comp.CurrentTrack + 1) % comp.Tracks.Count;
    }
}
