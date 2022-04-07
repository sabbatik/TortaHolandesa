using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    delegate void delegator();
    delegator _playerDelegator;
    [SerializeField] GameEvent _onLost;
    [SerializeField] GameEvent _onWin;
    [SerializeField] GameEvent _touchedFruit;
    [SerializeField] GameEvent _onRestart;
    bool _lost = false;
    bool _won = false;

    Rigidbody2D _rb;
    [SerializeField] [Range(2.0f, 20.0f)] private float _moveSpeed = 5.0f;
    [SerializeField] [Range(2.0f, 20.0f)] private float _jumpForce = 12.0f;
    float _horizontal;
    public Transform _groundCheck;
    LayerMask _groundLayer;
    private bool OnFloor()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }
    
    public AudioClip _jumpSFX;
    AudioSource _audioSource;
    private SpriteRenderer _playerRenderer;
    private Animator _anim; 
    
    void Start()
    {   
        _rb = GetComponent<Rigidbody2D>();
        _playerDelegator += Animations;
        _groundLayer = LayerMask.GetMask("Platform");
        _playerRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {   
        _rb.velocity = new Vector2(_horizontal * _moveSpeed, _rb.velocity.y);
    }

    void Update()
    {
        _playerDelegator();

        // Fix Jump animation bug
        if (_rb.velocity.y>0.01f) _groundCheck.gameObject.SetActive(false);
            else _groundCheck.gameObject.SetActive(true);
    }

    public void Move(InputAction.CallbackContext context)
    {  
        _horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && OnFloor())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _audioSource.PlayOneShot(_jumpSFX, 0.7F);
        }
        else if(context.canceled && _rb.velocity.y > 0f) _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);   
    }

    void Animations()
    {   
        // Mirroring
        if(_horizontal < 0.0f) _playerRenderer.flipX = true;
            else if(_horizontal > 0.0f) _playerRenderer.flipX = false;

        // Idle and Run
        _anim.SetFloat("speed", Mathf.Abs(_horizontal));

        // Jump
        if(OnFloor()) _anim.SetBool("is_jumping", false);
            else _anim.SetBool("is_jumping", true);

        // Fix Jump animation bug
        if(!_groundCheck.gameObject.activeSelf) _anim.SetBool("is_jumping", true);
            else if(_groundCheck.gameObject.activeSelf && !OnFloor()) _anim.SetBool("is_jumping", true);
                else _anim.SetBool("is_jumping", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "fruit" && !_won) _touchedFruit?.Invoke();
        
        if(other.tag == "hole" && !_won)
        {
        _lost = true;
        _onLost?.Invoke();
        }
    }

    public void Win()
    {
        _won = true;
        _lost = false;
    }

    public void Restart()
    {   
        _lost = false;
        _won = false;
        _onRestart?.Invoke(); 
    }

    public void Respawn (Transform _spawnPoint)
    {
        transform.position = _spawnPoint.position;
    }

}