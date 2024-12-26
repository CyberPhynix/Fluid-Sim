using UnityEngine;

public class Simulation : MonoBehaviour
{
    public Display display;
    public Spawner spawner;

    [Header("Physics")]
    [Range(0, 10)]
    public float gravity = 9.81f;
    [Range(0, 1)]
    public float collisionDamping = 0.9f;
    [Range(0, 3)]
    public float smoothingRadius = 1;
    [Range(0, 2)]
    public float particleMass = 1;

    [Header("Environment")]
    public Vector2 boundSize = new(15, 8);

    private float[] densities; // REMOVE
    private Vector2[] positions;
    private Vector2[] velocitys;

    // Start is called once before the first execution of Update
    private void Start()
    {
        spawner.Init();
        positions = spawner.GetPositions();
        velocitys = spawner.GetVelocitys();
        densities = new float[positions.Length]; // REMOVE
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
            // Density
            densities[i] = CalculateDensity(positions[i]);

            // Force
            var force = Vector2.zero;

            // Gravity
            force += Vector2.down * gravity;

            velocitys[i] += force * Time.deltaTime;

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
            var color = Color.Lerp(Color.green, Color.red, densities[i] / 10);
            Gizmos.color = color;
            Gizmos.DrawSphere(positions[i], 0.2f);
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