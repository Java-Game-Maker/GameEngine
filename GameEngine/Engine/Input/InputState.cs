using Silk.NET.Input;
using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class InputState
    {
        public Dictionary<Key, Action> Bind_OnKeyDown = new Dictionary<Key, Action>();
        public Dictionary<Key, Action> Bind_OnKeyHeld = new Dictionary<Key, Action>();
        public Dictionary<MouseButton, Action> Bind_OnMouseDown = new Dictionary<MouseButton, Action>();
        public Dictionary<MouseButton, Action> Bind_OnMouseHeld = new Dictionary<MouseButton, Action>();
        public Action<float, float> Bind_OnMouseMove;

        public void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (Bind_OnKeyDown.ContainsKey(key))
            {
                Bind_OnKeyDown[key].Invoke();
            }
        }

        public void OnKeyHeld(Key key)
        {
            if (Bind_OnKeyHeld.ContainsKey(key))
            {
                Bind_OnKeyHeld[key].Invoke();
            }
        }

        public void OnMouseDown(IMouse mouse, MouseButton button)
        {
            if (Bind_OnMouseDown.ContainsKey(button))
            {
                Bind_OnMouseDown[button].Invoke();
            }
        }

        public void OnMouseHeld(MouseButton button)
        {
            if (Bind_OnMouseHeld.ContainsKey(button))
            {
                Bind_OnMouseHeld[button].Invoke();
            }
        }

        public void OnMouseMove(IMouse mouse, System.Numerics.Vector2 position)
        {
            Bind_OnMouseMove?.Invoke(position.X, position.Y);
        }
    }
}