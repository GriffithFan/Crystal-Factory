using UnityEngine;
using UnityEngine.UI;

public class SoftEllipseGraphic : MaskableGraphic
{
    [SerializeField] private Color centerColor = new Color(0.12f, 0.85f, 1f, 0.34f);
    [SerializeField] private Color edgeColor = new Color(0.02f, 0.22f, 0.42f, 0f);
    [SerializeField] private int segments = 48;

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();

        Rect rect = GetPixelAdjustedRect();
        Vector2 center = rect.center;
        float radiusX = rect.width * 0.5f;
        float radiusY = rect.height * 0.5f;

        vertexHelper.AddVert(center, centerColor, new Vector2(0.5f, 0.5f));

        int safeSegments = Mathf.Clamp(segments, 12, 96);
        for (int index = 0; index <= safeSegments; index++)
        {
            float angle = (Mathf.PI * 2f * index) / safeSegments;
            Vector2 point = center + new Vector2(Mathf.Cos(angle) * radiusX, Mathf.Sin(angle) * radiusY);
            vertexHelper.AddVert(point, edgeColor, Vector2.zero);
        }

        for (int index = 1; index <= safeSegments; index++)
        {
            vertexHelper.AddTriangle(0, index, index + 1);
        }
    }
}
