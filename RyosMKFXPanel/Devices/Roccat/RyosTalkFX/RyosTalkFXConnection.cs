using Roccat_Talk.TalkFX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roccat_Talk.RyosTalkFX
{
    /// <summary>
    /// Ryos MK PRO specific LED illumination configuration. 
    /// Requires ROCCAT Talk to be installed and running.
    /// </summary>
    public class RyosTalkFXConnection : BasicTalkFxConnection
    {
        private readonly Func<IntPtr, bool> _init_ryos_talk;
        private readonly Func<IntPtr, bool, bool> _set_ryos_kb_SDKmode;
        private readonly Action<IntPtr> _turn_off_all_LEDS;
        private readonly Action<IntPtr> _turn_on_all_LEDS;
        private readonly Action<IntPtr, int, int> _all_Key_Blinking;
        private readonly Action<IntPtr, byte> _setLedOn;
        private readonly Action<IntPtr, byte> _setLedOff;
        private readonly Action<IntPtr, byte[]> _set_all_LEDS;
        private readonly Action<IntPtr, byte[], byte[], byte> _set_all_LEDSFX;
        private readonly Action<IntPtr, byte[], Color[], byte> _set_all_LEDSFX_struct;
        private readonly Action<IntPtr, byte[], byte[], byte[], byte[], byte> _set_all_LEDSFX_individual;

        public RyosTalkFXConnection()
        {
            if (Environment.Is64BitProcess)
            {
                _init_ryos_talk = TalkFxBindings.RyosMKPRO.init_ryos_talk_x64;
                _set_ryos_kb_SDKmode = TalkFxBindings.RyosMKPRO.set_ryos_kb_SDKmode_x64;
                _turn_off_all_LEDS = TalkFxBindings.RyosMKPRO.turn_off_all_LEDS_x64;
                _turn_on_all_LEDS = TalkFxBindings.RyosMKPRO.turn_on_all_LEDS_x64;
                _all_Key_Blinking = TalkFxBindings.RyosMKPRO.All_Key_Blinking_x64;
                _setLedOn = TalkFxBindings.RyosMKPRO.set_LED_on_x64;
                _setLedOff = TalkFxBindings.RyosMKPRO.set_LED_off_x64;
                _set_all_LEDS = TalkFxBindings.RyosMKPRO.Set_all_LEDS_x64;
                _set_all_LEDSFX = TalkFxBindings.RyosMKFX.Set_all_LEDSFX_x64;
                _set_all_LEDSFX_struct = TalkFxBindings.RyosMKFX.Set_all_LEDSFX_struct_x64;
                _set_all_LEDSFX_individual = TalkFxBindings.RyosMKFX.Set_all_LEDSFX_individual_x64;
            }
            else
            {
                _init_ryos_talk = TalkFxBindings.RyosMKPRO.init_ryos_talk_x86;
                _set_ryos_kb_SDKmode = TalkFxBindings.RyosMKPRO.set_ryos_kb_SDKmode_x86;
                _turn_off_all_LEDS = TalkFxBindings.RyosMKPRO.turn_off_all_LEDS_x86;
                _turn_on_all_LEDS = TalkFxBindings.RyosMKPRO.turn_on_all_LEDS_x86;
                _all_Key_Blinking = TalkFxBindings.RyosMKPRO.All_Key_Blinking_x86;
                _setLedOn = TalkFxBindings.RyosMKPRO.set_LED_on_x86;
                _setLedOff = TalkFxBindings.RyosMKPRO.set_LED_off_x86;
                _set_all_LEDS = TalkFxBindings.RyosMKPRO.Set_all_LEDS_x86;
                _set_all_LEDSFX = TalkFxBindings.RyosMKFX.Set_all_LEDSFX_x86;
                _set_all_LEDSFX_struct = TalkFxBindings.RyosMKFX.Set_all_LEDSFX_struct_x86;
                _set_all_LEDSFX_individual = TalkFxBindings.RyosMKFX.Set_all_LEDSFX_individual_x86;
            }
        }

        /// <summary>
        /// Initiate connection to Ryos MK PRO keyboard and check if present
        /// </summary>
        /// <returns>True when the keyboard was detected, and false otherwise.</returns>
        public bool Initialize()
        {
            return _init_ryos_talk(RoccatHandle);
        }

        /// <summary>
        /// Takes control of a connected Ryos MK PRO keyboard
        /// </summary>
        /// <returns></returns>
        public bool EnterSdkMode()
        {
            return _set_ryos_kb_SDKmode(RoccatHandle, true);
        }

        /// <summary>
        /// Gives up control of a connected Ryos MK PRO keyboard
        /// </summary>
        /// <returns></returns>
        public bool ExitSdkMode()
        {
            return _set_ryos_kb_SDKmode(RoccatHandle, false);
        }

        /// <summary>
        /// Turns off all LED lights
        /// </summary>
        public void TurnOffAllLights()
        {
            _turn_off_all_LEDS(RoccatHandle);
        }

        /// <summary>
        /// Turns on all LED lights
        /// </summary>
        public void TurnOnAllLights()
        {
            _turn_on_all_LEDS(RoccatHandle);
        }

        /// <summary>
        /// Blinks all LED lights
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="loopTimes"></param>
        public void BlinkAllKeys(int delayTime, int loopTimes)
        {
            _all_Key_Blinking(RoccatHandle, delayTime, loopTimes);
        }

        /// <summary>
        /// Sets the LED on for the specified key
        /// </summary>        
        /// <remarks>It is advisable to always use <see cref="SetWholeKeyboardState(KeyboardState)"/> or <see cref="SetWholeKeyboardState(bool[])"/></remarks>
        /// <param name="key"></param>
        public void SetLedOn(Key key)
        {
            _setLedOn(RoccatHandle, key.Code);
        }

        /// <summary>
        /// Sets the LED off for the specified key. 
        /// </summary>
        /// <remarks>It is advisable to always use <see cref="SetWholeKeyboardState(KeyboardState)"/> or <see cref="SetWholeKeyboardState(bool[])"/></remarks>
        /// <param name="key"></param>
        public void SetLedOff(Key key)
        {
            _setLedOff(RoccatHandle, key.Code);
        }

        /// <summary>
        /// Sends the specified LED configuration to the keyboard
        /// </summary>
        /// <remarks>It is advisable to use this method when you want to alter the configuration of more than one key. This is because calling <see cref="SetLedOn(Key)"/> and <see cref="SetLedOff(Key)"/> multiple times may cause the keyboard to lag</remarks>
        /// <param name="keyboardState"></param>
        public void SetWholeKeyboardState(KeyboardState keyboardState)
        {
            if (keyboardState == null) throw new ArgumentNullException("keyboardState");

            SetWholeKeyboardState(keyboardState.GetCurrentState());
        }

        /// <summary>
        /// Sends the specified LED configuration to the keyboard
        /// </summary>
        /// <remarks>It is advisable to use this method when you want to alter the configuration of more than one key. This is because calling <see cref="SetLedOn(Key)"/> and <see cref="SetLedOff(Key)"/> multiple times may cause the keyboard to lag</remarks>
        /// <param name="keyboardState">A bool array where the index represents the Key code and the value the LED configuration (on/off)</param>
        public void SetWholeKeyboardState(bool[] keyboardState)
        {
            if (keyboardState == null) throw new ArgumentNullException("keyboardState");
            var keyboardStateParam = ConvertKeyboardState(keyboardState);

            _set_all_LEDS(RoccatHandle, keyboardStateParam);
        }

        private static byte[] ConvertKeyboardState(IEnumerable<bool> keyboardState)
        {
            return keyboardState.Select(Convert.ToByte).ToArray();
        }

        /// <summary>
        /// Sends the specified LED configuration to the Ryos MK FX keyboard
        /// </summary>
        /// <remarks>It is advisable to use this method for all advanced usecases for Ryos MK FX keyboard.</remarks>
        /// <param name="keyState">A byte array where the index represents the Key code and the value the LED status 0 - off, 1 - on</param>
        /// <param name="keyColor">
        /// A Color array where the index represents the Key code and the struct contains RGB intensity values:
        /// 0 - red first key, 1 - green first key, 2 - blue first key, 3 - red second key, ...
        /// </param>
        /// <param name="layout">A byte which represents keyboard layout: DE - 0x00, US - 0x01, JP - 0x02</param>
        public void SetMkFxKeyboardState(byte[] keyState, byte[] keyColor, byte layout)
        {
            _set_all_LEDSFX(RoccatHandle, keyState, keyColor, layout);
        }

        /// <summary>
        /// Sends the specified LED configuration to the Ryos MK FX keyboard
        /// </summary>
        /// <remarks>It is advisable to use this method for all advanced usecases for Ryos MK FX keyboard.</remarks>
        /// <param name="keyState">A byte array where the index represents the Key code and the value the LED status 0 - off, 1 - on</param>
        /// <param name="rgbStruct">A Color array where the index represents the Key code and the struct contains RGB intensity values</param>
        /// <param name="layout">A byte which represents keyboard layout: DE - 0x00, US - 0x01, JP - 0x02</param>
        public void SetMkFxKeyboardState(byte[] keyState, Color[] rgbStruct, byte layout)
        {
            _set_all_LEDSFX_struct(RoccatHandle, keyState, rgbStruct, layout);
        }

        /// <summary>
        /// Sends the specified LED configuration to the Ryos MK FX keyboard
        /// </summary>
        /// <remarks>It is advisable to use this method for all advanced usecases for Ryos MK FX keyboard.</remarks>
        /// <param name="keyState">A byte array where the index represents the Key code and the value the LED status 0 - off, 1 - on</param>
        /// <param name="red">A byte array where the index represents the Key code and the value the red LED intensity</param>
        /// <param name="green">A byte array where the index represents the Key code and the value the green LED intensity</param>
        /// <param name="blue">A byte array where the index represents the Key code and the value the blue LED intensity</param>
        /// <param name="layout">A byte which represents keyboard layout: DE - 0x00, US - 0x01, JP - 0x02</param>
        public void SetMkFxKeyboardState(byte[] keyState, byte[] red, byte[] green, byte[] blue, byte layout) {
            _set_all_LEDSFX_individual(RoccatHandle, keyState, red, green, blue, layout);
        }
    }
}
