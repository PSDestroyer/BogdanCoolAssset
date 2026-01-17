using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelLoader : MonoBehaviour
{
    public LevelData nextLevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LevelManager.Instance.LoadScene(nextLevel);
        }
    }
}
