using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IEventSystemHandler
{
    public GameObject fps;
    public float maxSpeed = 10f;
    bool LeftButtonHeld = false;
    bool RightButtonHeld = false;
    bool ForwardButtonHeld = false;
    bool BackwardButtonHeld = false;
    bool RotationButtonHeld = false;
    bool RotationBackwardsButtonHeld = false;
    bool upButtonHeld = false;
    bool downButtonHeld = false;
    bool copyButtonHeld = false;
    public void pressedLeft(BaseEventData eventData)
    {
        LeftButtonHeld = true;
    }
    public void pressedRight(BaseEventData eventData)
    {
        RightButtonHeld = true;
    }
    public void pressedForward(BaseEventData eventData)
    {
        ForwardButtonHeld = true;
    }
    public void pressedBackward(BaseEventData eventData)
    {
        BackwardButtonHeld = true;
    }
    public void pressedRotate(BaseEventData eventData)
    {
        RotationButtonHeld = true;
    }
    public void pressedRotateBack(BaseEventData eventData)
    {
        RotationBackwardsButtonHeld = true;
    }
    public void pressedUp(BaseEventData eventData)
    {
        upButtonHeld = true;
    }
    public void pressedDown(BaseEventData eventData)
    {
        downButtonHeld = true;
    }
    public void pressedCopy(BaseEventData eventData)
    {
        copyButtonHeld = true;
    }
    public void notpressed(BaseEventData eventData)
    {
        LeftButtonHeld = false;
        RightButtonHeld = false;
        ForwardButtonHeld = false;
        BackwardButtonHeld = false;
        RotationButtonHeld = false;
        RotationBackwardsButtonHeld = false;
        upButtonHeld = false;
        downButtonHeld = false;
        copyButtonHeld = false;
    }
    void Start()
    {
        //fps = GameObject.FindWithTag("FPS");
    }
    public void Update()
    {
        if (LeftButtonHeld)
        {
            
            fps.transform.Translate(-0.1f, 0, 0);
            

        }
        else if (RightButtonHeld)
        {
            fps.transform.Translate(0.1f, 0, 0);
        }
        else if (ForwardButtonHeld)
        {
            fps.transform.Translate(0, 0, 0.1f);
        }
        else if (BackwardButtonHeld)
        {
            fps.transform.Translate(0, 0, -0.1f);
        }
        else if (RotationButtonHeld)
        {
            fps.transform.Rotate((new Vector3(0, -6, 0)) * Time.deltaTime, -1, Space.World);
        }
        else if (RotationBackwardsButtonHeld)
        {
            fps.transform.Rotate((new Vector3(0, -6, 0)) * Time.deltaTime, 1, Space.World);
        }
        else if (upButtonHeld)
        {
            fps.transform.Translate(0, 0.1f, 0);
        }
        else if (downButtonHeld)
        {
            fps.transform.Translate(0, -0.1f, 0);
        }
        else if (copyButtonHeld)
        {
            //fps.SendMessage("copyAcross");
            //GenerateAtom.Instance.copyAcross();
        }

    }
}
