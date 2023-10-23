using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlaceObject : MonoBehaviour {
    [SerializeField] private GameObject _prefab;

    private ARRaycastManager _arRaycastManager;
    private ARPlaneManager _arPlaneManager;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private GameObject _friend;

    public GameObject Friend => _friend;

    private void Awake() {
        _arRaycastManager ??= GetComponent<ARRaycastManager>();
        _arPlaneManager ??= GetComponent<ARPlaneManager>();
    }

    private void OnEnable() {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(Finger finger) {
        if (finger.index != 0) return;

        if (_friend != null) return;


        if (_arRaycastManager.Raycast(finger.currentTouch.screenPosition, _hits, TrackableType.PlaneWithinPolygon)) {
            foreach (var hit in _hits) {
                Pose pose = hit.pose;
                _friend = Instantiate(_prefab, pose.position, pose.rotation);

                if (_arPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp) {
                    RotationFriendToCamera();

                    ShowAllPlanes(false);
                }
            }
        }
    }

    private void RotationFriendToCamera() {
        Vector3 position = _friend.transform.position;
        position.y = 0f;

        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.y = 0f;

        Vector3 direction = cameraPosition - position;
        Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
        Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, _friend.transform.up.normalized);
        Quaternion targetRotation = Quaternion.Euler(scaledEuler);

        _friend.transform.rotation *= targetRotation;
    }

    private void ShowAllPlanes(bool status) {
        foreach (var plane in _arPlaneManager.trackables) {
            plane.gameObject.SetActive(status);
        }
    }
}
