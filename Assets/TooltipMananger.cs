using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipObject; // ������ �����ϴ� Tooltip UI ������Ʈ (Image ������Ʈ)
    private RectTransform tooltipRect;
    public TextMeshProUGUI tooltipText;
    private float tooltipDelay = 1f; // ���콺�� �÷����� �� n�� ��ٸ��� �ð�
    private bool isHovering = false;
    private float hoverTime = 0f;

    private void Start()
    {
        tooltipRect = tooltipObject.GetComponent<RectTransform>();

        // ���� �� �˾�â�� ��Ȱ��ȭ
        tooltipObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (tooltipObject.activeSelf)
        {
            // ���콺 ��ġ�� ���� �˾�â�� ��ġ�� ����
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