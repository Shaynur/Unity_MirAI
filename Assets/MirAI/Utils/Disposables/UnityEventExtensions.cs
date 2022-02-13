using System;
using UnityEngine.Events;

namespace Assets.MirAI.Utils.Disposables {
    public static class UnityEventExtensions {
        public static IDisposable Subscribe(this UnityEvent unityEvent, UnityAction call) {
            unityEvent.AddListener(call);
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }
        public static IDisposable Subscribe<T>(this UnityEvent<T> unityEvent, UnityAction<T> call) {
            unityEvent.AddListener(call);
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }

        public static IDisposable Subscribe<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call) {
            unityEvent.AddListener(call);
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
        }
    }
}