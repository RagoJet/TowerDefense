using System;
using UnityEngine;

public enum KingState{
    Idle,
    Death
}

[RequireComponent(typeof(KingAnimations))]
public class King : MonoBehaviour, ISaveable{
    [SerializeField] private ParticleSystem deathKingFx;
    [SerializeField] private ParticleSystem reincarnationKingFx;
    [SerializeField] private ParticleSystem addHealthFX;
    private GameManager _gameManager;
    private KingAnimations _animations;
    private KingState _state;

    private Enemy _enemyTarget;

    private int _maxHealth;
    private int _currentHealth;


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
            reincarnationKingFx.Play();
        }
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
            deathKingFx.Play();
        }
    }


    public void AddHealth(int health){
        _maxHealth += health;
        if (_state != KingState.Death){
            _currentHealth += health;
        }

        UIHealthEvent?.Invoke();
        addHealthFX.Play();
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
    }

    public void LoadDataFromContainer(){
        _maxHealth = _dataContainer.maxHealthKing;
    }

    public void SetDataContainer(DataContainer dataContainer){
        _dataContainer = dataContainer;
    }
}