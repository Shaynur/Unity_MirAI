using Assets.MirAI.UI;
using Assets.MirAI.Utils.Disposables;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.MirAI.Utils {

    public static class WindowUtils {

        public static MenuController CreateMenuWindow(string resourcePath, string canvasName, UnityAction okAction, UnityAction cancelAction) {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = GameObject.Find(canvasName);
            var go = Object.Instantiate(window, canvas.transform);
            var controller = go.GetComponent<MenuController>();
            if (okAction != null) {
                controller.OnOk.Subscribe(okAction);
            }
            if (cancelAction != null) {
                controller.OnCancel.Subscribe(cancelAction);
            }
            return controller;
        }
    }
}