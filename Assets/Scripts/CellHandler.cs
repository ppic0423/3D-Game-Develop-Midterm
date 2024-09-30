using UnityEngine;
using UnityEngine.UI;

public class CellHandler : Selector
{
    RectTransform current_UI;
    [SerializeField] RectTransform onTower_UI;
    [SerializeField] RectTransform empty_UI;

    [SerializeField] GameObject temp_turret;

    public override void Enter()
    {
        if (target.GetComponent<Tile>().turret != null)
        {
            onTower_UI.gameObject.SetActive(true);
            current_UI = onTower_UI;
            // onTower_UI.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        }
        else
        {
            empty_UI.gameObject.SetActive(true);
            current_UI = empty_UI;
            // empty_UI.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
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

    // 磐房 积己
    public void BuildTurret()
    {
        if (target.GetComponent<Tile>().turret != null)
            return;

        GameObject go = Instantiate(temp_turret, target.transform);
        target.GetComponent<Tile>().turret = go.GetComponent<Turret>();

        mouseInput.SetSelector(null);
    }
    // 磐房 力芭
    public void RemoveTurret()
    {
        if (target.GetComponent<Tile>().turret == null)
            return;

        Destroy(target.GetComponent<Tile>().turret.gameObject);
        target.GetComponent<Tile>().turret = null;

        mouseInput.SetSelector(null);
    }
}