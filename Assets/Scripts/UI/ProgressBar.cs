using UnityEngine;

namespace Assets.Scripts
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Transform _left;
        [SerializeField] private Transform _right;
        [SerializeField] private Transform _marker;

        public void UpdateView(float progress)
        {
            var x = _left.position.x + (_right.position.x - _left.position.x) * progress;
            _marker.transform.position = new Vector2(x, _marker.transform.position.y);
        }
    }
}
