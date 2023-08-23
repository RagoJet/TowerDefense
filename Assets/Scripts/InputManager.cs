using UnityEngine;

public class InputManager : MonoBehaviour{
    [SerializeField] private Camera gameCamera;

    private Ray _ray;
    private RaycastHit _hit;

    private Weapon _mouseCursorWeapon;

    private void Update(){
        if (Input.GetMouseButtonDown(0)){
            _ray = gameCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit)){
                if (_hit.collider.TryGetComponent(out Weapon weapon)){
                    _mouseCursorWeapon = weapon;
                }
            }
        }

        if (Input.GetMouseButton(0) && _mouseCursorWeapon != null){
            _ray = gameCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit)){
                if (_hit.collider.TryGetComponent(out Island island)){
                    Vector3 newPosition = _hit.point;
                    _mouseCursorWeapon.transform.position = newPosition;
                }

                if (_hit.collider.TryGetComponent(out Cell cell)){
                    Vector3 newPosition = new Vector3(_hit.point.x, cell.GetPosition().y, _hit.point.z);
                    _mouseCursorWeapon.transform.position = newPosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)){
            _mouseCursorWeapon = null;
        }
    }
}