﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsInput;

namespace _2C2P.Helper
{
    //Source to https://www.codeproject.com/Articles/19004/A-Simple-C-Global-Low-Level-Keyboard-Hook
    //Modified Version
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    /// 


    class globalKeyboardHook
    {
        #region Constant, Structure and Delegate Definitions
        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

        private InputSimulator sim;

        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;
        #endregion

        #region Instance Variables
        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        IntPtr hhook = IntPtr.Zero;
        /// <summary>
        /// Handles raw key input or just ingores it
        /// </summary>
        public CustomKeyEventHandler handler;
        private IntPtr hInstance;
        public keyboardHookStruct lastStruct;
        private int lastParam;
        private keyboardHookProc delegateHolder; 
        public static globalKeyboardHook me;

        internal keyboardHookProc DelegateHolder { get => delegateHolder; set => delegateHolder = value; }
        #endregion



        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
        /// </summary>
        public globalKeyboardHook()
        {
            me = this;            
            handler = new CustomKeyEventHandler();
            hook();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="globalKeyboardHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        ~globalKeyboardHook()
        {
            unhook();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Installs the global hook
        /// </summary>
        public void hook()
        {
            hInstance = LoadLibrary("User32");
            DelegateHolder = hookProc;
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, DelegateHolder, hInstance, 0);
        }

        public void injectKey(Keys key, type typeValue)
        {
            /**keyboardHookStruct lParam = new keyboardHookStruct();
            lParam.vkCode = (int) key;
            lParam.flags = 0;
            lParam.dwExtraInfo = 0;
            lParam.scanCode = GetScanCode(key);
            lParam.time = ((int)DateTime.Now.Ticks);
            if (lParam.time < 0) lParam.time *= -1;
            int wparam;
            if(typeValue == type.keyDown)
            {
                wparam = WM_SYSKEYDOWN;
            }
            else
            {
                wparam = WM_SYSKEYUP;
            }
            lastParam = wparam;
            lastStruct = lParam;
            CallNextHookEx(me.hhook, 0, 256, ref lParam);
            */
            if (sim == null) sim = new InputSimulator();
            sim.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode) key);
        }

        private int GetScanCode(Keys key)
        {
            switch(key)
            {
                case Keys.Q:
                    return 16;
                case Keys.W:
                    return 17;
                case Keys.E:
                    return 18;
                case Keys.R:
                    return 19;
                case Keys.T:
                    return 20;
                case Keys.F1:
                    return 59;
                case Keys.F2:
                    return 60;
                case Keys.F3:
                    return 61;
                case Keys.F4:
                    return 62;
                case Keys.D:
                    return 32;
                case Keys.F:
                    return 33;
                case Keys.P:
                    return 25;
            }
            return 0;
        }

        /// <summary>
        /// Uninstalls the global hook
        /// </summary>
        public void unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public int hookProc(int code, int wParam, ref keyboardHookStruct lParam)
        {
            Keys key = (Keys)lParam.vkCode;
            KeyEventArgs kea = new KeyEventArgs(key);
            if (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
            {                
                handler.KeyDown(key);
            }
            else if (wParam == WM_KEYUP || wParam == WM_SYSKEYUP)
            {
                handler.KeyUp(key);
            }
            if (kea.Handled) return 1;
            return CallNextHookEx(hhook, code, wParam, ref lParam);
        }
        #endregion

        #region DLL imports
        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        #endregion
    }
}
