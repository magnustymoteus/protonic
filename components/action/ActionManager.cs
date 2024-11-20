using System.Linq;
using Godot;

namespace protonic;
using System.Collections.Generic;
using protonic.utils;

public class ActionManager : Singleton<ActionManager>
{
    public LinkedList<Action> Actions = new LinkedList<Action>();
    public LinkedList<Action> UndoneActions = new LinkedList<Action>();
    public static int ActionsLimit = 2500;

    public void AddAction(Action action)
    {
        if(Actions.Count == ActionsLimit) Actions.RemoveLast();
        Actions.AddFirst(action);
    }

    public void Undo()
    {
        if (Actions.Count > 0)
        {
            if (UndoneActions.Count == ActionsLimit) UndoneActions.RemoveLast();
            Action action = Actions.First.Value;
            Actions.RemoveFirst();
            UndoneActions.AddFirst(action);

            action.UndoAction.Delegate.DynamicInvoke();
        }
    }

    public void Redo()
    {
        if (UndoneActions.Count > 0)
        {
            Action action = UndoneActions.First.Value;
            UndoneActions.RemoveFirst();
            AddAction(action);

            action.Delegate.DynamicInvoke();
        }
    }
}