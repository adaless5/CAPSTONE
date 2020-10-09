using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable<T>
{
    void TakeDamage(T amountOfDamage);

    void Die();


}
