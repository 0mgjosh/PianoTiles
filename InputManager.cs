using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace PianoTiles
{
    class InputManager
    {
        public KeyboardState _keystate { get; set; }
        public Dictionary<Keys, int> key_lookup = new Dictionary<Keys, int>();
        public EventHandler<InputData> KeyPressed;
        public delegate void KeyPressedEventHandler(object sender, InputData i);

        private InputData _data = new InputData();
        private int _keycount;

        public void Initialize()
        {
            key_lookup.Add(Keys.D1, 0);
            key_lookup.Add(Keys.D2, 1);
            key_lookup.Add(Keys.D3, 2);
            key_lookup.Add(Keys.D4, 3);
            key_lookup.Add(Keys.D5, 4);
            key_lookup.Add(Keys.D6, 5);
        }

        public void Update()
        {
            _keystate = Keyboard.GetState();

            if (_keystate.GetPressedKeyCount() > _keycount)
            {
                _keycount = _keystate.GetPressedKeyCount();
                _data.Key = _keystate.GetPressedKeys()[_keystate.GetPressedKeyCount() - 1];
                _data.Index = key_lookup.GetValueOrDefault(_data.Key);
                KeyWasPressed(this, _data);
            } else if (_keystate.GetPressedKeyCount() < _keycount)
            {
                _keycount = _keystate.GetPressedKeyCount();
            }
        }

        private void KeyWasPressed(object sender, InputData i)
        {
            KeyPressed?.Invoke(sender, i);
        }
    }

    class InputData : EventArgs
    {
        public Keys Key { get; set; }
        public int Index { get; set; }
    }
}
