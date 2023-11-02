using System.Collections.Generic;
using BehaviorTree;

public class EnemyBT : Tree
{
    public static float speed = 2f;
    public static float fovRange = 6f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckPlayerInFOVRange(transform),
                new TaskGoToTarget(transform),
            })
        });

        return root;
    }
}
