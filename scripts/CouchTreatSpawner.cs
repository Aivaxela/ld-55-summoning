using Godot;

public partial class CouchTreatSpawner : Marker2D
{
    [Export] PackedScene couchPickup;

    public override void _Ready()
    {
        int willSpawnInt = GD.RandRange(0, 2);

        if (willSpawnInt == 2)
        {
            EnergyPickup newCouchPickup = (EnergyPickup)couchPickup.Instantiate();
            CallDeferred("AddNewChild", newCouchPickup);
            newCouchPickup.GlobalPosition = GlobalPosition;
        }
    }

    private void AddNewChild(EnergyPickup newCouchPickup)
    {
        GetParent().AddChild(newCouchPickup);
    }
}
