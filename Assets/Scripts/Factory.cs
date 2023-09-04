using UnityEngine;

public class Factory : MonoBehaviour{
    [SerializeField] private EnemyDescriptions enemyDescriptions;
    [SerializeField] private WeaponDescriptions weaponDescriptions;

    [SerializeField] private King theKing;
    [SerializeField] private GameObject theGate;

    [SerializeField] private Cells cells;


    [SerializeField] private int maxLevelWeapons = 24;

    private readonly LazyEnemyPool _lazyEnemyPool = new LazyEnemyPool();
    private readonly LazyWeaponPool _lazyWeaponPool = new LazyWeaponPool();

    private void Awake(){
        _lazyEnemyPool.Init();
        _lazyWeaponPool.Init();
        CreateAndDirectEnemy(new EnemyData(0, 6));
        CreateAndDirectEnemy(new EnemyData(0, 1));
        CreateAndDirectEnemy(new EnemyData(1, 2));
        CreateAndDirectEnemy(new EnemyData(2, 3));
        CreateAndDirectEnemy(new EnemyData(0, 6));
    }

    private void CreateAndDirectEnemy(EnemyData enemyData){
        Enemy enemy = _lazyEnemyPool.GetEnemy(enemyData);

        if (enemy == null){
            enemy = CreateEnemy(enemyData);
        }

        enemy.Construct(theKing, theGate.transform);
    }

    private Enemy CreateEnemy(EnemyData enemyData){
        int levelOfRace = enemyData.levelOfRace;
        int levelOfUnit = enemyData.levelOfUnit;

        Enemy enemy;
        switch (levelOfRace){
            case 0:
                enemy = Instantiate(enemyDescriptions.ListOfHumans[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfHumans[levelOfUnit]);
                return enemy;
            case 1:
                enemy = Instantiate(enemyDescriptions.ListOfElves[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfElves[levelOfUnit]);
                return enemy;
            case 2:
                enemy = Instantiate(enemyDescriptions.ListOfUndead[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfUndead[levelOfUnit]);
                return enemy;
            case 3:
                enemy = Instantiate(enemyDescriptions.ListOfOrcs[levelOfUnit].enemyPrefab);
                enemy.Construct(theKing, theGate.transform, enemyDescriptions.ListOfOrcs[levelOfUnit]);
                return enemy;
        }

        return null;
    }

    private void HideEnemy(Enemy enemy){
        _lazyEnemyPool.HideEnemy(enemy);
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
        weapon.Construct(weaponDescriptions.ListWeapons[index], this, cell);
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