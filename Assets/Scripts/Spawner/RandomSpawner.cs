using UnityEngine;

public class RandomSpawner : Spawner
{
    public uint count = 100;
    public float initialVelocity = 1;
    public bool spawnInMiddle;

    public override void Init()
    {
        positions = new Vector2[count];
        velocitys = new Vector2[count];

        for (var i = 0; i < count; i++)
        {
            positions[i] = spawnInMiddle ? Vector2.zero : new Vector2(Random.Range(-15f, 15f), Random.Range(-8f, 8f));
            velocitys[i] = new Vector2(Random.value * 2 - 1f, Random.value * 2 - 1f) * initialVelocity;
        }
    }
}