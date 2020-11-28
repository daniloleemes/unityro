﻿using System;
using UnityEngine;

public class ROCamera : MonoBehaviour {

    private const float MIN_ZOOM = 12f;
    private const float MAX_ZOOM = 20f;
    private const float ALTITUDE_MIN = 20f;

    private float sensitivy = 4.0f;
    private float zoom = 0f;
    private float currentX = 0f;
    private float altitude = 12f;

    [SerializeField] private Transform _target;
    public float _yaw = 0f;
    public float _pitch = 0f;

    void LateUpdate() {
        var direction = new Vector3(0, 12, -zoom);
        var rotation = Quaternion.Euler(altitude, currentX, 0);
        transform.position = _target.position + rotation * direction;

        /**
         * LookAt is not quite the solution we want because it changes the angle
         * when pitching
         */
        transform.LookAt(_target.position);
    }

    /**
     * Another way of rotating around the subject
     * zoom = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * zoom;
     */
    private void Update() {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (Input.GetMouseButton(1)) {
            currentX += Input.GetAxis("Mouse X");
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            altitude += scrollDelta;
        } else if (scrollDelta != 0) {
            zoom = Mathf.Clamp(zoom += scrollDelta, MIN_ZOOM, MAX_ZOOM);
        }
    }

    protected virtual void CalculateYawPitch() {
        Vector3 dir = (_target.position - gameObject.transform.position).normalized;
        Vector3 m = dir; m.y = gameObject.transform.position.y;

        _yaw = (float)Math.Atan2(dir.x, dir.z);

        float magnitude = (new Vector2(m.x, m.z)).magnitude;

        _pitch = (float)Math.Atan2(dir.y, magnitude);
    }

}
