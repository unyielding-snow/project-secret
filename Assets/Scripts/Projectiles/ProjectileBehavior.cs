using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float Speed = 3.0f;
    public bool sourceEnemy = true;

    // Only damage stuff on this layer 
    public LayerMask damageLayers;

    private void Update()
    {
        transform.position += -transform.right * Time.deltaTime * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         *  Hardcoding object type for now based on name,
         *  in the future check based on a specific tag?
         *  (players, walls/border hitboxes, enemies)
         */
        if (sourceEnemy && collision.gameObject.name == "Player")
        {
            DoDamage(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void DoDamage(GameObject target)
    {
        //Check if collision is included in layermask
        if ((damageLayers.value & 1 << target.layer) != 0)
        {
            //Find health component on collided object
            HealthSystem health = target.GetComponent<HealthSystem>();
            if (health)
            {
                health.Damage(10, target);
                Debug.Log("Damage Applied");
            }
        }
    }

}
