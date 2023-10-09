using Towers;

/// <summary>
///  Interface for damageable objects.
/// </summary>
public interface IDamageable
{
    public void TakeDamage(ApplyingDamageToEnemy applyingDamage);
}