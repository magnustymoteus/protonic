using Godot;

namespace protonic;
using System.Collections.Generic;
using protonic.utils;

public class ActionManager : Singleton<ActionManager>
{
    public Stack<Action> Actions = new Stack<Action>();
    public Stack<Action> UndoneActions = new Stack<Action>();

    public void AddAction(Action action)
    {
        if(Actions.Count == 100) Actions.Clear();
        Actions.Push(action);
    }

    public void Undo()
    {
        if (Actions.Count > 0)
        {
            if (UndoneActions.Count == 100) UndoneActions.Clear();
            Action action = Actions.Pop();
            UndoneActions.Push(action);

            action.UndoAction.Delegate.DynamicInvoke();
        }
    }

    public void Redo()
    {
        if (UndoneActions.Count > 0)
        {
            Action action = UndoneActions.Pop();
            AddAction(action);
            GD.Print("do ", action.Name);

            action.Delegate.DynamicInvoke();
        }
    }
}