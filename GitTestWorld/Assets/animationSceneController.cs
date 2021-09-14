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
        bool walkPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        if(!isWalking && walkPressed ) {
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
        if (!walkPressed) {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    
    
    
}
