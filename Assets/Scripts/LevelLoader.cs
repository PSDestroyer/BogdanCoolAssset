using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelLoader : MonoBehaviour
{
    public LevelData nextLevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Load();
        }
    }

    protected virtual void Load()
    {
        LevelManager.Instance.LoadScene(nextLevel);
    }
}
