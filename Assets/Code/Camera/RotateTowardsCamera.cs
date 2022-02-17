        using UnityEngine;

        // https://answers.unity.com/questions/319924/object-rotation-relative-to-camera.html
        public class RotateTowardsCamera : MonoBehaviour {
            [SerializeField] private bool _lockX;
            [SerializeField] private bool _lockY;
            [SerializeField] private bool _lockZ;

            [SerializeField] private Vector3 _rotationCorrection;

            private void Update() {
                var rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up).eulerAngles;

                if (_lockX) {
                    rotation.x = transform.rotation.x;
                }

                if (_lockY) {
                    rotation.y = transform.rotation.y;
                }

                if (_lockZ) {
                    rotation.z = transform.rotation.z;
                }

                rotation += _rotationCorrection;

                transform.rotation = Quaternion.Euler(rotation);
            }
        }