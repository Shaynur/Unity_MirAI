using UnityEngine;

namespace Assets.MirAI.Utils {

    public static class RectTransformExtensions {

        public static Rect GetWorldRect(this RectTransform rectTransform) {

            Vector3[] corners = new Vector3[4];

            rectTransform.GetWorldCorners(corners);
            // Get the bottom left corner.
            Vector3 position = corners[0];

            Vector2 size = new Vector2(
                rectTransform.lossyScale.x * rectTransform.rect.size.x,
                rectTransform.lossyScale.y * rectTransform.rect.size.y);

            return new Rect(position, size);
        }
    }
}