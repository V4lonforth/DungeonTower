using DungeonTower.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DungeonTower.Input
{
    public class InputController : Singleton<InputController>
    {
        public Action OnInput { get; set; }

        private readonly List<IInteractive> activeInteractives = new List<IInteractive>();
        private readonly List<IInteractive> disabledInteractives = new List<IInteractive>();

        private EventSystem eventSystem;

        private void Awake()
        {
            eventSystem = EventSystem.current;
        }

        private void Update()
        {
            ManageInteractiveStates();

            foreach (Touch touch in UnityEngine.Input.touches)
            {
                if (touch.phase != TouchPhase.Began || eventSystem == null || !eventSystem.IsPointerOverGameObject(touch.fingerId))
                    HandleTouch(touch.fingerId, touch.position, touch.phase);
            }

            if (UnityEngine.Input.touchCount == 0)
            {
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    if (eventSystem == null || !eventSystem.IsPointerOverGameObject())
                        HandleTouch(-1, UnityEngine.Input.mousePosition, TouchPhase.Began);
                }
                if (UnityEngine.Input.GetMouseButton(0))
                    HandleTouch(-1, UnityEngine.Input.mousePosition, TouchPhase.Moved);
                if (UnityEngine.Input.GetMouseButtonUp(0))
                    HandleTouch(-1, UnityEngine.Input.mousePosition, TouchPhase.Ended);
            }
        }

        private void ManageInteractiveStates()
        {
            for (int i = 0; i < activeInteractives.Count; i++)
            {
                if (!activeInteractives[i].Active)
                {
                    disabledInteractives.Add(activeInteractives[i]);
                    activeInteractives.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < disabledInteractives.Count; i++)
            {
                if (disabledInteractives[i].Active)
                {
                    activeInteractives.Add(disabledInteractives[i]);
                    disabledInteractives.RemoveAt(i);
                    i--;
                }
            }

            activeInteractives.Sort((a, b) => b.Layer.CompareTo(a.Layer));
        }

        private bool HandleTouch(int touchId, Vector2 touchPosition, TouchPhase touchPhase)
        {
            OnInput?.Invoke();
            foreach (IInteractive interactive in activeInteractives)
                if (interactive.HandleTouch(touchId, touchPosition, touchPhase))
                    return true;
            return false;
        }

        public void AddInteractive(IInteractive interactive)
        {
            disabledInteractives.Add(interactive);
        }

        public bool RemoveInteractive(IInteractive interactive)
        {
            return activeInteractives.Remove(interactive) || disabledInteractives.Remove(interactive);
        }
    }
}
