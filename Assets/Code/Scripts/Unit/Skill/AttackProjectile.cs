using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    private PieceUnit target;
    private int damage;
    private float speed;
    public GameObject ExplosionParticle;

    public void Init(PieceUnit target, int damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(damage);
            ExplosionParticle.SetActive(true);
            Destroy(gameObject, 1.0f);
            Destroy(this);
        }
    }
}
