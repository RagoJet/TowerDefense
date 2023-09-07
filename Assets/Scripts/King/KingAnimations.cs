using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KingAnimations : MonoBehaviour{
    private Animator _animator;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");

    private void Start(){
        _animator = GetComponent<Animator>();
    }

    public void PlayIdleAnimation(){
        _animator.SetTrigger(Idle);
    }


    public void PlayAttackAnimation(){
        _animator.SetTrigger(Attack);
    }


    public void PlayDeadAnimation(){
        _animator.SetTrigger(Death);
    }
}