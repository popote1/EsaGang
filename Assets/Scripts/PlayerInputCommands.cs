using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputCommands : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public PlayerInputManager PlayerInputManager;
    public UIPlayerMenuConfigurationMenu ConfigMenu;
    public VeryController3 Player;
    public void OnMovement(InputValue val)=>Player?.OnMovement(val.Get<Vector2>());
    public void OnPunch(InputValue val){}
    public void OnHit(InputValue val)=>Player?.OnHit();
    public void OnGrabObject(InputValue val)=>Player?.OnGrabObject();
    public void OnBack(InputValue val) => ConfigMenu?.GoBack();
}
