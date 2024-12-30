using UnityEngine;

public class Simulation : MonoBehaviour
{
    public Display display;
    public Spawner spawner;

    [Header("Simulation")]
    public uint timeStepsPerSecond = 50;

    [Header("Physics")]
    [Range(0, 10)]
    public float gravity = 9.81f;
    [Range(0, 1)]
    public float collisionDamping = 0.9f;
    [Range(0, 3)]
    public float smoothingRadius = 1;
    [Min(0)]
    public float particleMass = 1;
    [Min(0)]
    public float targetDensity = 5;
    [Tooltip("Stiffness; How fast the density is corrected; Gas constant")]
    public float pressureForceMultiplier = 1;

    [Header("Environment")]
    public Vector2 boundSize = new(15, 8);

    private float[] densities;
    private Vector2[] forces;
    private Vector2[] positions;
    private float[] pressures;
    private Vector2[] velocitys;

    // Start is called once before the first execution of Update
    private void Start()
    {
        Time.fixedDeltaTime = 1f / timeStepsPerSecond;

        spawner.Init();
        positions = spawner.GetPositions();
        velocitys = spawner.GetVelocitys();
        densities = new float[positions.Length];
        pressures = new float[positions.Length];
        forces = new Vector2[positions.Length];
    }

    // Update is called once per frame
    private void Update()
    {
        display.DrawCircles(positions);
    }

    // FixedUpdate is called Frame-rate independent
    private void FixedUpdate()
    {
        for (var i = 0; i < positions.Length; i++)
        {
            densities[i] = CalculateDensity(positions[i]);
            pressures[i] = CalculatePressure(i);
        }


        for (var i = 0; i < positions.Length; i++)
        {
            // Force
            Vector2 force = Vector2.zero;

            // Pressure
            force += CalculatePressureForce(i);

            // Gravity
            force += Vector2.down * gravity;

            forces[i] = force;
        }

        for (var i = 0; i < positions.Length; i++)
        {
            velocitys[i] += forces[i] * Time.deltaTime;
            positions[i] += velocitys[i] * Time.deltaTime;

            // Collisions
            resolveCollision(ref positions[i], ref velocitys[i]);
        }
    }

    // Draw gizmos bounding box
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, .4f, 0, .5f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize * 2);
        if (positions == null) return;
        for (var i = 0; i < positions.Length; i++)
        {
            // density deviation
            var pd = densities[i] - targetDensity;
            Gizmos.color = Color.Lerp(pd > 0 ? Color.red : Color.blue, Color.green, 1 - Mathf.Abs(pd));
            Gizmos.DrawSphere(positions[i], 0.08f);
        }
    }

    public float CalculateDensity(Vector2 pos)
    {
        var density = 0f;
        for (var j = 0; j < positions.Length; j++)
        {
            var distance = Vector2.Distance(pos, positions[j]);
            density += particleMass * Kernel.Poly6Kernel(distance, smoothingRadius);
        }

        return density;
    }

    private float CalculatePressure(int i)
    {
        var ownDensity = Kernel.Poly6Kernel(0, smoothingRadius);
        var density = densities[i] - ownDensity;
        return pressureForceMultiplier * (density - targetDensity);
        // return Mathf.Max(pressureForceMultiplier * (density - targetDensity), 0);
    }

    private Vector2 CalculatePressureForce(int particleIndex)
    {
        Vector2 pressureForce = Vector2.zero;
        for (var j = 0; j < positions.Length; j++)
        {
            if (particleIndex == j) continue;

            var distance = Vector2.Distance(positions[particleIndex], positions[j]);
            Vector2 direction = (positions[j] - positions[particleIndex]).normalized;

            var sharedPressure = (pressures[particleIndex] + pressures[j]) / 2;
            pressureForce += direction *
                             (sharedPressure * Kernel.SpikyKernelGradient(distance, smoothingRadius) *
                                 particleMass / densities[j]);
            // pressureForce += direction * positions[j] * (Kernel.SpikyKernelGradient(distance, smoothingRadius) * particleMass) /
            //                 densities[j];
        }

        return pressureForce;
    }

    private void resolveCollision(ref Vector2 pos, ref Vector2 vel)
    {
        if (Mathf.Abs(pos.x) > boundSize.x)
        {
            pos.x = Mathf.Sign(pos.x) * boundSize.x;
            vel.x *= -1 * collisionDamping;
        }

        if (Mathf.Abs(pos.y) > boundSize.y)
        {
            pos.y = Mathf.Sign(pos.y) * boundSize.y;
            vel.y *= -1 * collisionDamping;
        }
    }
}