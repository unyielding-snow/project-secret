using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
    public class SwordHitbox : MonoBehaviour
    {
        public UnityEvent EnemyHit;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Enemy entered the hitbox!");

                EnemyHit.Invoke();

                //EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
                //enemyController.OnHit();
            }
        }
    }
}