using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAttackState : State
    {

        public override void Enter()
        {
            Debug.Log("Attack State");
        }

        public override void Do()
        {

        }
    }
}