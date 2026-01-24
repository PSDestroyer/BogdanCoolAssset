using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Boss", order = 0)]
public class BossData : ScriptableObject
{
    private Boss _boss;
    
    public void Initialize(Boss boss)
    {
        _boss = boss;
    }
}
