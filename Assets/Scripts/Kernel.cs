using UnityEngine;

public static class Kernel
{
    public static float Poly6Kernel(float r, float h)
    {
        if (r < 0 || r > h) return 0;

        var h2 = h * h;
        var h9 = h2 * h2 * h2 * h2 * h;

        var volume = 315 / (256 * h);

        return 315f / (64f * Mathf.PI * h9) * Mathf.Pow(h2 - r * r, 3) / volume;
    }

    public static float SpikyKernel(float r, float h)
    {
        if (r < 0 || r > h) return 0;

        var h6 = h * h * h * h * h * h;

        return 15f / (Mathf.PI * h6) * Mathf.Pow(h - r, 3);
    }
}