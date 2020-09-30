using UnityEngine;

//VR - This class is a damage system for any target/enemy
public class Target : MonoBehaviour
{
    [Header("Health Settings")]
    public float m_HP = 50.0f;

    public void TakeDamage (float damage)
    {
        m_HP -= damage;
        if (m_HP <= 0.0f)
        {
            Die();
        }
    }

    //VR - Currently destroying gameObjects when damaged, can look into creating object pool for recyclable assets later on
    void Die()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
