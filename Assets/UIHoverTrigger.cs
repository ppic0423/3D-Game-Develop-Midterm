using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverTrigger : MonoBehaviour
{
    public GameObject UI;
    float delta;
    bool isOn;
    // EventTrigger ������Ʈ�� �������ų� �߰�
    private void Start()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }

        // OnPointerEnter �̺�Ʈ �߰�
        AddEventTrigger(trigger, EventTriggerType.PointerEnter, OnPointerEnter);

        // OnPointerExit �̺�Ʈ �߰�
        AddEventTrigger(trigger, EventTriggerType.PointerExit, OnPointerExit);
    }

    private void Update()
    {
    }
    // �̺�Ʈ Ʈ���� �߰� �Լ�
    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    // PointerEnter �̺�Ʈ �ڵ鷯
    public void OnPointerEnter(BaseEventData data)
    {
        UI.SetActive(true);
    }

    // PointerExit �̺�Ʈ �ڵ鷯
    public void OnPointerExit(BaseEventData data)
    {
        UI.SetActive(false);
    }
}