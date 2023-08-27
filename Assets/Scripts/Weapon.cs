using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour{
    private WeaponDescription _description;

    private Cell _cell;
    private Factory _factory;
    private Collider _collider;

    public void Construct(WeaponDescription description, Factory factory, Cell cell){
        _description = description;
        _factory = factory;
        OccupyTheCell(cell);
    }

    public void Construct(Cell cell){
        OccupyTheCell(cell);
    }


    private void Awake(){
        _collider = GetComponent<Collider>();
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


    public DataWeapon GetDataWeapon(){
        return _description._dataWeapon;
    }

    private void OnMouseDrag(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        int islandLayerMask = 1 << LayerMask.NameToLayer("Island");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, islandLayerMask)){
            if (hit.collider.TryGetComponent(out Island island)){
                Vector3 newPosition = new Vector3(hit.point.x, island.transform.position.y + 4f, hit.point.z);
                gameObject.transform.position = newPosition;
            }
        }
    }

    private void OnMouseUp(){
        _collider.enabled = false;
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.down * 1.5f, transform.localScale / 2f,
            Quaternion.identity);
        foreach (var colllider in colliders){
            if (colllider.TryGetComponent(out Weapon weapon)){
                _collider.enabled = true;
                if (this.GetDataWeapon().Equals(weapon.GetDataWeapon())){
                    this.FreeTheCell();
                    _factory.MergeWeapons(this, weapon, weapon._cell);
                }
                else{
                    ReturnPositionToCell();
                }

                return;
            }
        }

        foreach (var colllider in colliders){
            if (colllider.TryGetComponent(out Cell cell)){
                FreeTheCell();
                OccupyTheCell(cell);
                _collider.enabled = true;
                return;
            }
        }

        ReturnPositionToCell();
        _collider.enabled = true;
    }
}