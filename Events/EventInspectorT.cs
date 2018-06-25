﻿using UnityEngine;
using UnityEngine.Events;

namespace LUT.Event
{
    public class EventInspector<T, EventObjectType,UnityEventType> : MonoBehaviour where UnityEventType : UnityEvent<T> where EventObjectType : EventObject<T>
    {

        [SerializeField]
        private EventObjectType _targetEvent;
#if UNITY_EDITOR
        private EventObjectType _cacheEvent;
#endif
        [SerializeField]
        private UnityEventType _onInvoke;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_cacheEvent != null && Application.isPlaying)
            {
                _cacheEvent.Unregister(Invoke);
            }
            _cacheEvent = _targetEvent;
        }
#endif

        public void Reset()
        {
            _targetEvent = null;
        }
        public void OnEnable()
        {
            if(_targetEvent == null)
            {
                Debug.LogError("No event on EventInspector of " + name + ". Disabling itself");
                enabled = false;
                return;
            }
#if UNITY_EDITOR
            _cacheEvent = _targetEvent;
#endif
            _targetEvent.Register(Invoke);
        }

        public void OnDisable()
        {
            _targetEvent.Unregister(Invoke);
        }

        private void Invoke(T t)
        {
            _onInvoke.Invoke(t);
        }
    }
}
