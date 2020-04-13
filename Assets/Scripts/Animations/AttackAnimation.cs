using System;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    private Animator animator;
    private Action endAttack;
    private Transform targetTransform;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (targetTransform != null)
            transform.position = targetTransform.position;
    }

    public void Attack(Transform target, Action action)
    {
        targetTransform = target;
        transform.position = targetTransform.position;

        gameObject.SetActive(true);
        animator.SetTrigger("Attack");

        endAttack = action;
    }

    public void End()
    {
        targetTransform = null;
        gameObject.SetActive(false);
        endAttack?.Invoke();
        endAttack = null;
    }
}