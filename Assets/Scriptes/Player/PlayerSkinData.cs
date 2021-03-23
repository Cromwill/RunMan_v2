using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "newPlayerSkinData", menuName = "PlayerSckinData")]
public class PlayerSkinData : ScriptableObject, IBuyableObject
{
    [SerializeField] private int _id;
    [SerializeField] private int _price;
    [SerializeField] private string _type;

    [SerializeField] private GameObject _backPack;
    [SerializeField] private GameObject _feet;
    [SerializeField] private GameObject _hair;
    [SerializeField] private GameObject _hands;
    [SerializeField] private GameObject _headAttachments;
    [SerializeField] private GameObject _jacket;
    [SerializeField] private GameObject _legs;
    [SerializeField] private GameObject _torso;
    [SerializeField] private GameObject _vests;

    public float Price => _price;

    public int Id => _id;

    public string Type => _type;

    public string[] GetSkins()
    {
        Type fieldsType = typeof(PlayerSkinData);
        FieldInfo[] fields = fieldsType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(a => (a.GetValue(this) as GameObject) != null).ToArray();
        string[] skinNames = new string[fields.Length];

        for(int i = 0; i< fields.Length; i++)
        {
            skinNames[i] = (fields[i].GetValue(this) as GameObject).name;
        }
        return skinNames;
    }
}
