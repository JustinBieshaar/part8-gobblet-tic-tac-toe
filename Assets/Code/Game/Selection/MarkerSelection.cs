    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class MarkerSelection : MonoBehaviour {
        [SerializeField] private GameObject _base;
        [SerializeField] private MarkerInfo[] _markersInfo;
        [SerializeField] private float _spacing;

        private readonly List<Marker> _markers = new List<Marker>();
        private Marker _selectedMarker = null;
        private int _turn;

        public Marker SelectedMarker => _selectedMarker;

        [Serializable]
        private struct MarkerInfo {
            public GameObject Marker;
            public int Count;
        }

        private void Start() {
            Generate();
        }

        public void UpdateMarkers() {
            _markers.ForEach(m => m.Check());
        }

        public void SetTurn(int turn) {
            _turn = turn;
            for (int i = 0; i < _markers.Count; i++) {
                _markers[i].SetTurn(turn);
            }
        }

        private void Generate() {
            Clear();
            var size = Vector3.Scale(_base.GetComponent<MeshFilter>().mesh.bounds.size,
                _base.transform.localScale);

            var totalMarkers = _markersInfo.Sum(m => m.Count);
            var index = 1;
            for (int i = 0; i < _markersInfo.Length; i++) {
                for (int j = 0; j < _markersInfo[i].Count; j++) {
                    var totalSize = size.x - _spacing * totalMarkers;
                    var offset = totalSize / (totalMarkers + 1);
                    var position = offset * index + _spacing * index - _spacing / 2.0f - size.x / 2.0f;

                    var marker = Instantiate(_markersInfo[i].Marker, _base.transform.parent).GetComponent<Marker>();
                    marker.transform.localPosition = new Vector3(position * -1, 0, 0);
                    marker.OnSelected += OnMarkerSelected;
                    marker.SetTurn(_turn);
                    marker.SetSize(i);
                    _markers.Add(marker);

                    index++;
                }
            }
        }

        private void OnMarkerSelected(bool selected, Marker marker) {
            if (selected) {
                if (_selectedMarker != null) {
                    _selectedMarker.Deselect();
                }

                _selectedMarker = marker;
            }
            else {
                if (_selectedMarker == marker) {
                    _selectedMarker = null;
                }
            }
        }

        private void Clear() {
            _markers.ForEach(m => Destroy(m.gameObject));

            _markers.Clear();
        }

        public void Reset() {
            Generate();
        }
    }