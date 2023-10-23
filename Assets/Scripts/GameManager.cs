using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour {
    private const string m1 = "Move the camera";
    private const string m2 = "Touch";

    [SerializeField] private bool _onePlane = false;
    [SerializeField] private GameObject _placedObject;

    [SerializeField] private Button _replaceButton;
    [SerializeField] private Text _messageText;
    [SerializeField] private Text _planesCountText;
    [SerializeField] private Toggle _onePlaneToggle;

    [SerializeField] private Transform _pointer;

    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private ARPlaneManager _planeManager;

    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    private GameObject _spawnedPlacedObject;

    private void Start() {
        _messageText.text = m1;
        _pointer.gameObject.SetActive(false);
        _replaceButton.onClick.AddListener(ReplaceButtonClick);
        _onePlaneToggle.onValueChanged.AddListener(SetOnePlane);
    }

    private void SetOnePlane(bool status) {
        _onePlane = status;
    }

    private void ReplaceButtonClick() {
        foreach (var plane in _planeManager.trackables) {
            Destroy(plane.gameObject);
        }

        Destroy(_spawnedPlacedObject);
        _messageText.text = $"{m1}";
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount == 0) {
            touchPosition = default;
            return false;
        }

        touchPosition = Input.GetTouch(0).position;
        return true;
    }

    private void Update() {
        ShowPlaceCount();

        if (!TryGetTouchPosition(out var touchPosition) || !_raycastManager.Raycast(touchPosition, _raycastHits, TrackableType.PlaneWithinPolygon)) {
            ShowPointer(false, Vector3.zero);
            return;
        } else {

            var plane = _planeManager.GetPlane(_raycastHits[0].trackableId);
            var hitPose = _raycastHits[0].pose;
            
            GetRotation(plane, out Quaternion rotation);

            if (!_spawnedPlacedObject) 
                _spawnedPlacedObject = Instantiate(_placedObject, hitPose.position, rotation);

            _spawnedPlacedObject.transform.position = hitPose.position;

            plane.gameObject.SetActive(false);

            ShowPointer(true, touchPosition);
        }
    }

    private void ShowPlaceCount() {
        _planesCountText.text = $"{_planeManager.trackables.count}";
    }

    private void GetRotation(ARPlane plane, out Quaternion rotation) {
         rotation = new Quaternion();
        _messageText.text = $"{plane.alignment}";
        
        switch (plane.alignment) {
            case PlaneAlignment.None:
                break;
            case PlaneAlignment.HorizontalUp:
                rotation = Quaternion.identity;
                break;
            case PlaneAlignment.HorizontalDown:
                rotation = new Quaternion(180, 0, 0, 0);
                break;
            case PlaneAlignment.Vertical:
                rotation = new Quaternion(-90, 0, 0, 0);
                break;
            case PlaneAlignment.NotAxisAligned:
                rotation = plane.transform.rotation;
                break;
            default:
                break;
        }
    }

    private void ShowPointer(bool status, Vector3 position) {
        _pointer.gameObject.SetActive(status);

        if (status) _pointer.position = position;
    }
}
