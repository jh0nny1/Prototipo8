using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour {

    private         Rigidbody rb;

    public Animator animator;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {    
        if (Input.GetKey(KeyCode.W)) { 
            animator.SetBool("Walk", true);
            animator.SetBool("SprintJump", false);
            animator.SetBool("SprintSlide", false);
        }
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("Walk", false);
            animator.SetBool("SprintJump", false);
            animator.SetBool("SprintSlide", false);
        }
    }
}
