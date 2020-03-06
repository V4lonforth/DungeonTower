using System;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private Animator animator;
    private Action endAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Attack(Vector3 position, Action action)
    {
        transform.position = position;
        transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 4) * 90f);
        gameObject.SetActive(true);
        animator.SetTrigger("Attack");

        endAttack = action;
    }

    public void End()
    {
        gameObject.SetActive(false);
        endAttack?.Invoke();
    }
}