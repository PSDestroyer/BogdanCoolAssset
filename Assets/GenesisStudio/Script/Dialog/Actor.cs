using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Genesis Studio/Dialogue/Actor")]
public class Actor : ScriptableObject
{
    [SerializeField] string actor;
    [SerializeField] Color actorColor = Color.darkGreen;

    public string ActorName { get => actor; set => actor = value; }
    public Color ActorColor { get => actorColor; set => actorColor = value; }
}
