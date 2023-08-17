using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDescriptions", menuName = "WeaponDescriptions")]
public class WeaponDescriptions : ScriptableObject{
    [SerializeField] private List<WeaponDescription> listRocketLauncher;
    [SerializeField] private List<WeaponDescription> listBigRocketLauncher;
    [SerializeField] private List<WeaponDescription> listCatapult;

    public List<WeaponDescription> ListRocketLauncher => listRocketLauncher;
    public List<WeaponDescription> ListBigRocketLauncher => listBigRocketLauncher;
    public List<WeaponDescription> ListCatapult => listCatapult;
}