using TMPro;
using UnityEngine;

public class DronController : MonoBehaviour {
    [SerializeField] private InputView _inputView;
    [SerializeField] private FixedJoystick _fixedJoystick;
    [SerializeField] private Rigidbody _rigidbody;
    
    [SerializeField] private float _speed;
    private void Start() {
        _fixedJoystick = FindObjectOfType<FixedJoystick>();
        _inputView = FindObjectOfType<InputView>();
        _inputView.ShowText($"Дрон создан!");
    }

    private void FixedUpdate() {
        float xVal = _fixedJoystick.Horizontal;
        float yVal = _fixedJoystick.Vertical;

        Vector3 movement = new Vector3(xVal, 0, yVal);
        _rigidbody.velocity = movement * _speed;

        if (xVal != 0 && yVal !=0) {
            _inputView.ShowText($"X[{xVal:N1}]\nY[{yVal:N1}]\n{_rigidbody.velocity}]");
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                Mathf.Atan2(xVal, yVal) * Mathf.Rad2Deg,
                transform.eulerAngles.z);
        } else {
            _inputView.ShowText($"Нет ввода!");
        }
    }
}
