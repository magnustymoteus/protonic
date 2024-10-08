namespace protonic;
using Godot;

public partial class CircleControl : Control
{
    public string TaskType;
    private Vector2 _position;
    public override void _Ready()
    {
        SetGlobalPosition(this._position);
        SetSize(new Vector2(32,32));

        Circle circle = new Circle(GetGlobalPosition(), 25, new Color(0.2f, 0.2f, 0.2f, 0.65f));
        AddChild(circle);
		
        Label label = new Label();
        AddChild(label);
        label.SetText(TaskType[0].ToString().ToUpper());
        label.SetAnchorsPreset(LayoutPreset.Center);
        label.SetPosition(new Vector2(16,16)-label.GetSize()/2);
    }

    public override void _Process(double delta)
    {
        
    }

    public CircleControl(Vector2 Position, string TaskType)
    {
        this._position = Position;
        this.TaskType = TaskType;
    }
}