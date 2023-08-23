using UnityEngine;

public class Factory : MonoBehaviour{
    [SerializeField] private WeaponDescriptions weaponDescriptions;
    [SerializeField] private Cells cells;

    private readonly LazyWeaponPool _lazyWeaponPool = new LazyWeaponPool();

    private void Awake(){
        _lazyWeaponPool.Init();
    }

    public void TryBuyRocketLauncher(){
        if (cells.TryGetCell(out var cell)){
            CreateFirstLevelWeapon(TypeWeapon.RocketLauncher).OccupyTheCage(cell);
        }
    }

    public void TryBuyBigRocketLauncher(){
        if (cells.TryGetCell(out var cell)){
            CreateFirstLevelWeapon(TypeWeapon.BigRocketLauncher).OccupyTheCage(cell);
        }
    }

    public void TryBuyCatapult(){
        if (cells.TryGetCell(out var cell)){
            CreateFirstLevelWeapon(TypeWeapon.Catapult).OccupyTheCage(cell);
        }
    }


    private Weapon CreateFirstLevelWeapon(TypeWeapon typeWeapon){
        DataWeapon dataWeapon = new DataWeapon(typeWeapon, 1);
        Weapon weapon = _lazyWeaponPool.TryGetWeapon(dataWeapon);
        if (weapon == null){
            weapon = CreateWeapon(dataWeapon);
        }

        return weapon;
    }

    private Weapon CreateWeapon(DataWeapon dataWeapon){
        int index = dataWeapon._level - 1;
        Weapon weapon;
        switch (dataWeapon._typeWeapon){
            case TypeWeapon.RocketLauncher:
                weapon = Instantiate(weaponDescriptions.ListRocketLauncher[index].weaponPrefab);
                weapon.Construct(weaponDescriptions.ListRocketLauncher[index]);
                return weapon;

            case TypeWeapon.BigRocketLauncher:
                weapon = Instantiate(weaponDescriptions.ListBigRocketLauncher[index].weaponPrefab);
                weapon.Construct(weaponDescriptions.ListBigRocketLauncher[index]);
                return weapon;

            case TypeWeapon.Catapult:
                weapon = Instantiate(weaponDescriptions.ListCatapult[index].weaponPrefab);
                weapon.Construct(weaponDescriptions.ListCatapult[index]);
                return weapon;

            default: return null;
        }
    }
}