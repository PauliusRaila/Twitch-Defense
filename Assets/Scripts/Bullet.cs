using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    Transform target;
    public float speed = 10f;
    
    public GameObject impactEffect;

    [HideInInspector]
    public float damage = 0f;

    [HideInInspector]
    public float explosionRadius = 0f;

    public void Seek (Transform _target) {
        target = _target;
	}
	
	
	void Update () {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
          


	}

    private void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else {

            Damage(target);
        }


      

        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders) {
            if (collider.tag == "Enemy") {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy) {
        if (enemy.GetComponent<EnemyNPC>() != null)
            enemy.GetComponent<EnemyNPC>().decreaseHealth(damage);
        else
            enemy.GetComponent<BossNPC>().decreaseHealth(damage);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
