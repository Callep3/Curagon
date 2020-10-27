using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuragonAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;


    void Awake()
    {//Get the animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Eat");
        }

       if(Input.GetMouseButton)
    }
}
