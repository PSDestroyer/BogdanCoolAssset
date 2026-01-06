using GenesisStudio;

public interface IDamageable
{
    float Health { get; set; }

    void ApplyDamage(float damage);
    void Die();
}