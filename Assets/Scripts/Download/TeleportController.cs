using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportController : MonoBehaviour
{
    public InputActionProperty m_teleportModeActivate;
    public InputActionProperty m_teleprotModeCancel;

    private InputAction teleportModeActivate;
    private InputAction teleprotModeCancel;

    public XRRayInteractor teleportInteractor;

    private void Start()
    {
        teleportModeActivate = m_teleportModeActivate.action;
        teleprotModeCancel = m_teleprotModeCancel.action;

        EnableAction();
    }

    private void OnDestroy()
    {
        DisableAction();
    }

    private void Update()
    {
        if (CanEnterTeleport())
        {
            SetTeleportController(true);
            return;
        }
        if (CanExitTeleport())
        {
            SetTeleportController(false);
            return;
        }
    }

    private void SetTeleportController(bool isEnable)
    {
        if (teleportInteractor != null)
        {
            teleportInteractor.gameObject.SetActive(isEnable);
        }
    }

    private bool CanEnterTeleport()
    {
        bool isTriggerTeleport = teleportModeActivate != null && teleportModeActivate.triggered;
        bool isCancelTeleport = teleprotModeCancel != null && teleprotModeCancel.triggered;
        return isTriggerTeleport && !isCancelTeleport;
    }

    private bool CanExitTeleport()
    {
        bool isCancelTeleport = teleprotModeCancel != null && teleprotModeCancel.triggered;
        bool isReleaseTeleport = teleportModeActivate != null && teleportModeActivate.phase == InputActionPhase.Waiting;
        return isCancelTeleport || isReleaseTeleport;
    }

    private void EnableAction()
    {
        if (teleportModeActivate != null && teleportModeActivate.enabled)
        {
            teleportModeActivate.Enable();
        }
    }

    private void DisableAction()
    {
        if (teleportModeActivate != null && teleportModeActivate.enabled)
        {
            teleportModeActivate.Disable();
        }
    }
}
