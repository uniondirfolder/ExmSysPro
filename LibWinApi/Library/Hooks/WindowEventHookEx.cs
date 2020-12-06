using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using LibWinApi.Library.Classes;
using LibWinApi.Library.Delegates;
using LibWinApi.Library.Enums;
using LibWinApi.Library.Import;

namespace LibWinApi.Library.Hooks
{
    public sealed class WindowEventHookEx
    {
        private readonly WinEventProc _eventProc;
        readonly SortedDictionary<EnumWindowEvent,IntPtr>_hooksDictionary = new SortedDictionary<EnumWindowEvent, IntPtr>();
        EventHandler<WindowEventArgs> _activated;
        EventHandler<WindowEventArgs> _minimized;
        EventHandler<WindowEventArgs> _unminimized;
        EventHandler<WindowEventArgs> _textChanged;

        void Hook(IntPtr hookHandle, EnumWindowEvent @event, IntPtr hWnd, int @object, int child, int threadId,
            int timestampMs)
        {
            EventHandler<WindowEventArgs> handler;
            switch (@event)
            {
                case EnumWindowEvent.FOREGROUND_CHANGED:
                    handler = _activated;
                    break;
                case EnumWindowEvent.NAME_CHANGED:
                    handler = _textChanged;
                    break;
                case EnumWindowEvent.MINIMIZED:
                    handler = _minimized;
                    break;
                case EnumWindowEvent.UNMIMINIZED:
                    handler = _unminimized;
                    break;
                default:
                    Debug.Write($"Unexpected event {@event}");
                    return;
            }

            handler?.Invoke(this, new WindowEventArgs(hWnd));
        }
        IntPtr SetHook(EnumWindowEvent @event)
        {
            IntPtr hookId = DllUser32.SetWinEventHook(
                hookMin: @event, hookMax: @event,
                moduleHandle: IntPtr.Zero, callback: _eventProc,
                processId: 0, threadId: 0,
                flags: EnumHookFlags.OUT_OF_CONTEXT);
            if (hookId == IntPtr.Zero)
                throw new Win32Exception();
            return hookId;
        }
        void EnsureHook(EnumWindowEvent @event)
        {
            if (!_hooksDictionary.TryGetValue(@event, out var hookId))
            {
                hookId = SetHook(@event);
                _hooksDictionary.Add(@event, hookId);
            }
        }
        void EventAdd(ref EventHandler<WindowEventArgs> handler, EventHandler<WindowEventArgs> user,
            EnumWindowEvent @event)
        {
            lock (_hooksDictionary)
            {
                handler += user;
                this.EnsureHook(@event);
            }
        }
        void EventRemove(ref EventHandler<WindowEventArgs> handler, EventHandler<WindowEventArgs> user,
            EnumWindowEvent @event)
        {
            lock (_hooksDictionary)
            {
                if (handler != null && handler - user == null)
                {
                    if (!DllUser32.UnhookWinEvent(_hooksDictionary[@event]))
                        throw new Win32Exception();
                    bool existed = _hooksDictionary.Remove(@event);
                    Debug.Assert(existed);
                }

                handler -= user;
            }
        }

        public event EventHandler<WindowEventArgs> Activated
        {
            add => EventAdd(ref _activated, value, EnumWindowEvent.FOREGROUND_CHANGED);
            remove => EventRemove(ref _activated, value, EnumWindowEvent.FOREGROUND_CHANGED);
        }
        public event EventHandler<WindowEventArgs> Minimized
        {
            add => EventAdd(ref _minimized, value, EnumWindowEvent.MINIMIZED);
            remove => EventRemove(ref _minimized, value, EnumWindowEvent.MINIMIZED);
        }
        public event EventHandler<WindowEventArgs> Unminimized
        {
            add => EventAdd(ref _unminimized, value, EnumWindowEvent.UNMIMINIZED);
            remove => EventRemove(ref _unminimized, value, EnumWindowEvent.UNMIMINIZED);
        }
        public event EventHandler<WindowEventArgs> TextChanged
        {
            add => EventAdd(ref _textChanged, value, EnumWindowEvent.NAME_CHANGED);
            remove => EventRemove(ref _textChanged, value, EnumWindowEvent.NAME_CHANGED);
        }
    }
}