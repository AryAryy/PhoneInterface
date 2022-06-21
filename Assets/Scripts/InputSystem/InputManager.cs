using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PhoneInterfaceCode.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        private enum ActionMaps
        {
            InGame,
            PhoneInterface
        }

        public enum PhoneInterfaceActionType
        {
            OpenPhone,
            ClosePhone,
            Select,
            Return,
            NavigationUp,
            NavigationDown,
            NavigationRight,
            NavigationLeft
        }
        
        private static InputManager instance_;
        private PlayerInput inputSystem_;

        private InputAction phoneOpen_;
        private InputAction phoneClose_;
        private InputAction phoneChoose_;
        private InputAction phoneReturn_;
        private InputAction phoneNavUp_;
        private InputAction phoneNavDown_;
        private InputAction phoneNavRight_;
        private InputAction phoneNavLeft_;

        [SerializeField] private ActionMaps actionMap;

        public event Action<PhoneInterfaceActionType> OnPhoneInterfaceInput;

        public static InputManager Instance => instance_;

        private void Awake()
        {
            instance_ = this;
            inputSystem_ = GetComponent<PlayerInput>();
            inputSystem_.SwitchCurrentActionMap(actionMap.ToString());
        }

        private void AssignInputActions()
        {
            phoneOpen_ = inputSystem_.actions["OpenPhone"];
            phoneClose_ = inputSystem_.actions["ClosePhone"];
            phoneChoose_ = inputSystem_.actions["Choose"];
            phoneReturn_ = inputSystem_.actions["Return"];
            phoneNavUp_ = inputSystem_.actions["NavigationUp"];
            phoneNavDown_ = inputSystem_.actions["NavigationDown"];
            phoneNavRight_ = inputSystem_.actions["NavigationRight"];
            phoneNavLeft_ = inputSystem_.actions["NavigationLeft"];
        }

        private void AssignInputEvents()
        {
            phoneOpen_.performed += PhoneInterface_OpenPhone;
            phoneClose_.performed += PhoneInterface_ClosePhone;
            phoneChoose_.performed += PhoneInterface_Select;
            phoneReturn_.performed += PhoneInterface_Return;
            phoneNavUp_.performed += PhoneInterface_NavigationUp;
            phoneNavDown_.performed += PhoneInterface_NavigationDown;
            phoneNavRight_.performed += PhoneInterface_NavigationRight;
            phoneNavLeft_.performed += PhoneInterface_NavigationLeft;
        }

        private void UnAssignInputEvents()
        {
            phoneOpen_.performed -= PhoneInterface_OpenPhone;
            phoneClose_.performed -= PhoneInterface_ClosePhone;
            phoneChoose_.performed -= PhoneInterface_Select;
            phoneReturn_.performed -= PhoneInterface_Return;
            phoneNavUp_.performed -= PhoneInterface_NavigationUp;
            phoneNavDown_.performed -= PhoneInterface_NavigationDown;
            phoneNavRight_.performed -= PhoneInterface_NavigationRight;
            phoneNavLeft_.performed -= PhoneInterface_NavigationLeft;
        }
        
        private void PhoneInterface_OpenPhone(InputAction.CallbackContext ctx)
        {
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.OpenPhone);
            SwitchActionMap(ActionMaps.PhoneInterface.ToString());
        }

        private void PhoneInterface_ClosePhone(InputAction.CallbackContext ctx)
        {
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.ClosePhone);
            SwitchActionMap(ActionMaps.InGame.ToString());
        }
        
        private void PhoneInterface_Select(InputAction.CallbackContext ctx) =>
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.Select);

        private void PhoneInterface_Return(InputAction.CallbackContext ctx) =>
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.Return);

        private void PhoneInterface_NavigationUp(InputAction.CallbackContext ctx) =>
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.NavigationUp);

        private void PhoneInterface_NavigationDown(InputAction.CallbackContext ctx) =>
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.NavigationDown);

        private void PhoneInterface_NavigationRight(InputAction.CallbackContext ctx) =>
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.NavigationRight);

        private void PhoneInterface_NavigationLeft(InputAction.CallbackContext ctx) =>
            OnPhoneInterfaceInput?.Invoke(PhoneInterfaceActionType.NavigationLeft);

        private void SwitchActionMap(string map) => inputSystem_.SwitchCurrentActionMap(map);

        private void OnEnable()
        {
            AssignInputActions();
            UnAssignInputEvents();
            AssignInputEvents();
        }

        private void OnDisable() => UnAssignInputEvents();
    }
}