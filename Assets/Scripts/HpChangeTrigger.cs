using UnityEngine;

namespace Assets.Scripts
{
    public class HpChangeTrigger : MonoBehaviour
    {
        [SerializeField] private float _value;
        [SerializeField] private bool _heal;

        private bool _activated;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag != "Player" || _activated)
                return;

            var character = col.GetComponent<CharacterController>();
            if (character != null)
                character.ChangeHP(_heal ? _value : -_value);
        
            _activated = true;
        }
    }
}
