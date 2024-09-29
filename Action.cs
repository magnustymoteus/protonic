namespace protonic;
using System;
public class Action
{
    public string Name;
    public Delegate Delegate;
    public Action UndoAction;

    public Action(string Name, Delegate Delegate)
    {
        this.Name = Name;
        this.Delegate = Delegate;
    }
}
