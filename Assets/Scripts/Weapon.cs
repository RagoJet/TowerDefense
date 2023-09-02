using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour{
    private WeaponDescription _description;

    private Cell _cell;
    private Factory _factory;

    public void Construct(WeaponDescription description, Factory factory, Cell cell){
        _description = description;
        _factory = factory;
        OccupyTheCell(cell);
    }

    public void Construct(Cell cell){
        OccupyTheCell(cell);
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
                    ref Cell tempCell = ref _cell;
                    // this.FreeTheCell();
                    if (_factory.TryMergeWeapons(this, weapon, weapon._cell) == false){
                        // OccupyTheCell(_cell);
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