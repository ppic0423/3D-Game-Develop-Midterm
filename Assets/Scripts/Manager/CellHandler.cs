using System.Collections.Generic;
using UnityEngine;

public class CellHandler : Selector
{
    [Header("Grid")]
    public Grid grid;

    [Header("UI")]
    RectTransform current_UI;
    [SerializeField] RectTransform onTower_UI;
    [SerializeField] RectTransform empty_UI;
    [SerializeField] GameObject TurretShop_1;
    [SerializeField] GameObject TurretShop_2;

    public override void Enter()
    {
        if (target.GetComponent<Tile>().turret != null)
        {
            onTower_UI.gameObject.SetActive(true);
            current_UI = onTower_UI;
        }
        else
        {
            empty_UI.gameObject.SetActive(true);
            TurretShop_1.SetActive(true);
            TurretShop_2.SetActive(false);
            current_UI = empty_UI;
        }
    }
    public override void Tick()
    {
        current_UI.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }
    public override void Exit()
    {
        onTower_UI.gameObject.SetActive(false);
        empty_UI.gameObject.SetActive(false);
    }

    // 터렛 생성
    public void BuildTurret(GameObject prefab)
    {
        // 골드 확인
        int cost = prefab.GetComponent<Turret>().Cost;
        if (ResourceManager.Instance.Gold < cost)
            return;
        // 골드 사용
        ResourceManager.Instance.UseGold(cost);

        // 터렛 생성
        GameObject go = Instantiate(prefab, target.transform);
        Turret newTurret = go.GetComponent<Turret>();
        target.GetComponent<Tile>().turret = newTurret;

        /* 시너지 적용 */

        // 주변 타일 및 터렛 확인
        List<Tile> neighbourTiles = grid.Neighbours(target.GetComponent<Tile>());
        // 방금 만든 터렛 시너지 적용할 대상 수집
        List<Turret> nearbyTurretsForNewTurret = new List<Turret>();

        // 방금 만든 터렛에 시너지 적용
        SynergyManager.Instance.CheckAndApplySynergy(newTurret, nearbyTurretsForNewTurret);
        
        // 주변 터렛 시너지 적용
        foreach (Tile tile in neighbourTiles)
        {
            if (tile.turret != null)
            {
                nearbyTurretsForNewTurret.Add(tile.turret);

                // 이웃 타일의 터렛 시너지를 적용할 대상 수집
                List<Turret> neighbourTurretsForExistingTurret = new List<Turret>();
                foreach (Tile neighbourTile in grid.Neighbours(tile))
                {
                    if (neighbourTile.turret != null)
                    {
                        neighbourTurretsForExistingTurret.Add(neighbourTile.turret);
                    }
                }

                // 이웃 터렛 시너지 적용
                SynergyManager.Instance.CheckAndApplySynergy(tile.turret, neighbourTurretsForExistingTurret);
            }
        }



        // 마우스 입력 해제
        mouseInput.SetSelector(null);
    }
    // 터렛 제거
    public void RemoveTurret()
    {
        if (target.GetComponent<Tile>().turret == null)
            return;

        Destroy(target.GetComponent<Tile>().turret.gameObject);
        target.GetComponent<Tile>().turret = null;

        mouseInput.SetSelector(null);
    }
    // 터렛 업그레이드
    public void UpgradeTurret()
    {
        Turret targetTurret = target.GetComponent<Tile>().turret;

        targetTurret.Upgrade();
    }
    // 터렛 판매
    public void SellTurret()
    {
        Turret targetTurret = target.GetComponent<Tile>().turret;

        ResourceManager.Instance.AddGold((int)(targetTurret.Cost * 2 / 3));
    }
}