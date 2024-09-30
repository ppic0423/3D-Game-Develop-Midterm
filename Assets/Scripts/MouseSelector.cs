using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    Selector currentSelector;
    GameObject currentObj;
    Selector cellSelector;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        cellSelector = GetComponentInChildren<CellHandler>();

        cellSelector.mouseInput = this;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // 마우스 위치에서 레이 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이가 오브젝트와 충돌했는지 확인
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
            {
                // 레이가 UI와 충돌할 경우 리턴
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                currentObj = hit.collider.gameObject;

                // 충돌한 오브젝트의 레이어에 따라 출력 변경
                switch (hit.collider.gameObject.layer)
                {
                    case (int)Define.Layer.Cell:
                        SetSelector(cellSelector);
                        break;
                    default:
                        SetSelector(null);
                        currentObj = null;
                        break;
                }
            }
        }

        if(currentSelector != null)
        {
            currentSelector.Tick();
        }
        // TODO : 선택된 오브젝트, 설렉터 변경은 여기서
    }

    public void SetSelector(Selector selector)
    {
        if(currentSelector != null)
        {
            currentSelector.Exit();
        }

        currentSelector = selector;

        if(currentSelector != null)
        {
            currentSelector.target = currentObj;
            currentSelector.Enter();
        }
    }
}
