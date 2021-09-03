using System.Threading.Tasks;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CharacterController : Singleton<CharacterController>
    {
        [SerializeField] private AudioClip _damageSound;
        [SerializeField] private AudioClip _healSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private int _jumpForce;
    
        [SerializeField] private Transform _healPrefab;
        [SerializeField] private Transform _damagePrefab;
        [SerializeField] private Button _jumpBtn;
        [SerializeField] private Image _hpBar;
    
        private Rigidbody2D _rb;
        private Animator _anim;
        private AudioSource _audioSource;
        private float _hp;
        private bool _isDead;

        public UnityEvent OnDeath;
    
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        
            _jumpBtn.onClick.AddListener(Jump);
        
            _hp = 100;
            _hpBar.fillAmount = 0;
        }

        private void Jump()
        {
            if (!_anim.GetBool("IsGround") || _isDead)
                return;

            _rb.AddForce(new Vector2(0, _jumpForce));

            _audioSource.PlayOneShot(_jumpSound);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            _anim.SetBool("IsGround", true);
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            _anim.SetBool("IsGround", false);
        }

        public void ChangeHP(float value)
        {
            if (_isDead)
                return;

            _hp += value;
            _hp = Mathf.Clamp(_hp, 0, 100);

            if (value > 0)
            {
                var instance = Instantiate(_healPrefab, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity); 
                instance.parent = gameObject.transform;
                _audioSource.PlayOneShot(_healSound);
            }
            else
            {
                var instance = Instantiate(_damagePrefab, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
                instance.parent = gameObject.transform;
                _audioSource.PlayOneShot(_damageSound);
            }
        
            _hpBar.fillAmount = 1 - _hp / 100;

            if (_hp <= 0)
                Death();
        }

        private void Death()
        {
            if (_isDead)
                return;

            _isDead = true;
            _anim.SetBool("Death", true);

            OnDeath?.Invoke();
        }
    }
}
