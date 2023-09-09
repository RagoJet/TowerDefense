using UnityEngine;

public class WeaponsFactory : MonoBehaviour{
    private WeaponDescriptions weaponDescriptions;
    private Cells cells;
    [SerializeField] private int maxLevelWeapons = 24;

    private readonly LazyWeaponPool _lazyWeaponPool = new LazyWeaponPool();


    public void Construct(WeaponDescriptions weaponDescriptions, Cells cells){
        this.weaponDescriptions = weaponDescriptions;
        this.cells = cells;
        _lazyWeaponPool.Init();
    }

    private void HideWeapon(Weapon weapon){
        _lazyWeaponPool.HideWeapon(weapon);
    }

    public bool TryCreateWeapon(){
        if (cells.TryGetCell(out var cell)){
            CreateFirstLevelWeapon(cell);
            return true;
        }
        else return false;
    }

    private Weapon CreateFirstLevelWeapon(Cell cell){
        Weapon weapon = _lazyWeaponPool.TryGetWeapon(1);
        if (weapon == null){
            weapon = CreateWeapon(1, cell);
        }

        weapon.Construct(cell);
        return weapon;
    }

    private Weapon CreateWeapon(int levelWeapon, Cell cell){
        int index = levelWeapon - 1;
        Weapon weapon;
        weapon = Instantiate(weaponDescriptions.ListWeapons[index].weaponPrefab);
        weapon.Construct(weaponDescriptions.ListWeapons[index], this, cell,
            GetComponent<EnemiesFactory>().ListOfAliveEnemies);
        return weapon;
    }

    public bool TryMergeWeapons(Weapon weapon1, Weapon weapon2, Cell cell){
        int newLevel = weapon1.GetLevelWeapon() + 1;
        if (newLevel > maxLevelWeapons){
            return false;
        }

        HideWeapon(weapon1);
        HideWeapon(weapon2);
        Weapon weapon = _lazyWeaponPool.TryGetWeapon(newLevel);
        if (weapon == null){
            weapon = CreateWeapon(newLevel, cell);
        }
        else{
            weapon.Construct(cell);
        }

        return true;
    }
}