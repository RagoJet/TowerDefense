﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum StateEnemy{
    Running,
    Attacking,
    Death
}

[RequireComponent(typeof(EnemyAnimations))]
public class Enemy : MonoBehaviour{
    [SerializeField] private ParticleSystem attackFX;
    private EnemyDescription _description;
    private EnemyAnimations _animations;
    private StateEnemy _state;
    private NavMeshAgent _agent;
    private King _target;

    private int _currentHealth;

    public event Action<Enemy> OnDie;

    private float _timeFromLastCheck;

    private int _levelOfGame;

    [SerializeField] private Image currentHealthImage;
    private Canvas _canvasHealth;

    private void Awake(){
        _agent = GetComponent<NavMeshAgent>();
        _animations = GetComponent<EnemyAnimations>();
        _canvasHealth = GetComponentInChildren<Canvas>();
    }

    public void Construct(King target, Transform theGateTransform, EnemyDescription description, int levelOfGame){
        _canvasHealth.gameObject.SetActive(true);
        _levelOfGame = levelOfGame;
        _description = description;
        transform.position = theGateTransform.position + new Vector3(Random.Range(-1f, 3f), 0f, Random.Range(-1f, -2f));
        _target = target;
        _currentHealth = _description.maxHealth * _levelOfGame;
        UpdateHealthUI(_description.enemyData.levelOfRace);
        PlayRunState();
    }


    public void Construct(King target, Transform theGateTransform, EnemyDescription description){
        _canvasHealth.gameObject.SetActive(true);
        _description = description;
        transform.position = theGateTransform.position + new Vector3(Random.Range(-1f, 3f), 0f, Random.Range(-1f, -2f));
        _target = target;
        _currentHealth = _description.maxHealth;
        UpdateHealthUI(_description.enemyData.levelOfRace);
        PlayRunState();
    }

    public void Construct(Transform theGateTransform){
        _canvasHealth.gameObject.SetActive(true);
        transform.position = theGateTransform.position + new Vector3(Random.Range(-1f, 3f), 0f, Random.Range(-1f, -2f));
        _currentHealth = _description.maxHealth;
        UpdateHealthUI(_description.enemyData.levelOfRace);
        PlayRunState();
    }

    private void UpdateHealthUI(int levelOfRace){
        switch (levelOfRace){
            case <4:
                currentHealthImage.fillAmount = (float) _currentHealth / _description.maxHealth;
                break;
            default:
                currentHealthImage.fillAmount = (float) _currentHealth / (_description.maxHealth * _levelOfGame);
                break;
        }
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
        transform.LookAt(_target.transform);
        _state = StateEnemy.Attacking;
        _animations.PlayAttackAnimation();
    }


    // call in attack animation
    public void DealDamage(){
        transform.LookAt(_target.transform);
        if (GetEnemyData().levelOfRace == 4){
            _target.TakeDamage(_description.damage * _levelOfGame);
            if (attackFX != null){
                attackFX.Play();
            }

            return;
        }

        _target.TakeDamage(_description.damage);
        if (attackFX != null){
            attackFX.Play();
        }
    }

    public void TakeDamage(int countOfDamage){
        if (_currentHealth <= 0 || _state == StateEnemy.Death) return;
        _currentHealth -= countOfDamage;

        if (_currentHealth <= 0){
            _currentHealth = 0;
            if (_target.GetState() == KingState.Idle){
                if (GetEnemyData().levelOfRace == 4){
                    Shop.Instance.AddGoldFromEnemy(GetGold() * _levelOfGame);
                }
                else{
                    Shop.Instance.AddGoldFromEnemy(GetGold());
                }
            }

            PlayDeathState();
        }
        else{
            UpdateHealthUI(_description.enemyData.levelOfRace);
        }
    }


    public void PlayDeathState(){
        _canvasHealth.gameObject.SetActive(false);
        _state = StateEnemy.Death;
        _agent.Stop();
        _animations.PlayDeathAnimation();
        StartCoroutine(LastTimeOfLiveEnemy());
    }

    IEnumerator LastTimeOfLiveEnemy(){
        yield return new WaitForSeconds(1.7f);
        OnDie?.Invoke(this);
    }


    public EnemyData GetEnemyData(){
        return _description.enemyData;
    }

    public StateEnemy GetState(){
        return _state;
    }

    public int GetGold(){
        return _description.gold;
    }
}