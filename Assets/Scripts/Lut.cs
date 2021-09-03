using UnityEngine;

namespace Assets.Scripts
{
    public class Lut : MonoBehaviour
    {
        [SerializeField] private LutType _type;
    
        private bool _activated;

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag != "Player" || _activated)
                return;

            switch (_type)
            {
                case LutType.Mask:
                    LutController.AddMask();
                    break;
                case LutType.Vaccine:
                    LutController.AddVaccine();
                    break;
                case LutType.Paper:
                    LutController.AddPaper();
                    break;
            }
        
            Destroy(gameObject);

            _activated = true;
        }
    }
}
