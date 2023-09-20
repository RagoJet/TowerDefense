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
        int newLevel = weapon1.GetLevel() + 1;
        if (newLevel > maxLevelWeapons){
            return false;
        }

        HideWeapon(weapon1);
        HideWeapon(weapon2);
        CreateWeapon(newLevel, cell);

        if (newLevel > _maxLevelOfCreatedWeapon){
            _maxLevelOfCreatedWeapon = newLevel;
            switch (_maxLevelOfCreatedWeapon){
                case 4:
                    _levelShopWeapon = 2;
                    ReplaceUselessWeapons();
                    break;
                case 9:
                    _levelShopWeapon = 5;
                    ReplaceUselessWeapons();
                    break;
                case 13:
                    _levelShopWeapon = 9;
                    ReplaceUselessWeapons();
                    break;
                case 17:
                    _levelShopWeapon = 13;
                    ReplaceUselessWeapons();
                    break;
                case 21:
                    _levelShopWeapon = 17;
                    ReplaceUselessWeapons();
                    break;
                case 24:
                    _levelShopWeapon = 24;
                    ReplaceUselessWeapons();
                    break;
            }
        }

        return true;
    }

    private void ReplaceUselessWeapons(){
        for (int i = 0; i < _cells._arrayCells.Length; i++){
            if (!_cells._arrayCells[i].IsAvailable()){
                if (_cells._arrayCells[i].GetWeapon().GetLevel() < _levelShopWeapon){
                    Destroy(_cells._arrayCells[i].GetWeapon().gameObject);
                    CreateWeapon(_levelShopWeapon, _cells._arrayCells[i]);
                }
            }
        }
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