using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypeHandler : MonoBehaviour
{
    private List<WeaponType> _tierOne = new List<WeaponType>()
    {
        WeaponType.AutoRifle
    };
    private List<WeaponType> _startingWeapons = new List<WeaponType>()
    {
        WeaponType.Sidearm
    };
    public List<WeaponType> GetStartingWeapons() => _startingWeapons;
    public List<WeaponType> GetWeaponsTierOne() => _tierOne;
    public List<WeaponType> GetAllWeapons()
    {
        var allWeapons = new List<WeaponType>();
        allWeapons.AddRange(_tierOne);

        return allWeapons;
    }
}
