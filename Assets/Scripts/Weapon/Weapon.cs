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
    private WeaponsFactory _factory;
    private Cell _cell;

    private WeaponDescription _description;
    private WeaponMode _weaponMode;


    private List<Enemy> _listOfEnemies;
    private Enemy _aimTarget;

    private float _timeFromLastAttack;


    public void Construct(WeaponDescription description, WeaponsFactory factory, Cell cell, List<Enemy> list){
        _weaponMode = WeaponMode.On;
        _description = description;
        _factory = factory;
        OccupyTheCell(cell);
        _listOfEnemies = list;
    }

    public void Construct(Cell cell){
        _weaponMode = WeaponMode.On;
        OccupyTheCell(cell);
    }

    private void Update(){
        switch (_weaponMode){
            case WeaponMode.Off:
                break;
            case WeaponMode.On:
                if (TryChooseATarget()){
                    Attack();
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
            transform.LookAt(_aimTarget.transform, Vector3.up);
            if (Time.time - _timeFromLastAttack >= _description.attackDelay){
                _aimTarget.TakeDamage(_description.damage);
                transform.DOShakeScale(_description.attackDelay * 0.7f, 0.1f);
                _timeFromLastAttack = Time.time;
            }
        }
    }

    private void OnMouseDown(){
        _weaponMode = WeaponMode.Off;
    }

    private void OnMouseDrag(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        int islandLayerMask = 1 << LayerMask.NameToLayer("Island");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, islandLayerMask)){
            if (hit.collider.TryGetComponent(out Island island)){
                Vector3 newPosition = new Vector3(hit.point.x, island.transform.position.y + 5f, hit.point.z);
                gameObject.transform.position = newPosition;
            }
        }
    }

    private void OnMouseUp(){
        _weaponMode = WeaponMode.On;
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
                else if (GetLevelWeapon() == targetCell.GetWeaponOfThisCell().GetLevelWeapon()){
                    if (_factory.TryMergeWeapons(this, targetCell.GetWeaponOfThisCell(), targetCell) == false){
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
        transform.position = _cell.GetPosition();
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

    public int GetLevelWeapon(){
        return _description.level;
    }

    private void OnDisable(){
        FreeTheCell();
    }
}