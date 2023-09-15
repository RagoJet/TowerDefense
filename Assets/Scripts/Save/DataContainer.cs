using System;
using System.Collections.Generic;

[Serializable]
public class DataContainer{
    public int currentLevel = 1;

    public List<CellsInformation> cellsInformation = new List<CellsInformation>();

    public int gold = 15;
    public int priceWeapon = 15;
    public int priceHealth = 10;
    public int priceDamage = 10;

    public int maxHealthKing = 7;
    public int damageKing = 1;
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