using Godot;
using System;
using System.Linq;
using protonic.utils;


public partial class ComponentTree : Godot.Tree
{
	[Export] private Node2D world;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConstructTreeFromFileStructure("./components/particleAccelerator/elements");

		Connect("item_selected", new Callable(this, nameof(OnItemSelected)));
	}
    private void ConstructTreeFromFileStructure(string rootPath)  {
	    var root = CreateItem();
	    SetHideRoot(true);
	    SetUpFileTree(rootPath);
    }
    public void SetUpFileTree(string rootPath, TreeItem root = null)
    {
	    using var dir = DirAccess.Open(rootPath);
	    if (root == null) root = CreateItem();
	    if (dir != null)
	    {
		    dir.ListDirBegin();
		    string fileName = dir.GetNext();
		    while (fileName != "")
		    {
			    if (dir.CurrentIsDir())
			    {
				    TreeItem currentChild = root.CreateChild();
				    currentChild.SetText(0, fileName);
				    SetUpFileTree(rootPath+"/"+fileName, currentChild);
			    }
			    fileName = dir.GetNext();
		    }
	    }
	    else
	    {
		    GD.Print("An error occurred when trying to access the path.");
	    }
    }
	private void OnItemSelected()
	{
		TreeItem selected = GetSelected();
		if (selected.GetChildCount() == 0)
		{
			world.Call("SwitchComponent", "./components/particleAccelerator/elements"+GetPath(selected));
		}
		else DeselectAll();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private string GetPath(TreeItem item)
	{
		string result = item.GetText(0);
		TreeItem currentItem = item;
		while (currentItem.GetParent() != null)
		{
			currentItem = currentItem.GetParent();
			result = currentItem.GetText(0) + "/"+result;
		}

		return result;
	}
}
