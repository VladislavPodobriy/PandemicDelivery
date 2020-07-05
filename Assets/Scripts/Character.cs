using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

public class Character : Singleton<Character>
{
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _healSound;
    [SerializeField] private AudioClip _jumpSound;
    

    [SerializeField] private Button _jumpBtn;
    [SerializeField] private Image _hpBar;
    [SerializeField] private int _jumpForce;

    [SerializeField] private Transform _healPrefab;
    [SerializeField] private Transform _damagePrefab;

    private Rigidbody2D _rb;
    private Animator _anim;
    private AudioSource _audioSource;

    [SerializeField] private float _hp;

    private bool _isDead;

    

    private void Start()
    {
        _jumpBtn.onClick.AddListener(Jump);

        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

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

    public void OnHeal(float value)
    {
        if (_isDead)
            return;

        _hp += value;
        _hp = Mathf.Clamp(_hp, 0, 100);

        Instantiate(_healPrefab, new Vector2(transform.position.x, 0.8f), Quaternion.identity);

        _audioSource.PlayOneShot(_healSound);

        _hpBar.fillAmount = 1 - _hp / 100;
    }

    public void OnDamage(float value)
    {
        if (_isDead)
            return;

        _hp -= value;
        _hp = Mathf.Clamp(_hp, 0, 100);

        _hpBar.fillAmount = 1 - _hp / 100;

        Instantiate(_damagePrefab, new Vector2(transform.position.x, 0.8f), Quaternion.identity);

        _audioSource.PlayOneShot(_damageSound);

        if (_hp <= 0)
            StartCoroutine(OnDeath());
    }

    public IEnumerator OnDeath()
    {
        _isDead = true;

        _anim.SetBool("Death", true);

        yield return new WaitForSeconds(1f);

        LevelController.StopGame();

        yield return new WaitForSeconds(2f);

        if (!LevelController.Instance.end && !LevelManager.IsEndless)
        {
            GameManager.OnLose();
            LevelController.Instance.end = true;
        }
        else if (!EndlessMode.Instance.end)
        {
            GameManager.OnEndlessEnd();
            EndlessMode.Instance.end = true;
        }

    }
}
