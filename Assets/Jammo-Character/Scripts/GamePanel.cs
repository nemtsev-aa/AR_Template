using UnityEngine;

public class GamePanel : MonoBehaviour {
    [SerializeField] private  FixedJoystick _joystick;
    [SerializeField] private PlaceObject _placeObject;

    private void Start() {
        Init();
    }

    public void Init() {
        GameObject fried = _placeObject.Friend;
        MovementInput input = fried.GetComponent<MovementInput>();
        input.SetFixedJoystick(_joystick);
    } 
}
