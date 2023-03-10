namespace UnityEngine.AI
{
    public class NavPathDebug : MonoBehaviour
    {
#if UNITY_EDITOR
        NavMeshAgent _agent;
        [SerializeField] private Color _gizmoColor = Color.white;
    
        void Start()
        {
            _agent=GetComponent<NavMeshAgent>();
        }

        private void OnDrawGizmos()
        {
            if (_agent == null || !_agent.hasPath) return; //Stop if no NavMeshAgent or no Path
            Gizmos.color = _gizmoColor;

            NavMeshPath path = _agent.path; //Get path
            for (int i = 1; i < path.corners.Length; i++) //For each point in path (except first)
            {
                Gizmos.DrawLine(path.corners[i-1], path.corners[i]); //Draw a line between the point and the previous
            }
        
            Gizmos.DrawWireCube(path.corners[path.corners.Length - 1]+Vector3.up, new Vector3(0.5f, 1f, 0.5f));
        }
#endif
    }
}