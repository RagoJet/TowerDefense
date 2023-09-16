using UnityEngine;

public class WeaponsFactory : MonoBehaviour, ISaveable{
    private WeaponDescriptions weaponDescriptions;
    private Cells _cells;
    [SerializeField] private int maxLevelWeapons = 24;

    private readonly LazyWeaponPool _lazyWeaponPool = new LazyWeaponPool();


    private DataContainer _dataContainer;

    private int _levelShopWeapon;
    private int _maxLevelOfCreatedWeapon;

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
            CreateWeapon(_levelShopWeapon, cell);
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

        if (newLevel > _maxLevelOfCreatedWeapon){
            _maxLevelOfCreatedWeapon = newLevel;
            if (_maxLevelOfCreatedWeapon % 8 == 0){
                _levelShopWeapon += 4;
            }
        }

        HideWeapon(weapon1);
        HideWeapon(weapon2);
        CreateWeapon(newLevel, cell);

        return true;
    }

    public void WriteDataToContainer(){
        _cells.SaveDataToContainer(_dataContainer);
        _dataContainer.levelShopWeapon = _levelShopWeapon;
        _dataContainer.maxLevelOfCreatedWeapon = _maxLevelOfCreatedWeapon;
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

        _levelShopWeapon = _dataContainer.levelShopWeapon;
        _maxLevelOfCreatedWeapon = _dataContainer.maxLevelOfCreatedWeapon;
    }

    public void SetDataContainer(DataContainer dataContainer){
        _dataContainer = dataContainer;
    }
}