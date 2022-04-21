using System;
using Assets.MirAI.Models;
using Assets.MirAI.UI.AiEditor.SelectAction;
using Assets.MirAI.UI.Widgets;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.MirAI.Utils {

    [Serializable]
    public class UnityEventString : UnityEvent<string> { }

    [Serializable]
    public class EventNodeMove : UnityEvent<Node, Vector3> { }

    [Serializable]
    public class EventWithNode : UnityEvent<Node> { }

    [Serializable]
    public class EventWithProgram : UnityEvent<Program> { }

    [Serializable]
    public class ItemClickEvent : UnityEvent<ProgramItemWidget> { }

    [Serializable]
    public class CommandBtnClickEvent : UnityEvent<SelectCommandButton> { }
}