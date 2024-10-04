namespace Godot.CSharp.DependencyInjection.Nodes;

internal static class NodeExtensions
{
    internal static List<Node> FlattenNodes(this Node node)
    {
        var nodes = new List<Node>();
        AggregateNodes(node, nodes);

        return nodes;
    }

    private static void AggregateNodes(Node currentNode, List<Node> nodeList)
    {
        nodeList.Add(currentNode);
        foreach (Node child in currentNode.GetChildren())
            AggregateNodes(child, nodeList);
    }
}
