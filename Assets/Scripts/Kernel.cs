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

    public static float Poly6KernelGradient(float r, float h)
    {
        if (r < 0 || r > h) return 0;

        var h2 = h * h;
        var h8 = h2 * h2 * h2 * h2;

        return -24 * r * Mathf.Pow(h2 - r * r, 2) / (Mathf.PI * h8);
    }

    public static float SpikyKernel(float r, float h)
    {
        if (r < 0 || r > h) return 0;

        var h6 = h * h * h * h * h * h;

        return 15f / (Mathf.PI * h6) * Mathf.Pow(h - r, 3);
    }

    public static float SpikyKernelGradient(float r, float h)
    {
        if (r < 0 || r > h) return 0;

        var h6 = h * h * h * h * h * h;

        return -45f / (Mathf.PI * h6) * Mathf.Pow(h - r, 2);
    }

    public static float SLSmoothingKernel(float dst, float radius)
    {
        if (dst >= radius)
            return 0;

        var volume = Mathf.PI * Mathf.Pow(radius, 4) / 6;
        return (radius - dst) * (radius - dst) / volume;
    }

    public static float SLSmoothingKernelDerivative(float dst, float radius)
    {
        if (dst >= radius)
            return 0;

        var scale = 12 / (Mathf.Pow(radius, 4) * Mathf.PI);
        return (dst - radius) * scale;
    }
}