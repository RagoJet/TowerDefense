using UnityEngine;

public class Factory : MonoBehaviour{
    [SerializeField] private WeaponDescriptions weaponDescriptions;
    [SerializeField] private Cells cells;


    private LazyWeaponPool _lazyWeaponPool = new LazyWeaponPool();

    private void Awake(){
        _lazyWeaponPool.Init();
    }

    public void TryBuyRocketLauncher(){
        var cell = cells.TryGetAvailableCell();
        if (cell != null){
            CreateRocketLauncher1().SetPosition(cell);
        }
    }

    public void TryBuyBigRocketLauncher(){
        var cell = cells.TryGetAvailableCell();
        if (cell != null){
            CreateBigRocketLauncher1().SetPosition(cell);
        }
    }

    public void TryBuyCatapult(){
        var cell = cells.TryGetAvailableCell();
        if (cell != null){
            CreateCatapult1().SetPosition(cell);
        }
    }


    private Weapon CreateRocketLauncher1(){
        DataWeapon dataWeapon = new DataWeapon(TypeWeapon.RocketLauncher, 1);
        Weapon weapon = _lazyWeaponPool.TryGetWeapon(dataWeapon);
        if (weapon == null){
            weapon = CreateWeapon(dataWeapon);
        }

        return weapon;
    }

    private Weapon CreateBigRocketLauncher1(){
        DataWeapon dataWeapon = new DataWeapon(TypeWeapon.BigRocketLauncher, 1);
        Weapon weapon = _lazyWeaponPool.TryGetWeapon(dataWeapon);
        if (weapon == null){
            weapon = CreateWeapon(dataWeapon);
        }

        return weapon;
    }

    private Weapon CreateCatapult1(){
        DataWeapon dataWeapon = new DataWeapon(TypeWeapon.Catapult, 1);
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