using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    public uint count = 100;
    [Range(0, 8)]
    public float spawnSize = 4;
    [Min(0)]
    public float initialVelocity = 1;

    private Vector2[] positions;
    private Vector2[] velocitys;

    public void Init()
    {
        positions = new Vector2[count];
        velocitys = new Vector2[count];

        for (var i = 0; i < count; i++)
        {
            positions[i] = new Vector2(Mathf.Sin(i * 2 * Mathf.PI / count) * spawnSize, Mathf.Cos(i * 2 * Mathf.PI / count) * spawnSize);
            velocitys[i] = new Vector2(Random.value * 2 - 1f, Random.value * 2 - 1f) * initialVelocity;
        }
    }

    public Vector2[] GetPositions()
    {
        return positions;
    }

    public Vector2[] GetVelocitys()
    {
        return velocitys;
    }
}