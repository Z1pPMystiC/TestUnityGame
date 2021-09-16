using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationSceneController : MonoBehaviour
{

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool wPressed = Input.GetKey("w");
        bool aPressed = Input.GetKey("a");
        bool sPressed = Input.GetKey("s");
        bool dPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");
        if(!isWalking && (wPressed || aPressed || sPressed || dPressed) ) {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        if(!isRunning && runPressed) {
            animator.SetBool("isRunning", true);
        }
        if (isRunning && !runPressed)
        {
            animator.SetBool("isRunning", false);
        }
        if (!(wPressed || aPressed || sPressed || dPressed)) {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    
    
    
}
