using UnityEngine;

public class WeaponsFactory : MonoBehaviour, ISaveable{
    private WeaponDescriptions weaponDescriptions;
    private Cells _cells;
    [SerializeField] private int maxLevelWeapons = 24;

    private readonly LazyWeaponPool _lazyWeaponPool = new LazyWeaponPool();


    private DataContainer _dataContainer;

    public void Construct(WeaponDescriptions weaponDescriptions, Cells cells){
        this.weaponDescriptions = weaponDescriptions;
        _cells = cells;
        _lazyWeaponPool.Init();
    }

    private void HideWeapon(Weapon weapon){
        _lazyWeaponPool.HideWeapon(weapon);
    }

    public bool TryCreateWeapon(){
        if (_cells.TryGetCell(out var cell)){
            CreateWeapon(1, cell);
            return true;
        }
        else return false;
    }

    private void CreateWeapon(int levelWeapon, Cell cell){
        Weapon weapon = _lazyWeaponPool.TryGetWeapon(levelWeapon);
        if (weapon == null){
            int index = levelWeapon - 1;
            weapon = Instantiate(weaponDescriptions.ListWeapons[index].weaponPrefab);
            weapon.Construct(weaponDescriptions.ListWeapons[index], this, cell,
                GetComponent<EnemiesFactory>().ListOfAliveEnemies);
        }
        else{
            weapon.Construct(cell);
        }
    }

    public bool TryMergeWeapons(Weapon weapon1, Weapon weapon2, Cell cell){
        int newLevel = weapon1.GetLevelWeapon() + 1;
        if (newLevel > maxLevelWeapons){
            return false;
        }

        HideWeapon(weapon1);
        HideWeapon(weapon2);
        CreateWeapon(newLevel, cell);

        return true;
    }

    public void WriteDataToContainer(){
        _cells.SaveDataToContainer(_dataContainer);
    }

    public void LoadDataFromContainer(){
        int count = _dataContainer.cellsInformation.Count;
        if (count > 0){
            for (int i = 0; i < count; i++){
                int indexOfCell = _dataContainer.cellsInformation[i].indexOfCell;
                int levelOfWeapon = _dataContainer.cellsInformation[i].levelWeapon;
                CreateWeapon(levelOfWeapon, _cells._arrayCells[indexOfCell]);
            }
        }
    }

    public void SetDataContainer(DataContainer dataContainer){
        _dataContainer = dataContainer;
    }
}