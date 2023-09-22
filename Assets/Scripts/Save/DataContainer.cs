using System;
using System.Collections.Generic;

[Serializable]
public class DataContainer{
    public int currentLevel = 0;

    public List<CellsInformation> cellsInformation = new List<CellsInformation>();

    public int gold = 20;
    public int priceWeapon = 15;
    public int priceHealth = 10;

    public int maxHealthKing = 10;
    public int levelShopWeapon = 1;
    public int maxLevelOfCreatedWeapon = 1;
}

[Serializable]
public struct CellsInformation{
    public CellsInformation(int indexOfCell, int levelWeapon){
        this.indexOfCell = indexOfCell;
        this.levelWeapon = levelWeapon;
    }

    public int indexOfCell;
    public int levelWeapon;
}