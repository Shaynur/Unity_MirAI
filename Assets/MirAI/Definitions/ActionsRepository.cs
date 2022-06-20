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
            Debug.Log("=============BEGIN===============");
            Debug.Log("Command = " + command.ToString("X8") + "\n");
            foreach (var action in _collection) {
                if (action.Command == (command & action.CommandMask)) {
                    result.Add(action);
                    Debug.Log(action.Id);
                }
            }
            Debug.Log("Totaly " + result.Count + " actions before processing.");
            Debug.Log("Processing...");
            bool paramIsHealth = true;
            for (int i = 0; i < result.Count; i++) {
                if (result[i].Id == "Unit")
                    result.RemoveAt(i--);
                else if (result[i].Id == "Me")
                    result.RemoveAt(i + 1);
                else if (result[i].Id == "Type") {
                    result.RemoveAt(i--);
                    paramIsHealth = false;
                }
                else if (result[i].Id == "HP") {
                    result.RemoveAt(i--);
                }
                else if (result[i].Id.Contains("HP") && !paramIsHealth)
                    result.RemoveAt(i--);
                else if (result[i].Id.Contains("Type") && paramIsHealth)
                    result.RemoveAt(i--);
            }
            foreach (var action in result)
                Debug.Log(action.Id);
            Debug.Log("Totaly " + result.Count);
            Debug.Log("==============END================");
            return result.ToArray();
        }


        public ActionsDef[] GetConditions2(int command) {
            var result = new List<ActionsDef>();
            Debug.Log("=============BEGIN===============");
            Debug.Log("Command = " + command.ToString("X8") + "\n");
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
                        Debug.Log(action.Id + "  - didn`t added");
                        removeNext = false;
                        continue;
                    }
                    if (action.Id == "Me")
                        removeNext = true;
                    Debug.Log(action.Id);
                    result.Add(action);
                }
            }
            //foreach (var action in result)
            //    Debug.Log(action.Id);
            Debug.Log("Totaly " + result.Count + " actions in list.");
            Debug.Log("==============END================");
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