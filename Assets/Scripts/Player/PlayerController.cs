using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    State state;

    void Update() {
        CheckInput();
    }

    void CheckInput(){
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Verticle");
    }
}

