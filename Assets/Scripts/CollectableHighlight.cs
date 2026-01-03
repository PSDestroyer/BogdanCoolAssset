using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CollectableHighlight : MonoBehaviour
{
    [SerializeField] float fadeDuration = 0.5f;

    Renderer _renderer;
    MaterialPropertyBlock _mpb;
    Coroutine _fadeRoutine;
    float _currentGlow = 0f; // Cache the current value

    static readonly int GlowStrengthID = Shader.PropertyToID("_GlowStrength");

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();

        SetGlow(0f);
    }

    [ContextMenu("Enable Highlight")]
    public void EnableHighlight()
    {
        StartFade(1f);
    }

    [ContextMenu("Disable Highlight")]
    public void DisableHighlight()
    {
        StartFade(0f);
    }

    void StartFade(float target)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeRoutine(target));
    }

    IEnumerator FadeRoutine(float target)
    {
        float startValue = _currentGlow;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            SetGlow(Mathf.Lerp(startValue, target, t));
            yield return null;
        }

        SetGlow(target);
    }

    void SetGlow(float value)
    {
        _currentGlow = value;
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat(GlowStrengthID, value);
        _renderer.SetPropertyBlock(_mpb);
    }
}