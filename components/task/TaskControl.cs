namespace protonic;
using Godot;

public partial class TaskControl : Control
{
    public string TaskType;
    private Vector2 _position;
    private bool _selected = false;
    private float _time = 0.0f;
    
    public void SetSelect(bool selected)
    {
        this._selected = selected;
    }
    
    public override void _Ready()
    {
        SetGlobalPosition(this._position);
        SetSize(new Vector2(32,32));

        var task = GD.Load<PackedScene>("res://components/task/task.tscn");
        Node2D instance = task.Instantiate<Node2D>();
        AddChild(instance);
    }
    
    public override void _Process(double delta)
    {
        if (this._selected)
        {
            this._time += (float)delta;
            float yOffset = -Mathf.Sin(this._time * 10f)*0.75f;
            SetGlobalPosition(GetGlobalPosition() + new Vector2(0, yOffset));
            
        }
        else SetGlobalPosition(GlobalPosition.Lerp(this._position, (float)delta*5.0f));
        
    }

    public TaskControl(Vector2 Position, string TaskType)
    {
        this._position = Position;
        this.TaskType = TaskType;
    }
}