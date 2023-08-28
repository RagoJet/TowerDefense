using UnityEngine;

public class Factory : MonoBehaviour{
    [SerializeField] private WeaponDescriptions weaponDescriptions;
    [SerializeField] private Cells cells;
    [SerializeField] private int maxLevelWeapons = 4;
    [SerializeField] private int maxLevelType = 3;

    private readonly LazyWeaponPool _lazyWeaponPool = new LazyWeaponPool();

    private void Awake(){
        _lazyWeaponPool.Init();
    }

    private void HideWeapon(Weapon weapon){
        _lazyWeaponPool.HideWeapon(weapon);
    }

    public void TryBuyWeapon(){
        if (cells.TryGetCell(out var cell)){
            CreateFirstLevelWeapon(cell);
        }
    }

    private Weapon CreateFirstLevelWeapon(Cell cell){
        DataWeapon dataWeapon = new DataWeapon((TypeWeapon) 1, 1);
        Weapon weapon = _lazyWeaponPool.TryGetWeapon(dataWeapon);
        if (weapon == null){
            weapon = CreateWeapon(dataWeapon, cell);
        }

        weapon.Construct(cell);
        return weapon;
    }

    private Weapon CreateWeapon(DataWeapon dataWeapon, Cell cell){
        int index = dataWeapon._level - 1;
        Weapon weapon;
        switch (dataWeapon._typeWeapon){
            case TypeWeapon.Catapult:
                weapon = Instantiate(weaponDescriptions.ListCatapult[index].weaponPrefab);
                weapon.Construct(weaponDescriptions.ListCatapult[index], this, cell);
                break;
            case TypeWeapon.RocketLauncher:
                weapon = Instantiate(weaponDescriptions.ListRocketLauncher[index].weaponPrefab);
                weapon.Construct(weaponDescriptions.ListRocketLauncher[index], this, cell);
                break;

            case TypeWeapon.BigRocketLauncher:
                weapon = Instantiate(weaponDescriptions.ListBigRocketLauncher[index].weaponPrefab);
                weapon.Construct(weaponDescriptions.ListBigRocketLauncher[index], this, cell);
                break;
            default: return null;
        }

        return weapon;
    }

    public Weapon MergeWeapons(Weapon weapon1, Weapon weapon2, Cell cell){
        DataWeapon dataWeapon = weapon1.GetDataWeapon();

        TypeWeapon typeWeapon = dataWeapon._typeWeapon;
        int newLevel = dataWeapon._level + 1;

        if (newLevel > maxLevelWeapons){
            if (typeWeapon < (TypeWeapon) maxLevelType){
                typeWeapon = dataWeapon._typeWeapon + 1;
                newLevel = 1;
            }
        }

        DataWeapon newDataWeapon = new DataWeapon(typeWeapon, newLevel);
        HideWeapon(weapon1);
        HideWeapon(weapon2);

        Weapon weapon = _lazyWeaponPool.TryGetWeapon(newDataWeapon);
        if (weapon == null){
            weapon = CreateWeapon(newDataWeapon, cell);
        }
        else{
            weapon.Construct(cell);
        }

        return weapon;
    }
}