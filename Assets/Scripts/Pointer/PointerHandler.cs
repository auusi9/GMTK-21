using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Code.Pointer
{
    public class PointerHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursorHovered;
        [SerializeField] private Texture2D _cursorClickedPanel;
        [SerializeField] private Texture2D _cursorClicked;
        [SerializeField] private Texture2D _defaultCursor;
        
        private List<int> _panelsHovered = new List<int>();
        private List<int> _panelsClicked = new List<int>();

        private bool _isClicked;
        private Vector2 _cursorHotspot = new Vector2(25, 27);

        public static PointerHandler Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void PanelHovered(int panel)
        {
            _panelsHovered.Add(panel);

            SetPointerImage();
        }

        public void PanelStoppedHovering(int panel)
        {
            if(_panelsHovered.Contains(panel))
            {
                _panelsHovered.Remove(panel);
            }
            
            SetPointerImage();
        }

        public void PanelClicked(int panel)
        {
            _panelsClicked.Add(panel);
            
            SetPointerImage();
        }

        public void PanelReleased(int panel)
        {
            if(_panelsClicked.Contains(panel))
            {
                _panelsClicked.Remove(panel);
            }
            
            SetPointerImage();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (_isClicked == false)
                {
                    _isClicked = true;
                    SetPointerImage();
                }
            }
            else
            {
                if (_isClicked == true)
                {
                    _isClicked = false;
                    SetPointerImage();
                }
            }
        }

        private void SetPointerImage()
        {
            if (_panelsClicked.Count > 0)
            {
                Cursor.SetCursor(_cursorClickedPanel, _cursorHotspot, CursorMode.Auto);
                return;
            }

            if (_panelsHovered.Count > 0)
            {
                Cursor.SetCursor(_cursorHovered, _cursorHotspot, CursorMode.Auto);
                return;
            }

            if (_isClicked)
            {
                Cursor.SetCursor(_cursorClicked, _cursorHotspot, CursorMode.Auto);
                return;
            }
            
            Cursor.SetCursor(_defaultCursor, _cursorHotspot, CursorMode.Auto);
        }
    }
}