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

    [Header("Environment")]
    public Vector2 boundSize = new(15, 8);

    private Vector2[] positions;
    private Vector2[] velocitys;

    // Start is called once before the first execution of Update
    private void Start()
    {
        spawner.Init();
        positions = spawner.GetPositions();
        velocitys = spawner.GetVelocitys();
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
            // Force
            var force = Vector2.zero;

            // Gravity
            force += Vector2.down * gravity;

            velocitys[i] += force * Time.deltaTime;

            positions[i] += velocitys[i] * Time.deltaTime;

            // Collisions
            if (Mathf.Abs(positions[i].x) > boundSize.x)
            {
                positions[i].x = Mathf.Sign(positions[i].x) * boundSize.x;
                velocitys[i].x *= -1 * collisionDamping;
            }

            if (Mathf.Abs(positions[i].y) > boundSize.y)
            {
                positions[i].y = Mathf.Sign(positions[i].y) * boundSize.y;
                velocitys[i].y *= -1 * collisionDamping;
            }
        }
    }

    // Draw gizmos bounding box
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, .4f, 0, .5f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize * 2);
    }
}