using UnityEngine;
using UnityEngine.UI;

public class CrystalGraphic : MaskableGraphic
{
    [SerializeField] private Color topColor = new Color(0.64f, 1f, 1f, 1f);
    [SerializeField] private Color midColor = new Color(0.08f, 0.72f, 0.95f, 1f);
    [SerializeField] private Color bottomColor = new Color(0.02f, 0.28f, 0.58f, 1f);

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();

        Rect rect = GetPixelAdjustedRect();
        Vector2 top = new Vector2(rect.center.x, rect.yMax);
        Vector2 upperLeft = new Vector2(rect.xMin + rect.width * 0.22f, rect.yMax - rect.height * 0.28f);
        Vector2 upperRight = new Vector2(rect.xMax - rect.width * 0.22f, rect.yMax - rect.height * 0.28f);
        Vector2 center = rect.center;
        Vector2 lowerLeft = new Vector2(rect.xMin + rect.width * 0.18f, rect.yMin + rect.height * 0.35f);
        Vector2 lowerRight = new Vector2(rect.xMax - rect.width * 0.18f, rect.yMin + rect.height * 0.35f);
        Vector2 bottom = new Vector2(rect.center.x, rect.yMin);

        AddTriangle(vertexHelper, top, upperLeft, center, topColor);
        AddTriangle(vertexHelper, top, center, upperRight, new Color(0.42f, 0.96f, 1f, 1f));
        AddTriangle(vertexHelper, upperLeft, lowerLeft, center, midColor);
        AddTriangle(vertexHelper, upperRight, center, lowerRight, new Color(0.04f, 0.62f, 0.9f, 1f));
        AddTriangle(vertexHelper, center, lowerLeft, bottom, bottomColor);
        AddTriangle(vertexHelper, center, bottom, lowerRight, new Color(0.02f, 0.2f, 0.48f, 1f));
    }

    private static void AddTriangle(VertexHelper vertexHelper, Vector2 first, Vector2 second, Vector2 third, Color color)
    {
        int startIndex = vertexHelper.currentVertCount;
        vertexHelper.AddVert(first, color, Vector2.zero);
        vertexHelper.AddVert(second, color, Vector2.zero);
        vertexHelper.AddVert(third, color, Vector2.zero);
        vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
    }
}
