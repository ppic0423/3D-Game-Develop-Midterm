using System.Collections.Generic;
using UnityEngine;

public class CellHandler : Selector
{
    [Header("UI Elements")]
    private RectTransform currentUI;
    [SerializeField] private RectTransform onTowerUI;
    [SerializeField] private RectTransform emptyUI;
    [SerializeField] private GameObject turretShop1;
    [SerializeField] private GameObject turretShop2;

    [SerializeField] GameObject turretInfo_1;
    [SerializeField] private GameObject turretInfo_2;

    public override void Enter()
    {
        Tile tile = target.GetComponent<Tile>();

        if (tile.turret != null)
        {
            // 타일에 터렛이 있을 경우
            onTowerUI.gameObject.SetActive(true);
            currentUI = onTowerUI;
            turretInfo_1.SetActive(true);
            turretInfo_2.SetActive(false);
        }
        else
        {
            // 타일이 비어있을 경우
            emptyUI.gameObject.SetActive(true);
            turretShop1.SetActive(true);
            turretShop2.SetActive(false);
            currentUI = emptyUI;
        }
    }

    public override void Tick()
    {
        // UI의 위치를 타겟의 위치로 갱신
        currentUI.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }

    public override void Exit()
    {
        // 모든 UI 비활성화
        onTowerUI.gameObject.SetActive(false);
        emptyUI.gameObject.SetActive(false);
    }

    // 터렛 생성
    public void BuildTurret(GameObject prefab)
    {
        Tile tile = target.GetComponent<Tile>();
        Turret newTurret = prefab.GetComponent<Turret>();

        // 골드 부족 시 반환
        if (!CanAffordTurret(newTurret.Cost))
            return;

        // 터렛 생성 및 골드 차감
        ResourceManager.Instance.UseGold(newTurret.Cost);
        Turret turretInstance = Instantiate(prefab, tile.transform).GetComponent<Turret>();
        tile.turret = turretInstance;

        // 주변 타일의 터렛 시너지 적용
        List<Turret> nearbyTurrets = GetNearbyTurrets(tile);
        ApplySynergies(turretInstance, nearbyTurrets);

        // 마우스 입력 해제
        mouseInput.SetSelector(null);
    }

    // 터렛 제거
    public void RemoveTurret()
    {
        Tile tile = target.GetComponent<Tile>();
        if (tile.turret == null)
            return;

        Destroy(tile.turret.gameObject);
        tile.turret = null;

        // 마우스 입력 해제
        mouseInput.SetSelector(null);
    }

    // 터렛 업그레이드
    public void UpgradeTurret()
    {
        Turret turret = target.GetComponent<Tile>().turret;
        turret?.Upgrade();
    }

    // 터렛 판매
    public void SellTurret()
    {
        Turret turret = target.GetComponent<Tile>().turret;
        if (turret == null)
            return;

        ResourceManager.Instance.AddGold((int)(turret.Cost * 2 / 3));
        RemoveTurret();
    }

    // 터렛 구매 가능 여부 확인
    private bool CanAffordTurret(int cost)
    {
        return ResourceManager.Instance.Gold >= cost;
    }

    // 시너지 적용 함수
    private void ApplySynergies(Turret turret, List<Turret> nearbyTurrets)
    {
        // 새로 생성된 터렛에 시너지 적용
        SynergyManager.Instance.CheckAndApplySynergy(turret, nearbyTurrets);

        // 주변 터렛들의 시너지도 적용
        foreach (Turret nearbyTurret in nearbyTurrets)
        {
            List<Turret> neighbourTurrets = GetNearbyTurrets(nearbyTurret.GetComponentInParent<Tile>());
            SynergyManager.Instance.CheckAndApplySynergy(nearbyTurret, neighbourTurrets);
        }
    }

    // 주변 타일에서 터렛을 찾는 함수
    private List<Turret> GetNearbyTurrets(Tile tile)
    {
        List<Turret> nearbyTurrets = new List<Turret>();

        foreach (Tile neighbor in tile.parentGrid.Neighbours(tile))
        {
            if (neighbor.turret != null)
            {
                nearbyTurrets.Add(neighbor.turret);
            }
        }

        return nearbyTurrets;
    }
}
