namespace Content.Shared.NRFD14.BoomBox;

[RegisterComponent]
public sealed partial class BoomBoxComponent : Component
{
    [DataField]
    public bool Playing = false;

    [DataField]
    public int CurrentTrack = 0;

    [DataField(required: true)]
    public List<SoundSpecifier> Tracks = new();

    [DataField]
    public SoundSpecifier? StopSound;
}
