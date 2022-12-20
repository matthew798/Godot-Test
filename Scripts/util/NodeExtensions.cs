namespace Extensions;

using Godot;

public static class NodeExtensions{
    static void ClearChildren(this Node node){
        foreach(Node n in node.GetChildren()){
            node.RemoveChild(n);
            n.QueueFree();
        }
    }
}
