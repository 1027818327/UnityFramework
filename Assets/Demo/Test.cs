using UnityEngine;
using System.Collections;
using iBoxDB.LocalServer;
using System.Collections.Generic;

//Insert必须表里没有相同ID的记录；Update必须表里有相同ID的记录
public class Test : MonoBehaviour
{
    void Start()
    {
        //TestUsingAutoBox();
        //TestUsingBox();

        ShowResult();
    }

    private void TestUsingBox()
    {
        List<BaseObject> data = new List<BaseObject>();

        BaseObject item = new BaseObject() { ObjectName = "Player8" };
        item["position_x"] = 2;
        item["rotation_x"] = 34.2f;
        item["enable"] = false;
        item["tag"] = "item1_tag";

        BaseObject item2 = new BaseObject() { ObjectName = "Player9" };
        item2["position_x"] = 23;
        item2["rotation_x"] = 45.2f;
        item2["enable"] = false;
        item2["tag"] = "item1_tag";

        data.Add(item);
        data.Add(item2);

        MyIBoxDB.GetInstance().Save(MyIBoxDB.TABLE_BED_SCENE, data);
    }

    private void TestUsingAutoBox()
    {
        BaseObject item = new BaseObject() { ObjectName = "Player6" };
        item["position_x"] = 34;
        item["rotation_x"] = 89.5f;
        item["enable"] = true;
        item["tag"] = "item1_tag";

        MyIBoxDB.GetInstance().Save(MyIBoxDB.TABLE_BED_SCENE, item);
    }

    private void ShowResult()
    {
        foreach (BaseObject mItem in MyIBoxDB.GetInstance().Get("from " + MyIBoxDB.TABLE_BED_SCENE + " where ObjectName == ?", "Player8"))//只支持传参
        {
            int position_x = (int)mItem["position_x"];
            float rotation_x = (float)mItem["rotation_x"];
            bool enable = (bool)mItem["enable"];
            string tag = mItem["tag"] as string;

            string s = "position_x = " + position_x + "  rotation_x = " + rotation_x + "  enable = " + enable + "  tag = " + tag;
            print(s);
        }


        foreach (BaseObject mItem in MyIBoxDB.GetInstance().Get("from " + MyIBoxDB.TABLE_BED_SCENE, null))//只支持传参
        {
            foreach (KeyValuePair<string, object> kvp in mItem)
            {
                Debug.Log(kvp.Key);
            }

            int position_x = (int)mItem["position_x"];
            float rotation_x = (float)mItem["rotation_x"];
            bool enable = (bool)mItem["enable"];
            string tag = mItem["tag"] as string;

            string s = "position_x = " + position_x + "  rotation_x = " + rotation_x + "  enable = " + enable + "  tag = " + tag;
            print(s);
        }
    }
}