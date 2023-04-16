using UnityEngine;
using UnityEngine.InputSystem;

namespace Styles.Game
{
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterManager _character;
    
    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
    }

    private void OnEnable()
    {
        BindPlayerInput();
    }

    private void BindPlayerInput()
    {
        _playerControls.Character.Enable();
        
        _playerControls.Character.Move.performed += OnMovePerformed;
        _playerControls.Character.Move.canceled += OnMoveCanceled;

        _playerControls.Character.Interact.performed += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("OnInteract");
        _character.Interact();
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        _character.Move(Vector2.zero);
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        _character.Move(obj.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        UnBindPlayerInput();
    }

    private void UnBindPlayerInput()
    {
        _playerControls.Character.Move.performed -= OnMovePerformed;
    }
}
}