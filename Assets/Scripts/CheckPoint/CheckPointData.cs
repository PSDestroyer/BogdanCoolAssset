using System.Collections.Generic;
using UnityEngine;

public struct CheckPointData
{
    public float health;
    public float bananas;
    public List<Ability> abilities;
    public Transform spawnPoint;

    public CheckPointData(float health, float bananas, List<Ability> abilities, Transform spawnPoint)
    {
        this.health = health;
        this.bananas = bananas;
        this.abilities = abilities;
        this.spawnPoint = spawnPoint;
    }
}
