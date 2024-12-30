using UnityEngine;

public class CustomSpawner : Spawner
{
    public Vector2[] customPositions;
    public Vector2[] customVelocitys;

    public override void Init()
    {
        if (customVelocitys.Length == 0)
            for (var i = 0; i < customPositions.Length; i++)
                customVelocitys[i] = Vector2.zero;
        if (customPositions.Length != customVelocitys.Length)
            throw new UnityException("CustomSpawner: customPositions and customVelocitys must have the same length.");

        positions = customPositions;
        velocitys = customVelocitys;
    }
}