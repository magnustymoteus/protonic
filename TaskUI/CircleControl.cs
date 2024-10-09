namespace protonic;
using Godot;

public partial class CircleControl : Control
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

        Circle circle = new Circle(GetGlobalPosition(), 25, new Color(0.2f, 0.2f, 0.2f, 0.4f));
        AddChild(circle);
		
        Label label = new Label();
        AddChild(label);
        label.SetText(TaskType[0].ToString().ToUpper());
        label.SetAnchorsPreset(LayoutPreset.Center);
        label.SetPosition(new Vector2(16,16)-label.GetSize()/2);
    }

    public override void _Process(double delta)
    {
        if (this._selected)
        {
            this._time += (float)delta;
            float yOffset = Mathf.Sin(this._time * 5.0f);
            SetGlobalPosition(GetGlobalPosition() + new Vector2(0, yOffset));
        }
        else SetGlobalPosition(GlobalPosition.Lerp(this._position, (float)delta*5.0f));
    }

    public CircleControl(Vector2 Position, string TaskType)
    {
        this._position = Position;
        this.TaskType = TaskType;
    }
}