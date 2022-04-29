using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PolygonManager
{
    public static void DrawRegularPolygon(this LineRenderer lineRenderer, float sides, float radius, float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        lineRenderer.loop = false;



        const float tau = Mathf.PI * 2;
        int sidesInt = Mathf.CeilToInt(sides);

        lineRenderer.positionCount = sidesInt;
        for (int i = 0; i < sidesInt; i++)
        {
            float radian = i / sides * tau;
            float x = Mathf.Cos(radian) * radius;
            float y = Mathf.Sin(radian) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y));
        }



        const int extraSteps = 2;
        lineRenderer.positionCount += extraSteps;

        int posCount = lineRenderer.positionCount;
        for (int i = 0; i < extraSteps; i++)
            lineRenderer.SetPosition(posCount - extraSteps + i, lineRenderer.GetPosition(i));
    }
}
