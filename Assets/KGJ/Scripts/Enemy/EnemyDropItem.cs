using UnityEngine;
using Define;

public class EnemyDropItem : MonoBehaviour
{
    [SerializeField] GunType itemType;
    [SerializeField] int ammoNum = 5; // ÃÑ¾Ë ¼ö
    GameObject _dropItem;
    
    public void ReduceAmmoNum()
    {
        if (ammoNum > 0) ammoNum--;
    }

    public void DropItem()
    {
        if (ammoNum > 0 && itemType == GunType.BlueGun)
        {
            _dropItem = Resources.Load<GameObject>("Prefabs/BlueGun");
            _dropItem.GetComponent<GunAmmo_Script>().ammoPlusNum = ammoNum;
        }
        else if (ammoNum > 0 && itemType == GunType.RedGun)
        {
            _dropItem = Resources.Load<GameObject>("Prefabs/RedGun");
            _dropItem.GetComponent<GunAmmo_Script>().ammoPlusNum = ammoNum;
        }
        else if (itemType == GunType.Can)
        {
            _dropItem = Resources.Load<GameObject>("Prefabs/KGJ/Can");
        }
        else if (itemType == GunType.SmokeBomb)
        {
            _dropItem = Resources.Load<GameObject>("Prefabs/KGJ/SmokeBomb");
        }
        Instantiate(_dropItem, transform.position, Quaternion.identity);
    }
}
