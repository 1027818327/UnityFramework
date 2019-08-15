using iBoxDB.LocalServer;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʵ��һ��Save��Read���� 
/// </summary>
public class MyIBoxDB
{
    public const string TABLE_BED_SCENE = "TABLE_BED_SCENE";
    private static MyIBoxDB _Instance;

    private DB db = null;
    private DB.AutoBox autoBox = null;//�ö�����Խ���CRUD������ʹ����ϲ���Ҫ�ͷŶ���

    private MyIBoxDB()
    {
        if (db == null)
        {
            DB.Root(Application.streamingAssetsPath);//���ݿ�洢·��
            db = new DB(1);//���ݿ��ַ������˵ID                     
            db.GetConfig().EnsureTable<BaseObject>(TABLE_BED_SCENE, "ObjectName(20)");//�Ƚ����ʹ��,���ұ������ΪObjectName������20
        }

        if (autoBox == null)
            autoBox = db.Open();
    }

    public static MyIBoxDB GetInstance()
    {
        if (_Instance == null)
            _Instance = new MyIBoxDB();

        return _Instance;
    }

    /// <summary>
    /// ɾ�����ݿ⣬IBoxDB��û��ֱ��ɾ��һ����Ľӿڣ�������������Եø�������~
    /// ע�⣺ɾ�����ݿ�֮ǰҪ�رո����ݿ�
    /// </summary>
    /// <param name="address">���ݿ��ַ</param>
    public void DeleteDataBase(int address)
    {
        iBoxDB.DBDebug.DDebug.DeleteDBFiles(address);
    }

    /// <summary>
    /// �洢һ���б������
    /// </summary>
    /// <param name="tableName">���ĸ��������</param>
    /// <param name="data">Ҫ�洢�����ݼ���</param>
    public void Save(string tableName, List<BaseObject> data)
    {
        IBox iBox = _Instance.GetBox();
        Binder binder = iBox.Bind(tableName);//�󶨱�

        foreach (BaseObject ob in data)
        {
            //�������֮ǰ�й��ü�¼����Update��û����Insert
            if (_Instance.autoBox.SelectCount("from " + tableName + " where ObjectName == ?", ob.ObjectName) <= 0)
                binder.Insert(ob);
            else
                binder.Update(ob);
        }

        iBox.Commit();
        iBox.Dispose();
    }

    /// <summary>
    /// �洢һ�����������
    /// </summary>
    /// <param name="tableName">���ĸ��������</param>
    /// <param name="ob">����</param>
    public void Save(string tableName, BaseObject ob)
    {
        if (_Instance.autoBox.SelectCount("from " + tableName + " where ObjectName == ?", ob.ObjectName) <= 0)
            _Instance.autoBox.Insert<BaseObject>(tableName, ob);
        else
            _Instance.autoBox.Update<BaseObject>(tableName, ob);
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="QL">QL���</param>
    /// <param name="param">QL����</param>
    /// <returns></returns>
    public List<BaseObject> Get(string QL, string param)
    {
        return _Instance.autoBox.Select<BaseObject>(QL, param);
    }

    /// <summary>
    /// IBox���Խ������ݿ���������
    /// </summary>
    private IBox GetBox()
    {
        return _Instance.autoBox.Cube();
    }
}

//������������
public class BaseObject : Dictionary<string, object>
{
    public string ObjectName
    {
        get
        {
            return (string)base["ObjectName"];
        }
        set
        {
            if (value.Length > 20)
            {
                throw new ArgumentOutOfRangeException();
            }
            base["ObjectName"] = value;
        }
    }
}