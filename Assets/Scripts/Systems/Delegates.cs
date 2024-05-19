using System.Collections;
using UnityEngine;

// Delegate usage example: exampleDelegate?.Invoke()

namespace Assets.Scripts.Systems
{
    public class Delegates : MonoBehaviour
    {

        public delegate void OhHit(string type, int damage, GameObject obj);


    }
}