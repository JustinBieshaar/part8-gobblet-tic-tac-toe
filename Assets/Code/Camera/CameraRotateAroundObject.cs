using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://emmaprats.com/p/how-to-rotate-the-camera-around-an-object-in-unity3d/
public class CameraRotateAroundObject : MonoBehaviour {
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToTargetMin = 10;
    [SerializeField] private float _distanceToTargetMax = 100;
    [SerializeField] private float _yEulerMin = 10;
    [SerializeField] private float _yEulerMax = 80;

    private float _distanceToTarget;

    private Vector3 previousPosition;

    private void Start() {
        _distanceToTarget = _target.transform.position.z - _camera.transform.position.z;
        previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        Position();
    }

    private void Update() {
        if (GameManager.Instance.DevMode) {
            return;
        }

        Rotate();
        Zoom();
    }

    private void Rotate() {
        if (Input.GetMouseButtonDown(0)) {
            previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0)) {
            Position();
        }
    }

    private void Zoom() {
        float scrollFactor = Input.GetAxis("Mouse ScrollWheel") * 10f;
        if (scrollFactor != 0) {
            _distanceToTarget -= scrollFactor;
            _distanceToTarget = Mathf.Clamp(_distanceToTarget, _distanceToTargetMin, _distanceToTargetMax);
            Position(false);
        }
    }

    private void Position(bool rotate = true) {
        Vector3 newPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        Transform cameraTransform = _camera.transform;

        cameraTransform.position = _target.position;

        if (rotate) {
            Vector3 direction = previousPosition - newPosition;
            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            cameraTransform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cameraTransform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis,
                Space.World); // <— This is what makes it work!

            //clamp
            var cameraEuler = cameraTransform.eulerAngles;
            cameraEuler.x = Mathf.Clamp(cameraEuler.x, _yEulerMin, _yEulerMax);
            cameraTransform.eulerAngles = cameraEuler;
        }

        cameraTransform.Translate(new Vector3(0, 0, -_distanceToTarget));

        previousPosition = newPosition;
    }
}