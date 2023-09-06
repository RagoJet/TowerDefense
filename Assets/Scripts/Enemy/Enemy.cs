using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum StateEnemy{
    Running,
    Attacking,
    Death
}

[RequireComponent(typeof(EnemyAnimations))]
public class Enemy : MonoBehaviour{
    private EnemyDescription _description;
    private EnemyAnimations _animations;
    private StateEnemy _state;
    private King _target;

    private int _currentHealth;


    public event Action<Enemy> OnDie;

    private void Awake(){
        _animations = GetComponent<EnemyAnimations>();
    }

    public EnemyData GetLevelData(){
        return _description.enemyData;
    }

    public void Construct(King target, Transform theGateTransform, EnemyDescription description){
        _description = description;
        transform.position = theGateTransform.position + new Vector3(Random.Range(-2, 2), 0f, Random.Range(-1, -2));
        _target = target;
        _state = StateEnemy.Running;
        _currentHealth = _description.maxHealth;
    }

    public void Construct(King target, Transform theGateTransform){
        transform.position = theGateTransform.position + new Vector3(Random.Range(-2, 2), 0f, Random.Range(-1, -2));
        _target = target;
        _state = StateEnemy.Running;
        _currentHealth = _description.maxHealth;
        transform.forward = theGateTransform.forward;
    }

    private void Update(){
        if (_state == StateEnemy.Running){
            Vector3 direction = new Vector3(_target.transform.position.x, 0f, _target.transform.position.z);
            transform.Translate(direction * _description.moveSpeed * Time.deltaTime, Space.World);
            if ((_target.transform.position - transform.position).magnitude < _description.rangeAttack){
                _state = StateEnemy.Attacking;
                _animations.PlayAttackAnimation();
                transform.LookAt(_target.transform, Vector3.up);
            }
        }

        if (_state == StateEnemy.Attacking){
            if (_target.GetState() == KingState.Idle){
                _target.SetTarget(this);
            }
        }
    }

    // call in attack animation
    public void DealDamage(){
        if (_target.GetState() != KingState.Death){
            _target.TakeDamage(_description.damage);
        }
    }

    public void TakeDamage(int countOfDamage){
        _currentHealth -= countOfDamage;
        if (_currentHealth <= 0){
            Die();
        }
    }

    private void Die(){
        _state = StateEnemy.Death;
        _animations.PlayDeathAnimation();
        ResetSelfFromTarget();
        StartCoroutine(LifeTimeOfEnemy());
    }

    public void ResetSelfFromTarget(){
        if (_target.GetTarget() == this){
            _target.SetTarget(null);
        }
    }

    IEnumerator LifeTimeOfEnemy(){
        yield return new WaitForSeconds(2f);
        OnDie?.Invoke(this);
    }

    public StateEnemy GetState(){
        return _state;
    }
}