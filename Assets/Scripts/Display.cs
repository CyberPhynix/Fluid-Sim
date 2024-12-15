using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    [Header("Default Circle Settings")]
    public Color circleColor = Color.blue;
    public float circleSize = .5f;
    [Range(3, 30)]
    public uint circleResolution = 30;

    // Caching
    private Mesh circleMesh;
    private Color materialColor;
    private RenderParams rp;

    private void OnValidate()
    {
        UpdateMesh(circleSize, circleResolution);
        UpdateMaterial(circleColor);
    }

    public void UpdateMesh(float size, uint resolution)
    {
        circleSize = size;
        circleResolution = resolution;
        circleMesh = GenerateCircleMesh(circleSize / 2, circleResolution);
    }

    private void UpdateMaterial(Color color)
    {
        materialColor = color;
        var material = new Material(Shader.Find("Universal Render Pipeline/Unlit"))
        {
            enableInstancing = true,
            color = color
        };
        rp = new RenderParams(material);
    }

    public void DrawCircles(Vector2[] positions)
    {
        var matrices = new Matrix4x4[positions.Length];

        for (var i = 0; i < positions.Length; i++)
            matrices[i] = Matrix4x4.Translate(positions[i]);

        Graphics.RenderMeshInstanced(rp, circleMesh, 0, matrices);
    }

    private Mesh GenerateCircleMesh(float radius, uint resolution)
    {
        var mesh = new Mesh();

        var verticiesList = new List<Vector3>();
        float x, y;
        for (var i = 0; i < resolution; i++)
        {
            x = radius * Mathf.Sin(2 * Mathf.PI * i / resolution);
            y = radius * Mathf.Cos(2 * Mathf.PI * i / resolution);
            verticiesList.Add(new Vector3(x, y, 0f));
        }

        var verticies = verticiesList.ToArray();

        var trianglesList = new List<int>();
        for (var i = 0; i < resolution - 2; i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i + 1);
            trianglesList.Add(i + 2);
        }

        var triangles = trianglesList.ToArray();

        var normalsList = new List<Vector3>();
        for (var i = 0; i < verticies.Length; i++) normalsList.Add(-Vector3.forward);
        var normals = normalsList.ToArray();

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.normals = normals;

        mesh.RecalculateBounds();

        return mesh;
    }
}