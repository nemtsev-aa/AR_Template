using UnityEngine;

public class Dron_Engine : MonoBehaviour {
    [Header("Screw Properties")]
    [SerializeField] private Transform _screw;
    [SerializeField] private float _screwRotationSpeed = 80f;
    private Transform _droneTransform;

    void Start() {
        //_droneTransform = FindObjectOfType<Dron_Mover>().gameObject.transform;
    }

    private void Update() {
        HandleScrew();
    }

    private void HandleScrew() {
        if (!_screw) return;
        _screw.Rotate(Vector3.forward, _screwRotationSpeed);
    }
}
