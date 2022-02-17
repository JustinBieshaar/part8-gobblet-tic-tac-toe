        using System.Collections.Generic;
        using System.Linq;
        using UnityEngine;

        public class HitBox : MonoBehaviour {
            [SerializeField] private MeshRenderer _renderer;

            private int _type = -1;
            public int Type => _type;

            private bool _markerPlaced;
            private readonly List<Marker> _markers = new List<Marker>();
            private Marker CurrentMarker => _markers.Count > 0 ? _markers.Last() : null;

            private void Start() {
                _renderer.enabled = false;
            }

            private bool CheckAvailableToTrigger() {
                if (GameManager.Instance.GameEnd || GameManager.Instance.DevMode ||
                    GameManager.Instance.GetSelectedMarker == null) {
                    return false;
                }

                if (_markerPlaced && GameManager.Instance.GetSelectedMarker != null && CurrentMarker != null) {
                    return GameManager.Instance.GetSelectedMarker.Size > CurrentMarker.Size;
                }

                return true;
            }

            private void OnMouseOver() {
                if (!CheckAvailableToTrigger()) {
                    return;
                }

                _renderer.enabled = true;
            }

            private void OnMouseExit() {
                _renderer.enabled = false;
            }

            private void OnMouseUpAsButton() {
                if (!CheckAvailableToTrigger()) {
                    return;
                }

                var marker = GameManager.Instance.GetSelectedMarker;
                if (marker == null) {
                    return;
                }

                if (CurrentMarker != null) {
                    CurrentMarker.OverRuled(true);
                }

                marker.SetPosition(transform.position, transform, this);
                _markers.Add(marker);

                _renderer.enabled = false;
                _markerPlaced = true;
                _type = GameManager.Instance.Turn;

                GameManager.Instance.MoveMade();
            }

            public void RemoveMarker(Marker marker) {
                _markers.Remove(marker);
                
                if(CurrentMarker == null) {
                    _type = -1;
                    return;
                }
                
                _type = CurrentMarker.Type;
            }
        }