using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currHealth { get; private set; }
    private Animator anim;
    private Rigidbody2D rb;

    public float knockbackForce;
    public float knockbackForceUp;

    void Awake()
    {
        currHealth = startingHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //Debug.Log("Current Health: " + currHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(1);
            Debug.Log("DevMode: Press 1 for Player Taking Damage!");
        }
    }

    public void TakeDamage(float _damage)
    {
        currHealth = Mathf.Clamp(currHealth - _damage, 0, startingHealth);

        if (currHealth > 0)
        {
            anim.SetTrigger("Stun");
            Knockback();
        }
        else
        {
            SoundManager.instance.PlayerDeathSFX();
            anim.SetTrigger("Die");
            GetComponent<PlayerController>().enabled = false;
        }
    }

    public void AddHealth(float _value)
    {
        currHealth = Mathf.Clamp(currHealth + _value, 0, startingHealth);
    }

    public void Knockback()
    {
        Transform attacker = getDamageSource();
        Vector2 knockbackDir = new Vector2(transform.position.x - attacker.transform.position.x, 0);
        rb.velocity = new Vector2(knockbackDir.x, knockbackForceUp) * knockbackForce;
    }

    public Transform getDamageSource()
    {
        GameObject[] DamageSources = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform currDamageSource = null;

        foreach (GameObject go in DamageSources)
        {
            float currDistance;
            currDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currDistance < closestDistance)
            {
                closestDistance = currDistance;
                currDamageSource = go.transform;
            }
        }
        return currDamageSource;
    }
}
