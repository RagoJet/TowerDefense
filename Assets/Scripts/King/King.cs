using System;
using UnityEngine;


public enum KingState{
    Idle,
    Attacking,
    Death
}

[RequireComponent(typeof(KingAnimations))]
public class King : MonoBehaviour{
    private KingAnimations _animations;
    private KingState _state;

    private Enemy _enemyTarget;

    [SerializeField] private int maxHealth;
    private int _currentHealth;
    [SerializeField] private int damage;
    [SerializeField] private float rangeAttack;

    public event Action UIHealthEvent;

    private void Awake(){
        _currentHealth = maxHealth;
        _animations = GetComponent<KingAnimations>();
    }

    private void Update(){
        switch (_state){
            case KingState.Idle:
                if (_enemyTarget != null){
                    _state = KingState.Attacking;
                    return;
                }

                _animations.PlayIdleAnimation();
                break;
            case KingState.Attacking:
                if (_enemyTarget == null){
                    _state = KingState.Idle;
                    return;
                }

                transform.LookAt(_enemyTarget.transform, Vector3.up);
                _animations.PlayAttackAnimation();
                break;
            case KingState.Death:
                break;
        }
    }

    public void TakeDamage(int countOfDamage){
        _currentHealth -= countOfDamage;
        UIHealthEvent?.Invoke();
        if (_currentHealth <= 0){
            _state = KingState.Death;
            _animations.PlayDeadAnimation();
        }
    }


    // call in attack animation
    public void DealDamage(){
        if (_enemyTarget != null){
            _enemyTarget.TakeDamage(damage);
        }
    }

    public float GetPercentageOfHealth(){
        return (float) _currentHealth / maxHealth;
    }

    public KingState GetState(){
        return _state;
    }

    public void SetTarget(Enemy enemy){
        _enemyTarget = enemy;
    }

    public Enemy GetTarget(){
        return _enemyTarget;
    }
}