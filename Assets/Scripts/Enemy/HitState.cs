using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class HitState : State
    {
        public SpriteRenderer spriteRenderer;
        public Material flashMaterial;
        private Material originalMaterial;

        public override void Enter()
        {
            Debug.Log("Enemy enter Hitstate");

            startTime = Time.time;
            isComplete = false;
            originalMaterial = spriteRenderer.material;
            spriteRenderer.material = flashMaterial;

        }

        public override void Do()
        {
            Debug.Log("Doing Hit State");
            if (time >= 0.3f)
            {
                Debug.Log("Time Finished");
                Exit();
            }
        }

        public override void Exit()
        {
            Debug.Log("Enemy exit Hitstate");
            spriteRenderer.material = originalMaterial;
            isComplete = true;
        }
    }
}