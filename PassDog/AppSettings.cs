
using System;
using System.Collections.Generic;
using System.Xml;

public class AppSettings
{
    private Dictionary<string, string> dict;
    public AppSettings(string path)
    {
        dict = new Dictionary<string, string>();
        string filename = path;
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(filename);
        XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;
        foreach (XmlElement element in topM)
        {
            if (element.Name.ToLower() == "appsettings")
            {
                XmlNodeList nodelist = element.ChildNodes;
                if (nodelist.Count > 0)
                {
                    foreach (XmlElement el in nodelist)
                    {
                        if (!dict.ContainsKey(el.Attributes["key"].Value))
                        {
                            dict.Add(el.Attributes["key"].Value, el.Attributes["value"].Value);
                        }
                        else
                        {
                            dict[el.Attributes["key"].Value] = el.Attributes["value"].Value;
                        }
                    }
                }
            }
        }
        xmldoc.Save(filename);
    }
    private void Add(string key, string value)
    {
        if (!this.dict.ContainsKey(key))
        {
            this.dict.Add(key, value);
        }
        else
        {
            this.dict[key] = value;
        }

    }

    public string this[string key]
    {
        get
        {
            if (!this.dict.ContainsKey(key))
            {
                throw new Exception("404");
            }
            return this.dict[key];
        }
        set
        {
            if (this.dict.ContainsKey(key))
            {
                this.dict[key] = value;
            }
            else
            {
                this.dict.Add(key, value);
            }
        }
    }


}
