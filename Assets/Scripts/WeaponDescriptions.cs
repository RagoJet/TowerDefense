using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDescriptions", menuName = "WeaponDescriptions")]
public class WeaponDescriptions : ScriptableObject{
    [SerializeField] private List<WeaponDescription> listWeapons;

    public List<WeaponDescription> ListWeapons => listWeapons;
}