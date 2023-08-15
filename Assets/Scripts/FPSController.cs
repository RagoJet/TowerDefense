using UnityEngine;

public class FPSController : MonoBehaviour{
    [SerializeField] private int fps = 60;

    private void Awake(){
        Application.targetFrameRate = fps;
    }
}