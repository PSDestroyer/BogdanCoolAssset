using UnityEngine;
using System.Collections.Generic;

public class OutlineManager : MonoBehaviour
{
    private class OutlineData
    {
        public Renderer renderer;
        public Material[] originalMaterials;
        public Material outlineMaterial;
    }

    private OutlineData currentOutline;

    public Material baseOutlineMaterial;

    public void ShowOutline(Renderer renderer, Color color, float width)
    {
        if (currentOutline != null && currentOutline.renderer == renderer) return;

        ClearOutline();

        currentOutline = new OutlineData
        {
            renderer = renderer,
            originalMaterials = renderer.sharedMaterials,
            outlineMaterial = new Material(baseOutlineMaterial)
        };

        currentOutline.outlineMaterial.SetColor("_OutlineColor", color);
        currentOutline.outlineMaterial.SetFloat("_OutlineWidth", width);

        var mats = new List<Material>(currentOutline.originalMaterials);
        mats.Add(currentOutline.outlineMaterial);

        renderer.materials = mats.ToArray();
    }

    public void ClearOutline()
    {
        if (currentOutline == null) return;

        currentOutline.renderer.materials = currentOutline.originalMaterials;
        Destroy(currentOutline.outlineMaterial);
        currentOutline = null;
    }
}
