using iBoxDB.LocalServer;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现一个Save和Read方法 
/// </summary>
public class MyIBoxDB
{
    public const string TABLE_BED_SCENE = "TABLE_BED_SCENE";
    private static MyIBoxDB _Instance;

    private DB db = null;
    private DB.AutoBox autoBox = null;//该对象可以进行CRUD操作，使用完毕不需要释放对象

    private MyIBoxDB()
    {
        if (db == null)
        {
            DB.Root(Application.streamingAssetsPath);//数据库存储路径
            db = new DB(1);//数据库地址，或者说ID                     
            db.GetConfig().EnsureTable<BaseObject>(TABLE_BED_SCENE, "ObjectName(20)");//先建表后使用,并且表的主键为ObjectName，长度20
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
    /// 删除数据库，IBoxDB中没有直接删除一个表的接口，所以这个方法显得格外亲切~
    /// 注意：删除数据库之前要关闭该数据库
    /// </summary>
    /// <param name="address">数据库地址</param>
    public void DeleteDataBase(int address)
    {
        iBoxDB.DBDebug.DDebug.DeleteDBFiles(address);
    }

    /// <summary>
    /// 存储一个列表的数据
    /// </summary>
    /// <param name="tableName">向哪个表存数据</param>
    /// <param name="data">要存储的数据集合</param>
    public void Save(string tableName, List<BaseObject> data)
    {
        IBox iBox = _Instance.GetBox();
        Binder binder = iBox.Bind(tableName);//绑定表

        foreach (BaseObject ob in data)
        {
            //如果表中之前有过该记录，则Update；没有则Insert
            if (_Instance.autoBox.SelectCount("from " + tableName + " where ObjectName == ?", ob.ObjectName) <= 0)
                binder.Insert(ob);
            else
                binder.Update(ob);
        }

        iBox.Commit();
        iBox.Dispose();
    }

    /// <summary>
    /// 存储一个对象的数据
    /// </summary>
    /// <param name="tableName">向哪个表存数据</param>
    /// <param name="ob">数据</param>
    public void Save(string tableName, BaseObject ob)
    {
        if (_Instance.autoBox.SelectCount("from " + tableName + " where ObjectName == ?", ob.ObjectName) <= 0)
            _Instance.autoBox.Insert<BaseObject>(tableName, ob);
        else
            _Instance.autoBox.Update<BaseObject>(tableName, ob);
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="QL">QL语句</param>
    /// <param name="param">QL参数</param>
    /// <returns></returns>
    public List<BaseObject> Get(string QL, string param)
    {
        return _Instance.autoBox.Select<BaseObject>(QL, param);
    }

    /// <summary>
    /// IBox可以进行数据库的事务操作
    /// </summary>
    private IBox GetBox()
    {
        return _Instance.autoBox.Cube();
    }
}

//基本数据类型
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