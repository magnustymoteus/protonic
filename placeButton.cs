using Godot;
using System;

public partial class placeButton : MenuButton
{
    [Export] private Node2D world;
    public override void _Ready()
    {
        // Connect the id_pressed signal to the callback method
        GetPopup().SetHideOnItemSelection(false);
        GetPopup().SetHideOnCheckableItemSelection(false);
        GetPopup().SetHideOnStateItemSelection(false);
        Connect("pressed", new Callable(this, nameof(OnMenuButtonPressed)));
        GetPopup().Connect("id_pressed", new Callable(this, nameof(OnMenuItemPressed)));
        GetPopup().GrabFocus();
    }

 
    private void OnMenuItemPressed(int id)
    {
        world.Call("switchComponent", id);
        for(var i=0;i<GetPopup().GetItemCount();i++) GetPopup().SetItemChecked(i, false); 
        GetPopup().SetItemChecked(id, true);
        
    }

    private void OnMenuButtonPressed()
    {
        world.Call("switchBuildingMode");
    }
    

    public override void _Process(double delta)
    {
        
    }

    public override void _GuiInput(InputEvent @event)
    {
        // Capture mouse click event and prevent popup from closing when clicked outside
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed)
            {
                GD.Print("pressed");
                // Prevent the popup from closing
                //GetPopup().SetVisible(true);
            }
        }
    }
}