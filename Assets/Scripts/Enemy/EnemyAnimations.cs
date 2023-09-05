using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimations : MonoBehaviour{
    private Animator _animator;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    private void Awake(){
        _animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation(){
        _animator.SetTrigger(Attack);
    }

    public void PlayDeathAnimation(){
        _animator.SetTrigger(Die);
    }
}