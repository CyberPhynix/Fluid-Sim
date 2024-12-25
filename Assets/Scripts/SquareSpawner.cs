using UnityEngine;

public class SquareSpawner : Spawner
{
    public uint rows = 10;
    public uint columns = 10;
    [Min(0)]
    public float spacing = 1;

    public override void Init()
    {
        var count = rows * columns;
        positions = new Vector2[count];
        velocitys = new Vector2[count];

        for (var i = 0; i < count; i++)
        {
            var row = i / columns - rows / 2;
            var column = i % columns - columns / 2;
            positions[i] = new Vector2(column * spacing, row * spacing);
            velocitys[i] = Vector2.zero;
        }
    }
}