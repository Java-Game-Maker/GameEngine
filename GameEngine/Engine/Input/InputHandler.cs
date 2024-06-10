using Silk.NET.Input;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class InputHandler
    {
        public IInputContext inputContext;
        public IKeyboard keyboard;
        public IMouse mouse;

        public int activeState = 0;
        public List<InputState> inputStates = new List<InputState>();

        private Dictionary<Key, bool> keyStates = new Dictionary<Key, bool>();
        private Dictionary<MouseButton, bool> mouseButtonStates = new Dictionary<MouseButton, bool>();

        public InputHandler(IInputContext _inputContext)
        {
            inputContext = _inputContext;
            keyboard = _inputContext.Keyboards[0];
            mouse = _inputContext.Mice[0];
        }

        public void Initialize()
        {
            keyboard.KeyDown += inputStates[activeState].OnKeyDown;
            keyboard.KeyUp += OnKeyUp;
            keyboard.KeyDown += OnKeyDown;

            mouse.MouseDown += inputStates[activeState].OnMouseDown;
            mouse.MouseUp += OnMouseUp;
            mouse.MouseDown += OnMouseDown;
            mouse.MouseMove += inputStates[activeState].OnMouseMove;
        }

        private void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            keyStates[key] = true;
        }

        private void OnKeyUp(IKeyboard keyboard, Key key, int arg3)
        {
            keyStates[key] = false;
        }

        private void OnMouseDown(IMouse mouse, MouseButton button)
        {
            mouseButtonStates[button] = true;
        }

        private void OnMouseUp(IMouse mouse, MouseButton button)
        {
            mouseButtonStates[button] = false;
        }

        public void Update()
        {
            foreach (var keyState in keyStates.Where(k => k.Value))
            {
                inputStates[activeState].OnKeyHeld(keyState.Key);
            }

            foreach (var mouseButtonState in mouseButtonStates.Where(m => m.Value))
            {
                inputStates[activeState].OnMouseHeld(mouseButtonState.Key);
            }
        }
    }
}