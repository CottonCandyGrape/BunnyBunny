using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickCtrl : MonoBehaviour
{
    public GameObject JoyStick_Panel = null;
    public Image JoyStickBack_Img = null;
    public Image JoyStick_Img = null;

    Vector3 originPos = Vector3.zero;
    Vector3 joyBackPos;
    Vector2 joyDir;
    public Vector2 JoyDir { get { return joyDir.normalized; } }

    const float radius = 90.0f;

    void Start()
    {
        if (JoyStick_Panel != null && JoyStickBack_Img != null && JoyStick_Img != null)
        {
            originPos = JoyStickBack_Img.transform.position;

            EventTrigger trigger = JoyStick_Panel.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnPointerDrag((PointerEventData)data); });
            trigger.triggers.Add(entry);

            if (!AllSceneMgr.Instance.user.joystick)
                JoyStickBack_Img.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        joyDir = JoyStick_Img.transform.position - JoyStickBack_Img.transform.position;
        joyDir.Normalize();
    }

    void OnPointerDown(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left)
            return;

        if (JoyStickBack_Img == null || JoyStick_Img == null)
            return;

        JoyStickBack_Img.transform.position = data.position;
        JoyStick_Img.transform.localPosition = Vector3.zero;
    }

    void OnPointerUp(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left)
            return;

        if (JoyStickBack_Img == null || JoyStick_Img == null)
            return;

        JoyStickBack_Img.transform.position = originPos;
        JoyStick_Img.transform.localPosition = Vector3.zero;

        joyDir = data.position - (Vector2)joyBackPos;
    }

    void OnPointerDrag(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left)
            return;

        if (JoyStickBack_Img == null || JoyStick_Img == null)
            return;

        joyBackPos = JoyStickBack_Img.transform.position;
        joyDir = data.position - (Vector2)joyBackPos;

        if (joyDir.magnitude <= radius)
            JoyStick_Img.transform.localPosition = joyDir;
        else
            JoyStick_Img.transform.localPosition = joyDir.normalized * radius;
    }
}