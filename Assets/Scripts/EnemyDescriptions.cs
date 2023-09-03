using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDescriptions", menuName = "EnemyDescriptions")]
public class EnemyDescriptions : ScriptableObject{
    [SerializeField] private List<EnemyDescription> listOfHumans;
    [SerializeField] private List<EnemyDescription> listOfElves;
    [SerializeField] private List<EnemyDescription> listOfUndead;
    [SerializeField] private List<EnemyDescription> listOfOrcs;

    public List<EnemyDescription> ListOfHumans => listOfHumans;

    public List<EnemyDescription> ListOfElves => listOfElves;

    public List<EnemyDescription> ListOfUndead => listOfUndead;

    public List<EnemyDescription> ListOfOrcs => listOfOrcs;
}