using Silk.NET.Input;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public class InputState
    {
        public Dictionary<Key, Action> Bind_OnKeyDown = new Dictionary<Key, Action>();
        public Dictionary<Key, Action> Bind_OnKeyHeld = new Dictionary<Key, Action>();
        public Dictionary<Key, Action> Bind_OnMouseDown = new Dictionary<Key, Action>();

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
    }

    public class InputHandler
    {
        public IInputContext inputContext;
        public IKeyboard keyboard;
        public IMouse mouse;

        public int activeState = 0;
        public List<InputState> inputStates = new List<InputState>();

        private Dictionary<Key, bool> keyStates = new Dictionary<Key, bool>();

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
        }

        private void OnKeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            keyStates[key] = true;
        }

        private void OnKeyUp(IKeyboard keyboard, Key key, int arg3)
        {
            keyStates[key] = false;
        }

        public void Update()
        {
            foreach (var keyState in keyStates.Where(k => k.Value))
            {
                inputStates[activeState].OnKeyHeld(keyState.Key);
            }
        }
    }
}