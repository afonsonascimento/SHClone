using VRTK;

public class HapticFeedback : VRTK_InteractableObject
{
    private VRTK_ControllerReference controllerReference;

    
    public override void Grabbed(VRTK_InteractGrab grabbingObject)
    {
        base.Grabbed(grabbingObject);
        controllerReference = VRTK_ControllerReference.GetControllerReference(grabbingObject.controllerEvents.gameObject);
        HapticPulse();
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        controllerReference = null;
    }

    private void HapticPulse()
    {
        VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, 1, 0.1f, 0.01f);
    }
}
