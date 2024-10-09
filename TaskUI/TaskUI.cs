using System.Linq;
using Godot;
using Godot.Collections;
using protonic;
using protonic.utils;
public partial class TaskUI : Control
{
	// Called when the node enters the scene tree for the first time.
	private Godot.Tree TaskTree;

	private Variant CurrentLevel;

	[Export] private Control Tasks;

	public void DeselectAllTasks()
	{
		foreach (CircleControl TaskCircle in Tasks.GetChildren()) TaskCircle.SetSelect(false);
	}
	
	public void SelectTask(int index)
	{
		DeselectAllTasks();
		Tasks.GetChild<CircleControl>(index).SetSelect(true);
	}
	
	private void SetUpTasks(TreeItem root)
	{
		foreach (var CurrentTask in CurrentLevel.AsGodotDictionary()["tasks"].AsGodotArray())
		{
			var CurrentTaskDict = CurrentTask.AsGodotDictionary();
			TreeItem TaskItem = root.CreateChild();
			TaskItem.SetText(0, CurrentTaskDict["description"].AsString());
			switch (CurrentTaskDict["type"].AsString())
			{
				case "build":
					Array<int> position = CurrentTaskDict["position"].AsGodotArray<int>();
					Vector2 vectorPos = new Vector2(position[0], position[1]) * 32;
					CircleControl circleControl = new CircleControl(vectorPos, CurrentTaskDict["type"].AsString());
					Tasks.AddChild(circleControl);
					break;
				default:
					break;
			}
		}
	}
	public override void _Ready()
	{
		TaskTree = GetChild<Godot.Tree>(0);
		TreeItem root = TaskTree.CreateItem();
		root.SetText(0,"Tasks");
		CurrentLevel = FileLoader.LoadJsonFromFile(FileLoader.GetFile(GetTree().GetRoot().GetChild<World>(0).CurrentLevelPath));
		SetUpTasks(root);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
