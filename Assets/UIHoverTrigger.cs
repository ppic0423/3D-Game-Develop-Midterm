using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverTrigger : MonoBehaviour
{
    public GameObject UI;
    float delta;
    bool isOn;
    // EventTrigger 컴포넌트를 가져오거나 추가
    private void Start()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        // OnPointerEnter 이벤트 추가
        AddEventTrigger(trigger, EventTriggerType.PointerEnter, OnPointerEnter);

        // OnPointerExit 이벤트 추가
        AddEventTrigger(trigger, EventTriggerType.PointerExit, OnPointerExit);
    }

    private void Update()
    {
    }
    // 이벤트 트리거 추가 함수
    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    // PointerEnter 이벤트 핸들러
    public void OnPointerEnter(BaseEventData data)
    {
        UI.SetActive(true);
    }

    // PointerExit 이벤트 핸들러
    public void OnPointerExit(BaseEventData data)
    {
        UI.SetActive(false);
    }
}