using System;
using UnityEngine;

public enum KingState{
    Idle,
    Attacking,
    Death
}

[RequireComponent(typeof(KingAnimations))]
public class King : MonoBehaviour, ISaveable{
    private GameManager _gameManager;
    private KingAnimations _animations;
    private KingState _state;

    private Enemy _enemyTarget;

    private int _maxHealth;
    private int _currentHealth;
    private int _damage;

    private int _rangeAttackPowTwo = 9;

    public event Action UIHealthEvent;

    private DataContainer _dataContainer;

    public void Construct(GameManager gameManager){
        _gameManager = gameManager;
        _animations = GetComponent<KingAnimations>();
        _currentHealth = _maxHealth;
        _state = KingState.Idle;
    }

    public void Refresh(){
        _currentHealth = _maxHealth;
        UIHealthEvent.Invoke();
        _enemyTarget = null;
        if (_state != KingState.Idle){
            _state = KingState.Idle;
            _animations.PlayIdleAnimation();
        }
    }

    private void Update(){
        switch (_state){
            case KingState.Idle:
                if (_enemyTarget != null){
                    _state = KingState.Attacking;
                    _animations.PlayAttackAnimation();
                }

                break;
            case KingState.Attacking:
                if (_enemyTarget == null || _enemyTarget.GetState() == StateEnemy.Death){
                    _enemyTarget = null;
                    _state = KingState.Idle;
                    _animations.PlayIdleAnimation();
                }

                break;
            case KingState.Death:
                break;
        }
    }


    public void Aggro(Enemy enemy){
        if (_state == KingState.Idle){
            if ((transform.position - enemy.transform.position).sqrMagnitude < _rangeAttackPowTwo){
                _enemyTarget = enemy;
            }
        }
    }

    // call in attack animation
    public void DealDamage(){
        if (_enemyTarget == null || _enemyTarget.GetState() == StateEnemy.Death) return;
        transform.LookAt(_enemyTarget.transform, Vector3.up);
        _enemyTarget.TakeDamage(_damage);
    }

    public void TakeDamage(int countOfDamage){
        if (_state == KingState.Death) return;
        _currentHealth -= countOfDamage;
        UIHealthEvent?.Invoke();
        if (_currentHealth <= 0){
            _gameManager.StopGame();
            _state = KingState.Death;
            _enemyTarget = null;
            _animations.PlayDeadAnimation();
        }
    }


    public void AddDamage(int addDamage){
        _damage += addDamage;
    }

    public void AddHealth(int health){
        _maxHealth += health;
        if (_state != KingState.Death){
            _currentHealth += health;
        }

        UIHealthEvent?.Invoke();
    }

    public KingState GetState(){
        return _state;
    }

    public String GetStringHealthAndMaxHealth(){
        return _currentHealth + "/" + _maxHealth;
    }

    public float GetPercentageOfHealth(){
        return (float) _currentHealth / _maxHealth;
    }

    public void WriteDataToContainer(){
        _dataContainer.maxHealthKing = _maxHealth;
        _dataContainer.damageKing = _damage;
    }

    public void LoadDataFromContainer(){
        _maxHealth = _dataContainer.maxHealthKing;
        _damage = _dataContainer.damageKing;
    }

    public void SetDataContainer(DataContainer dataContainer){
        _dataContainer = dataContainer;
    }
}