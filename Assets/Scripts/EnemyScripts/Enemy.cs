using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState m_currentState;

    [Header("Stats")]
    public FloatValue m_maxHealth;
    public float m_health;
    public string m_name;
    public int m_baseAttack;
    public float m_moveSpeed;

    [Header("Death Effects")]
    public GameObject m_deathEffect;
    private const float m_deathEffectDelay = 1.0f;

    // Start is called before the first frame update
    void Awake()
    {
        m_health = m_maxHealth.RuntimeValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void TakeDamage(float damage)
    {
        m_health -= damage;
        if(m_health<=0f)
        {
            DeathEffect();
            this.gameObject.SetActive(false);
        }
    }

    private void DeathEffect()
    {
        if(m_deathEffect != null)
        {
            GameObject effect = Instantiate(m_deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, m_deathEffectDelay);
        }
    }

    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if(myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            m_currentState = EnemyState.idle;
        }
    }
}
