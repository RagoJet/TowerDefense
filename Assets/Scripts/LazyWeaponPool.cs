using System.Collections.Generic;

public class LazyWeaponPool{
    Dictionary<DataWeapon, Queue<Weapon>> WeaponDictionary = new();

    public void Init(){
        WeaponDictionary[new DataWeapon(TypeWeapon.RocketLauncher, 1)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.RocketLauncher, 2)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.RocketLauncher, 3)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.RocketLauncher, 4)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.BigRocketLauncher, 1)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.BigRocketLauncher, 2)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.BigRocketLauncher, 3)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.BigRocketLauncher, 4)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.Catapult, 1)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.Catapult, 2)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.Catapult, 3)] = new Queue<Weapon>();
        WeaponDictionary[new DataWeapon(TypeWeapon.Catapult, 4)] = new Queue<Weapon>();
    }

    public void HideWeapon(Weapon weapon){
        Queue<Weapon> queue = WeaponDictionary[weapon.GetDataWeapon()];
        queue.Enqueue(weapon);
        weapon.gameObject.SetActive(false);
    }

    public Weapon TryGetWeapon(DataWeapon dataWeapon){
        if (WeaponDictionary[dataWeapon].TryDequeue(out var weapon)){
            weapon.gameObject.SetActive(true);
        }

        return weapon;
    }
}