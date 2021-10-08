using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

public class Test : MonoBehaviour
{
    XmlNodeList xmlNodeList;
    XmlDocument xmlDoc;
    void Start()
    {
        int level = 3;
        TextReader textReader = new StreamReader(@"D:\abc\lv" + level + ".xml");
        xmlDoc = new XmlDocument();
        xmlDoc.Load(textReader);
        textReader.Close();
        xmlNodeList = xmlDoc.DocumentElement.ChildNodes; // ----> Read all childNode in file
        Debug.Log("-------------------------------------------------" + xmlNodeList.Count);
        List<ChuanXML> listChuanXML = new List<ChuanXML>();
        for (int i = 0; i < xmlNodeList.Count; i++)
        {
            XmlNode node = xmlNodeList[i];

            string col = node["col"].InnerText;
            string row = node["row"].InnerText;
            string b1x = node["b1x"].InnerText;
            string b1y = node["b1y"].InnerText;
            string b2x = node["b2x"].InnerText;
            string b2y = node["b2y"].InnerText;
            string bex = node["bex"].InnerText;
            string bey = node["bey"].InnerText;
            //Change pos
            string chan1x = node["chan1"] == null ? "0" : node["chan1"].InnerText;
            string chan1y = node[""] == null ? "0" : node[""].InnerText;
            string chan2x = node["chan2"] == null ? "0" : node["chan2"].InnerText;
            string chan2y = node[""] == null ? "0" : node[""].InnerText;
            string chanex = node[""] == null ? "0" : node[""].InnerText;
            string chaney = node[""] == null ? "0" : node[""].InnerText;

            string type = node["type"] == null ? "1" : node["type"].InnerText;
            string time = node["time"] == null ? "3" : node["time"].InnerText;
            string delay = node["delay"] == null ? "0.3" : node["delay"].InnerText;
            string thread = node["thre"] == null ? "1" : node["thre"].InnerText;

            string dele = node["dele"] == null ? "0" : node["dele"].InnerText;
            string timeturn = node["timeturn"] == null ? "0" : node["timeturn"].InnerText;
            //Debug.Log(col + " - " + row + " - " + b1x + " - " + b1y
            //     + " - " + b2x + " - " + b2y + " - " + bex + " - " + bey
            //     + " - " + chan1x + " - " + chan1y + " - " + chan2x + " - " + chan2y
            //     + " - " + chanex + " - " + chaney + " - " + type + " - " + time
            //     + " - " + delay + " - " + thread);

            AddNode(level, node.Name, new Vector2((float)Convert.ToDouble(col), (float)Convert.ToDouble(row)),
                new Vector2((float)Convert.ToDouble(b1x), (float)Convert.ToDouble(b1y)),
                new Vector2((float)Convert.ToDouble(b2x), (float)Convert.ToDouble(b2y)),
                new Vector2((float)Convert.ToDouble(bex), (float)Convert.ToDouble(bey)),
                new Vector2((float)Convert.ToDouble(chan1x), (float)Convert.ToDouble(chan1y)),
                new Vector2((float)Convert.ToDouble(chan2x), (float)Convert.ToDouble(chan2y)),
                new Vector2((float)Convert.ToDouble(chanex), (float)Convert.ToDouble(chaney)),
                Convert.ToInt16(type), Convert.ToInt16(thread),
                (float)Convert.ToDouble(time), (float)Convert.ToDouble(delay), Convert.ToInt16(dele),
                (float)Convert.ToDouble(timeturn));
        }
        Debug.Log("FILE SAVED!");

    }

    public static void AddNode(int level, string turn, Vector2 vmat, Vector2 vb1, Vector2 vb2,
            Vector2 vbe, Vector2 vchan1, Vector2 vchan2, Vector2 vchane,
            int vtype, int vthread, float vtime, float vdelay, int vdele, float vtimeturn)
    {
        string dir = @"D:\abc\lv"+level+"v.xml";
        XmlDocument xmlDoc = new XmlDocument();
        if (!System.IO.File.Exists(dir))
        {
            xmlDoc.LoadXml("<Level></Level>");
            xmlDoc.Save(dir);
        }
        TextReader textReader = new StreamReader(dir);
        xmlDoc.Load(textReader);
        XmlNode rootLevel = xmlDoc.DocumentElement;
        {
            XmlNode rootTurn = xmlDoc.CreateElement(turn);
            {
                XmlElement mat = xmlDoc.CreateElement("mat");
                mat.SetAttribute("x", "" + vmat.x);
                mat.SetAttribute("y", "" + vmat.y);
                rootTurn.AppendChild(mat);

                XmlElement b1 = xmlDoc.CreateElement("b1");
                b1.SetAttribute("x", "" + vb1.x);
                b1.SetAttribute("y", "" + vb1.y);
                rootTurn.AppendChild(b1);

                XmlElement b2 = xmlDoc.CreateElement("b2");
                b2.SetAttribute("x", "" + vb2.x);
                b2.SetAttribute("y", "" + vb2.y);
                rootTurn.AppendChild(b2);

                XmlElement be = xmlDoc.CreateElement("be");
                be.SetAttribute("x", "" + vbe.x);
                be.SetAttribute("y", "" + vbe.y);
                rootTurn.AppendChild(be);

                XmlElement chan1 = xmlDoc.CreateElement("chan1");
                chan1.SetAttribute("x", "" + vchan1.x);
                chan1.SetAttribute("y", "" + vchan1.y);
                rootTurn.AppendChild(chan1);

                XmlElement chan2 = xmlDoc.CreateElement("chan2");
                chan2.SetAttribute("x", "" + vchan2.x);
                chan2.SetAttribute("y", "" + vchan2.y);
                rootTurn.AppendChild(chan2);

                XmlElement chane = xmlDoc.CreateElement("chane");
                chane.SetAttribute("x", "" + vchane.x);
                chane.SetAttribute("y", "" + vchane.y);
                rootTurn.AppendChild(chane);

                XmlElement type = xmlDoc.CreateElement("type");
                type.SetAttribute("value", "" + vtype);
                rootTurn.AppendChild(type);

                XmlElement thread = xmlDoc.CreateElement("thread");
                thread.SetAttribute("value", "" + vthread);
                rootTurn.AppendChild(thread);

                XmlElement time = xmlDoc.CreateElement("time");
                time.SetAttribute("value", "" + vtime);
                rootTurn.AppendChild(time);

                XmlElement delay = xmlDoc.CreateElement("delay");
                delay.SetAttribute("value", "" + vdelay);
                rootTurn.AppendChild(delay);

                XmlElement dele = xmlDoc.CreateElement("dele");
                dele.SetAttribute("value", "" + vdele);
                rootTurn.AppendChild(dele);

                XmlElement timeturn = xmlDoc.CreateElement("timeturn");
                timeturn.SetAttribute("value", "" + vtimeturn);
                rootTurn.AppendChild(timeturn);

            }
            rootLevel.AppendChild(rootTurn);
        }

        textReader.Close();
        xmlDoc.Save(dir);
    }

    class ChuanXML
    {
        Vector2 mat;
        Vector2 b1;
        Vector2 b2;
        Vector2 be;
        Vector2 chan1;//thay doi vi tri dau tien
        Vector2 chan2;//thay doi vi tri thu 2
        Vector2 chane;//thay doi vi tri cuoi . mac dinh (0,0). x, y duong thi + . am thi -
        int type;
        int thread;
        float time;//Thoi gian thuc hien action
        float delay;//thoi gian delay giua moi lan de quai
        //Them moi
        int dele;//Xoa khi action xong - Mac dinh 0 - ko xoa
        float timeturn;//Sau bang nay giay se chuyen sang  turn khac - Mac dinh 0 - khi het vit moi chuyen


        public ChuanXML(Vector2 mat, Vector2 b1, Vector2 b2,
            Vector2 be, Vector2 chan1, Vector2 chan2, Vector2 chane,
            int type, int thread, float time, float delay, int dele, float timeturn)
        {
            this.mat = mat;
            this.b1 = b1;
            this.b2 = b2;
            this.be = be;
            this.chan1 = chan1;
            this.chan2 = chan2;
            this.chane = chane;
            this.type = type;
            this.thread = thread;
            this.time = time;
            this.delay = delay;
            this.dele = dele;
            this.timeturn = timeturn;
        }

        public ChuanXML()
        {
            this.mat = new Vector2();
            this.b1 = new Vector2();
            this.b2 = new Vector2();
            this.be = new Vector2();
            this.chan1 = new Vector2();
            this.chan2 = new Vector2();
            this.chane = new Vector2();
            this.type = 1;
            this.thread = 1;
            this.time = 2;
            this.delay = 0.3f;
            this.dele = 0;
            this.timeturn = 0;
        }
    }
}
