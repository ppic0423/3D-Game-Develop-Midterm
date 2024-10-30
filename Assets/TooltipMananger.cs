using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipObject; // 기존에 존재하는 Tooltip UI 오브젝트 (Image 오브젝트)
    private RectTransform tooltipRect;
    public TextMeshProUGUI tooltipText;
    private float tooltipDelay = 1f; // 마우스를 올려놓은 후 n초 기다리는 시간
    private bool isHovering = false;
    private float hoverTime = 0f;

    private void Start()
    {
        tooltipRect = tooltipObject.GetComponent<RectTransform>();

        // 시작 시 팝업창을 비활성화
        tooltipObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (tooltipObject.activeSelf)
        {
            // 마우스 위치에 따라 팝업창의 위치를 조정
            Vector3 mousePosition = Input.mousePosition;
            tooltipRect.position = mousePosition;
        }
    }

    public void OnMouseEnter(string tooltipTextContent)
    {
        tooltipObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        tooltipObject.SetActive(false);
    }
}