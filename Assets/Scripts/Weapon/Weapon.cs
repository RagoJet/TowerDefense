using System;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponMode{
    Off,
    On
}

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour{
    private WeaponDescription _description;

    private WeaponMode _weaponMode;
    private Cell _cell;
    private WeaponsFactory _factory;

    private List<Enemy> _listOfEnemies;
    private Enemy _aimTarget;

    public void Construct(WeaponDescription description, WeaponsFactory factory, Cell cell, List<Enemy> list){
        _weaponMode = WeaponMode.On;
        _description = description;
        _factory = factory;
        OccupyTheCell(cell);
        _listOfEnemies = list;
    }

    public void Construct(Cell cell){
        OccupyTheCell(cell);
    }

    private void Update(){
        switch (_weaponMode){
            case WeaponMode.Off:
                break;
            case WeaponMode.On:
                FindATarget();
                break;
        }
        
    }

    private void FindATarget(){
        if (_aimTarget != null){
            transform.LookAt(_aimTarget.transform, Vector3.up);
        }
        else{
            ChooseATarget();
        }
    }

    private void ChooseATarget(){
        Enemy closestEnemy = null;
        float shortestLenght = Single.MaxValue;
        foreach (var enemy in _listOfEnemies){
            float lenght = (transform.position - enemy.transform.position).magnitude;
            if (lenght < shortestLenght){
                shortestLenght = lenght;
                closestEnemy = enemy;
            }
        }

        _aimTarget = closestEnemy;
    }

    private void Attack(){
        if (_aimTarget != null){
            _aimTarget.TakeDamage(_description.damage);
        }
    }

    private void OnDisable(){
        FreeTheCell();
    }

    private void ReturnPositionToCell(){
        transform.position = _cell.GetPosition();
    }

    private void OccupyTheCell(Cell cell){
        _cell = cell;
        transform.position = _cell.GetPosition();
        cell.MakeOccupied();
    }

    private void FreeTheCell(){
        _cell.MakeAvailable();
        _cell = null;
    }

    public int GetLevelWeapon(){
        return _description.level;
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

        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

        foreach (RaycastHit hit in hits){
            if (hit.collider.TryGetComponent(out Weapon weapon)){
                if (weapon == this){
                    continue;
                }

                if (this.GetLevelWeapon() == weapon.GetLevelWeapon()){
                    if (_factory.TryMergeWeapons(this, weapon, weapon._cell) == false){
                        ReturnPositionToCell();
                    }
                }
                else{
                    ReturnPositionToCell();
                }

                return;
            }
        }

        foreach (RaycastHit hit in hits){
            if (hit.collider.TryGetComponent(out Cell cell)){
                FreeTheCell();
                OccupyTheCell(cell);
                return;
            }
        }

        ReturnPositionToCell();
    }
}