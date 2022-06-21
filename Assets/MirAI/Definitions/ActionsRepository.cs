using System;
using System.Collections.Generic;
using Assets.MirAI.AiEditor;
using Assets.MirAI.Utils;
using UnityEngine;

namespace Assets.MirAI.Definitions {

    [CreateAssetMenu(menuName = "MirAi/Actions", fileName = "Actions")]
    public class ActionsRepository : ScriptableObject {

        [SerializeField] private ActionsDef[] _collection;

        private static ActionsRepository _instance;
        public static ActionsRepository I => _instance == null ? Load() : _instance;
        private static ActionsRepository Load() {
            return _instance = Resources.Load<ActionsRepository>("Conditions");
        }

        public ActionsDef Get(string id) {
            if (!string.IsNullOrEmpty(id)) {
                foreach (var actionDef in _collection) {
                    if (actionDef.Id == id) {
                        return actionDef;
                    }
                }
            }
            return default;
        }

        public ActionsDef[] GetConditions(int command) {
            var result = new List<ActionsDef>();
            bool paramIsHealth = true;
            bool removeNext = false;
            foreach (var action in _collection) {
                if (action.Command == (command & action.CommandMask)) {
                    if (action.Id == "Type")
                        paramIsHealth = false;
                    if (removeNext
                       || action.Id == "Unit"
                       || action.Id == "Type"
                       || action.Id == "HP"
                       || action.Id == "AnyType"
                       || action.Id == "AnyRange"
                       || (action.Id.Contains("HP") && !paramIsHealth)
                       || (action.Id.Contains("Type") && paramIsHealth)) {
                        removeNext = false;
                        continue;
                    }
                    if (action.Id == "Me")
                        removeNext = true;
                    result.Add(action);
                }
            }
            return result.ToArray();
        }

        public int SetAction(int command, string id) {
            var action = Get(id);
            command &= ~action.CommandMask;
            return command |= action.Command;
        }

        public bool Contain(int command, string id) {
            var action = Get(id);
            return (command & action.CommandMask) == action.Command;
        }

#if UNITY_EDITOR
        public ActionsDef[] Collection => _collection;
#endif
    }

    [Serializable]
    public struct ActionsDef {
        [SerializeField] private string _id;
        [HexInt(digits = 8)][SerializeField] private int _commandMask;
        [HexInt(digits = 8)][SerializeField] private int _command;
        [SerializeField] private Sprite _icon;

        public string Id => _id;
        public int CommandMask => _commandMask;
        public int Command => _command;
        public Sprite Icon => _icon;
    }
}