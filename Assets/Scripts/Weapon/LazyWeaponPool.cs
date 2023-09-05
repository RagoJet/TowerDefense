using System.Collections.Generic;

public class LazyWeaponPool{
    readonly Dictionary<int, Queue<Weapon>> WeaponDictionary = new();

    public void Init(){
        for (int i = 1; i < 24; i++){
            WeaponDictionary[i] = new Queue<Weapon>();
        }
    }

    public void HideWeapon(Weapon weapon){
        Queue<Weapon> queue = WeaponDictionary[weapon.GetLevelWeapon()];
        queue.Enqueue(weapon);
        weapon.gameObject.SetActive(false);
    }

    public Weapon TryGetWeapon(int levelWeapon){
        if (WeaponDictionary[levelWeapon].TryDequeue(out var weapon)){
            weapon.gameObject.SetActive(true);
        }

        return weapon;
    }
}