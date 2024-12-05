using System.Linq;
using Godot;
using Godot.Collections;
using protonic;
using protonic.utils;
public partial class TaskUI : Control
{
	// Called when the node enters the scene tree for the first time.
	private Godot.Tree _taskTree;

	private Variant _currentLevel;

	[Export] private Control _tasks;

	public void DeselectAllTasks()
	{
		foreach (TaskControl TaskCircle in _tasks.GetChildren()) TaskCircle.SetSelect(false);
	}
	
	public void SelectTask(int index)
	{
		DeselectAllTasks();
		_tasks.GetChild<TaskControl>(index).SetSelect(true);
	}
	
	private void SetUpTasks(TreeItem root)
	{
		foreach (var CurrentTask in _currentLevel.AsGodotDictionary()["tasks"].AsGodotArray())
		{
			var CurrentTaskDict = CurrentTask.AsGodotDictionary();
			TreeItem TaskItem = root.CreateChild();
			TaskItem.SetText(0, CurrentTaskDict["description"].AsString());
			switch (CurrentTaskDict["type"].AsString())
			{
				case "build":
					Array<int> position = CurrentTaskDict["position"].AsGodotArray<int>();
					Vector2 vectorPos = new Vector2(position[0], position[1]) * 32;
					TaskControl taskControl = new TaskControl(vectorPos, CurrentTaskDict["type"].AsString());
					_tasks.AddChild(taskControl);
					break;
				default:
					break;
			}
		}
	}
	public override void _Ready()
	{
		_taskTree = GetChild<Godot.Tree>(0);
		TreeItem root = _taskTree.CreateItem();
		root.SetText(0,"Tasks");
		_currentLevel = FileManager.LoadJsonFromFile(FileManager.GetFile(GetTree().GetRoot().GetChild<World>(0).CurrentLevelPath));
		SetUpTasks(root);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
