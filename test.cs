using System.Collections.Generic;
using UnityEngine;
using telescope;
using UnityEngine.Analytics;
using System;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Dictionary<string, object> valueDic = new()
            {
                { "fabulousString", "hello there" },
                { "sparklingInt", 1337 },
                { "tremendousLong", Int64.MaxValue },
                { "spectacularFloat", 0.451f },
                { "incredibleDouble", 0.000000000000000031337 },
                { "peculiarBool", new List<int>() { 1,2,3,4,5 } },
                { "ultimateTimestamp", DateTime.UtcNow }
            };


        //Debug.Log(JsonConvert.SerializeObject(intDic, Formatting.Indented, new ValueConverter()));
        Telescope.Send(new TelescopeEvent() { entityName = "deneme", id = "testUSer", type = "insert", value = valueDic });
        Telescope.Send(new List<TelescopeEvent>()
            {
                new() { entityName = "deneme", id = "testUSer", type = "insert", value = valueDic },
                new() { entityName = "qqqqqq", id = "testUSer", type = "update", value = valueDic }
            }
        );

        Telescope.Send("entityName", "testUSer", new() { { "name", "Tarik" }, { "status", 1 } });
        Telescope.Send("entityName", "testUSer", valueDic);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
