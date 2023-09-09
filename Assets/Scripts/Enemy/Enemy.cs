using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
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
    private NavMeshAgent _agent;
    private King _target;

    private int _currentHealth;


    public event Action<Enemy> OnDie;

    private void Awake(){
        _agent = GetComponent<NavMeshAgent>();
        _animations = GetComponent<EnemyAnimations>();
    }

    public EnemyData GetLevelData(){
        return _description.enemyData;
    }

    public void Construct(King target, Transform theGateTransform, EnemyDescription description){
        _description = description;
        transform.position = theGateTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-1f, -2f));
        _target = target;
        _state = StateEnemy.Running;
        _currentHealth = _description.maxHealth;
        _agent.SetDestination(_target.transform.position);
    }

    public void Construct(Transform theGateTransform){
        transform.position = theGateTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-1f, -2f));
        _state = StateEnemy.Running;
        _currentHealth = _description.maxHealth;
        _agent.SetDestination(_target.transform.position);
    }

    private void Update(){
        switch (_state){
            case StateEnemy.Running:
                if (_agent.remainingDistance <= _agent.stoppingDistance){
                    _state = StateEnemy.Attacking;
                    _animations.PlayAttackAnimation();
                }

                break;
            case StateEnemy.Attacking:
                if (_target.GetState() == KingState.Idle){
                    _target.SetTarget(this);
                }

                break;
            case StateEnemy.Death:
                _agent.Stop();
                break;
        }
    }

    // call in attack animation
    public void DealDamage(){
        if (_target.GetState() != KingState.Death){
            _target.TakeDamage(_description.damage);
        }
    }

    public void TakeDamage(int countOfDamage){
        if (_currentHealth > 0){
            _currentHealth -= countOfDamage;
            if (_currentHealth <= 0 && _state != StateEnemy.Death){
                Die();
            }
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

    public int GetGold(){
        return _description.gold;
    }
}