using System;
using Assets.MirAI.Models;
using Assets.MirAI.UI.Widgets;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.MirAI.Utils {

    [Serializable]
    public class UnityEventString : UnityEvent<string> { }
    [Serializable]
    public class SelectNewNodeEvent : UnityEvent<NodeType> { }
    [Serializable]
    public class EventNodeMove : UnityEvent<Node, Vector3> { }
    [Serializable]
    public class EventFromNode : UnityEvent<Node> { }
    [Serializable]
    public class ItemClickEvent : UnityEvent<ProgramItemWidget> { }
}