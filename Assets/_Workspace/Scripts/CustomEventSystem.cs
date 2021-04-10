using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(StandaloneInputModule))]
public class CustomEventSystem : EventSystem
{
    public new static CustomEventSystem current
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<CustomEventSystem>();
            }
            return singleton;
        }
    }
    private static CustomEventSystem singleton;

    public new GameObject lastSelectedGameObject;
    public StandaloneInputModule standaloneInputModule;
    public Vector2 lastMousePosition;
    public bool hideCursorOnKeyIn = false;
    public bool selectOnHighlight = false;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        standaloneInputModule = GetComponent<StandaloneInputModule>();
    }
#endif

    protected override void Awake()
    {
        base.Awake();
        singleton = this;
        Input.imeCompositionMode = IMECompositionMode.Off;
    }

    protected override void Start()
    {
        base.Start();
        lastSelectedGameObject = null;
    }
    /// <summary>
    /// 1. 讓 Highlight = Select
    /// </summary>
    protected override void Update()
    {
        base.Update();

        #region 自動恢復上一個 Select GameObject
        if (!ReferenceEquals(currentSelectedGameObject, null))
        {
            lastSelectedGameObject = currentSelectedGameObject;
        }
        bool shouldActive = false;
        shouldActive |= !Mathf.Approximately(Input.GetAxisRaw(standaloneInputModule.verticalAxis), 0.0f);
        shouldActive |= !Mathf.Approximately(Input.GetAxisRaw(standaloneInputModule.horizontalAxis), 0.0f);
        if (shouldActive)
        {
            SetSelectedGameObject(lastSelectedGameObject);
        }
        #endregion

        #region 當鍵盤作用中隱藏鼠標，移動鼠標後顯示
        if (hideCursorOnKeyIn)
        {
            if ((Vector2)Input.mousePosition == lastMousePosition)
            {
                if (shouldActive) Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
        }
        #endregion
    }

    private void LateUpdate()
    {
        lastMousePosition = Input.mousePosition;
    }



    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.W)) Move(MoveDirection.Up);
    //    if (Input.GetKeyDown(KeyCode.S)) Move(MoveDirection.Down);
    //}

    //public void Move(MoveDirection direction)
    //{
    //    AxisEventData data = new AxisEventData(EventSystem.current);
    //    data.moveDir = direction;
    //    data.selectedObject = EventSystem.current.currentSelectedGameObject;
    //    ExecuteEvents.Execute(data.selectedObject, data, ExecuteEvents.moveHandler);
    //    Debug.Log(direction.ToString());
    //}
}