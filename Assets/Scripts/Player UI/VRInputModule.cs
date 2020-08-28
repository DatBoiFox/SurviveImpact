using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;


public class VRInputModule : BaseInputModule
{

    public Camera camera;
    public SteamVR_Input_Sources targetSource;
    public SteamVR_Action_Boolean clickAction;
    public GameObject currentGameobject = null;
    protected PointerEventData data = null;

    private VRInputModule instance;

    protected override void Awake()
    {
        base.Awake();
        data = new PointerEventData(eventSystem);
        DontDestroyOnLoad(this.gameObject);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Process()
    {
        // Reset
        data.Reset();
        data.position = new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);

        // Raycast
        eventSystem.RaycastAll(data, m_RaycastResultCache);
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentGameobject = data.pointerCurrentRaycast.gameObject;
        
        // Clear
        m_RaycastResultCache.Clear();

        // Hover
        HandlePointerExitAndEnter(data, currentGameobject);

        // Press
        if (clickAction.GetStateDown(targetSource))
            ProcessPress(data);

        // Release
        if (clickAction.GetLastStateUp(targetSource))
            ProcessRelease(data);

    }

    public PointerEventData GetData()
    {
        return data;
    }

    private void ProcessPress(PointerEventData data)
    {
        // Set Raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        // Check for object hit, get down hadler and call
        GameObject pointerPresshandler = ExecuteEvents.ExecuteHierarchy(currentGameobject, data, ExecuteEvents.pointerDownHandler);

        // If no down handler, try and get clock handler
        if (pointerPresshandler == null)
            pointerPresshandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentGameobject);

        // Set data
        data.pressPosition = data.position;
        data.pointerPress = pointerPresshandler;
        data.rawPointerPress = currentGameobject;
        
    }
    private void ProcessRelease(PointerEventData data)
    {
        // Execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        // Check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentGameobject);

        // Check if actual
        if (data.pointerPress == pointerUpHandler)
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);

        // Clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        // Reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
