using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    private Animator animator;
    private Transform targetTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    private void Update()
    {
        if (targetTransform != null)
            transform.position = targetTransform.position;
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
        transform.position = targetTransform.position;
    }

    public void End()
    {
        Destroy(gameObject);
    }
}