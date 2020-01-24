using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour, IDamagable
{
    public static event Action<string> Notify = delegate { };

    [Header("Player Info")]
    public new string name;

    public float health;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}

public interface IDamagable
{
    void TakeDamage(float damage);
    void Die();
}

