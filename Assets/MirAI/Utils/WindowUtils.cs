using UnityEngine;

namespace Assets.MirAI.Utils {

    public static class WindowUtils {

        public static GameObject CreateWindow(string resourcePath, string canvasName) {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = GameObject.Find(canvasName);
            return Object.Instantiate(window, canvas.transform);
        }
    }
}