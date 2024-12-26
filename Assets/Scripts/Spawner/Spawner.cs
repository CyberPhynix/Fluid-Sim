using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [Min(0)]
    protected Vector2[] positions;
    protected Vector2[] velocitys;

    public abstract void Init();

    public Vector2[] GetPositions()
    {
        return positions;
    }

    public Vector2[] GetVelocitys()
    {
        return velocitys;
    }
}