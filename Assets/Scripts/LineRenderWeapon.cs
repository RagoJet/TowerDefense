using UnityEngine;

public class LineRenderWeapon : MonoBehaviour{
    public static LineRenderWeapon Instance;

    private void Awake(){
        Instance = this;
    }
}