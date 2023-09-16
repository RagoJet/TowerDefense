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

    public event Action<Enemy, bool> OnDie;

    private float _timeFromLastCheck;

    private void Awake(){
        _agent = GetComponent<NavMeshAgent>();
        _animations = GetComponent<EnemyAnimations>();
    }


    public void Construct(King target, Transform theGateTransform, EnemyDescription description){
        _description = description;
        transform.position = theGateTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-1f, -2f));
        _target = target;
        _currentHealth = _description.maxHealth;
        PlayRunState();
    }

    public void Construct(Transform theGateTransform){
        transform.position = theGateTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-1f, -2f));
        _currentHealth = _description.maxHealth;
        PlayRunState();
    }

    private void Update(){
        if (_state == StateEnemy.Running){
            if (Time.time - _timeFromLastCheck < 0.25f) return;
            if (_agent.remainingDistance <= _agent.stoppingDistance){
                _timeFromLastCheck = Time.time;
                PlayAttackState();
            }
        }
    }


    public void PlayRunState(){
        _state = StateEnemy.Running;
        _agent.SetDestination(_target.transform.position);
        // play default (run) animations
    }

    public void PlayAttackState(){
        _state = StateEnemy.Attacking;
        _animations.PlayAttackAnimation();
    }


    // call in attack animation
    public void DealDamage(){
        _target.TakeDamage(_description.damage);
    }

    public void TakeDamage(int countOfDamage){
        if (_currentHealth <= 0 || _state == StateEnemy.Death) return;
        _currentHealth -= countOfDamage;
        if (_currentHealth <= 0){
            PlayDeathState();
        }
    }


    public void PlayDeathState(){
        _state = StateEnemy.Death;
        _agent.Stop();
        _animations.PlayDeathAnimation();
        StartCoroutine(LastTimeOfLiveEnemy());
    }

    IEnumerator LastTimeOfLiveEnemy(){
        var willBeGold = (_target.GetState() != KingState.Death);
        yield return new WaitForSeconds(1.8f);
        OnDie?.Invoke(this, willBeGold);
    }


    public EnemyData GetLevelData(){
        return _description.enemyData;
    }

    public StateEnemy GetState(){
        return _state;
    }

    public int GetGold(){
        return _description.gold;
    }
}