using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
    public class AttackHitbox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Enemy entered the hitbox!");

                GameObject enemy = other.gameObject;
                EnemyController enemyController = enemy.GetComponent<EnemyController>();

                if( enemyController != null) 
                {
                    enemyController.OnHit();
                }
                else
                {
                    Debug.LogError("Enemy Controller On Hit Not Found");
                }
            }
        }
    }
}