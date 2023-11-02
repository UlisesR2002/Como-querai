using BehaviorTree;
using UnityEngine;

public class CheckPlayerInFOVRange : Node
{
    private static int _enemyLayerMask = 1 << 6;
    private Transform _transform;

    public CheckPlayerInFOVRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t != null)
        {
            Collider[] colliders = Physics.OverlapSphere(
                _transform.position, EnemyBT.fovRange, _enemyLayerMask);

            if(colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
