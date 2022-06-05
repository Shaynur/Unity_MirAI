using System.Collections.Generic;
using Assets.MirAI.AiEditor.SelectAction;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MirAI.Definitions.Editor {

    [CustomPropertyDrawer(typeof(ActionsAttribute))]
    public class ActionsAttributeDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var actions = ActionsRepository.I.Collection;
            var ids = new List<string>();
            foreach (var action in actions) {
                ids.Add(action.Id);
            }
            var index = Mathf.Max(ids.IndexOf(property.stringValue), 0);

            index = EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            property.stringValue = ids[index];
            
            SelectCommandButton temp = property.serializedObject.targetObject as SelectCommandButton;
            //temp.ActionIcon.sprite = actions[index].Icon;

            var go = temp.gameObject;
            var img = go.GetComponent<Image>();
            img.sprite = actions[index].Icon;
            Debug.Log(img.sprite.name);
        }
    }
}