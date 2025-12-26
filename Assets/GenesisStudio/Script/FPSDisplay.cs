using UnityEngine;
public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    public Vector2 size;
    void Update() => deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    void OnGUI()
    {
        int fps = Mathf.CeilToInt(1.0f / deltaTime);
        GUI.Label(new Rect(10, 10, size.x, size.y), fps + " FPS");
    }
}
