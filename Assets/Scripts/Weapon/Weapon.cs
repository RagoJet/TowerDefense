using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum WeaponMode{
    Off,
    On
}

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour{
    [SerializeField] private ParticleSystem[] attacksFX;
    private WeaponsFactory _factory;
    private Cell _cell;

    private WeaponDescription _description;
    public WeaponMode weaponMode;


    private List<Enemy> _listOfEnemies;
    private Enemy _aimTarget;

    private float _timeFromLastAttack;

    private GameManager _gameManager;

    private Tween _tween;
    private LineRenderWeapon _lineRendererWeapon;

    int _islandLayerMask;

    public void Construct(WeaponDescription description, WeaponsFactory factory, Cell cell, List<Enemy> list,
        GameManager gameManager){
        _description = description;
        _factory = factory;
        _listOfEnemies = list;
        _gameManager = gameManager;
        transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y + 15f,
            cell.transform.position.z);
        OccupyTheCell(cell);
        _lineRendererWeapon = LineRenderWeapon.Instance;
        _islandLayerMask = 1 << LayerMask.NameToLayer("Island");
    }

    public void Construct(Cell cell){
        transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y + 15f,
            cell.transform.position.z);
        OccupyTheCell(cell);
    }


    private void Update(){
        switch (weaponMode){
            case WeaponMode.Off:
                break;
            case WeaponMode.On:
                if (_gameManager.state == GameState.Playing){
                    if (TryChooseATarget()){
                        Attack();
                    }
                }

                break;
        }
    }

    private bool TryChooseATarget(){
        if (_aimTarget == null || _aimTarget.GetState() == StateEnemy.Death){
            _aimTarget = null;
            float shortestLenght = Single.MaxValue;
            foreach (var enemy in _listOfEnemies){
                if (enemy.GetState() != StateEnemy.Death){
                    float lenght = (transform.position - enemy.transform.position).sqrMagnitude;
                    if (lenght < shortestLenght){
                        shortestLenght = lenght;
                        _aimTarget = enemy;
                    }
                }
            }
        }

        return _aimTarget != null;
    }

    private void Attack(){
        if (_aimTarget != null && _aimTarget.GetState() != StateEnemy.Death){
            Vector3 direction = _aimTarget.transform.position - transform.position;
            direction.y = 0;
            transform.forward = direction;
            if (Time.time - _timeFromLastAttack >= _description.attackDelay){
                _aimTarget.TakeDamage(_description.damage);
                transform.DOShakeScale(_description.attackDelay * 0.7f, 0.1f);
                _timeFromLastAttack = Time.time;
                AudioManager.Instance.PlayShotSound(_description.level);
                foreach (var weaponParticleSystem in attacksFX){
                    weaponParticleSystem.Play();
                }
            }
        }
    }

    private void OnMouseDown(){
        weaponMode = WeaponMode.Off;
        _tween = transform.DOMoveY(_cell.transform.position.y + 4f, 0.5f);
        _lineRendererWeapon.gameObject.SetActive(true);
        _lineRendererWeapon.transform.position = new Vector3(transform.position.x, transform.position.y - 3.5f,
            transform.position.z);
    }

    private void OnMouseDrag(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _islandLayerMask)){
            if (hit.collider.TryGetComponent(out Island island)){
                Vector3 newPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                gameObject.transform.position = newPosition;
                _lineRendererWeapon.transform.position = new Vector3(transform.position.x, transform.position.y - 3.5f,
                    transform.position.z);
            }
        }
    }

    private void OnMouseUp(){
        _lineRendererWeapon.gameObject.SetActive(false);
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = Vector3.down;
        RaycastHit hit;

        int cellLayerMask = 1 << LayerMask.NameToLayer("CellWeapon");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayerMask)){
            if (hit.collider.TryGetComponent(out Cell targetCell)){
                if (targetCell == _cell){
                    ReturnPositionToCell();
                }
                else if (targetCell.IsAvailable()){
                    FreeTheCell();
                    OccupyTheCell(targetCell);
                }
                else if (GetLevel() == targetCell.GetWeapon().GetLevel()){
                    if (_factory.TryMergeWeapons(this, targetCell.GetWeapon(), targetCell) == false){
                        ReturnPositionToCell();
                    }
                }
                else{
                    ReturnPositionToCell();
                }
            }
        }
        else{
            ReturnPositionToCell();
        }
    }

    private void ReturnPositionToCell(){
        _tween?.Kill();
        transform.DOMove(_cell.GetPosition(), 0.2f).OnComplete(() => { weaponMode = WeaponMode.On; });
    }

    private void OccupyTheCell(Cell cell){
        _cell = cell;
        _cell.MakeOccupied(this);
        ReturnPositionToCell();
    }

    private void FreeTheCell(){
        _cell.MakeAvailable();
        _cell = null;
    }

    public int GetLevel(){
        return _description.level;
    }

    private void OnDisable(){
        FreeTheCell();
    }
}