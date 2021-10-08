using Assets.Scripts.Town;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Assets.Scripts.Store
{
    public class ShopDataMission : ItemAbstract
    {
        public List<ProductData> listProducts;
        public bool isUsingItem;
        public int targetItem;
        public int targetSells;
        ProductData tempProduct;
        int countIndex;
        public void getDataFromXML(XmlNode nodeShop)
        {
            isUsingItem = false;
            targetItem = 0;
            targetSells = 0;
            listProducts = new List<ProductData>();
            countIndex = 0;
            if (nodeShop.Attributes["targetSells"] != null)
                targetSells = Convert.ToInt16(nodeShop.Attributes["targetSells"].Value);
            foreach (XmlNode childNode in nodeShop.ChildNodes)
            {
                if (childNode.Name.Equals("Product"))
                {
                    tempProduct = new ProductData(Convert.ToInt16(childNode.Attributes[0].Value), countIndex);
                    if (childNode.Attributes["startNumber"] != null)
                        tempProduct.startNumber = Convert.ToInt16(childNode.Attributes["startNumber"].Value);
                    if (childNode.Attributes["targetSell"] != null)
                        tempProduct.targetSell = Convert.ToInt16(childNode.Attributes["targetSell"].Value);
                    if (childNode.Attributes["targetProduction"] != null)
                        tempProduct.targetProduction = Convert.ToInt16(childNode.Attributes["targetProduction"].Value);
                    if (childNode.Attributes["numberRequest"] != null)
                        tempProduct.numberRequest = Convert.ToInt16(childNode.Attributes["numberRequest"].Value);
                    countIndex++;
                    listProducts.Add(tempProduct);
                }
                else
                {
                    if (childNode.Attributes["startNumber"] != null)
                        isUsingItem = Convert.ToBoolean(childNode.Attributes["startNumber"].Value);
                    if (childNode.Attributes["targetItem"] != null)
                        targetItem = Convert.ToInt16(childNode.Attributes["targetItem"].Value);
                }
            }
        }
        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return targetItem;//tổng sản phẩm bán theo thời tiết

            }
            else
            {
                return targetSells;//Tổng sản phẩm bán được
            }
        }
        public override int getType()
        {
            return 0;
        }
    }
    public class ProductData : ItemAbstract
    {
        public int idProduct;
        public int startNumber;
        public int targetSell;
        public int targetProduction;
        public int numberRequest;
        public ProductData(int id, int index, bool isFromShop = false)
        {
            idProduct = id;
            startNumber = 0;
            targetSell = 0;
            targetProduction = 0;
            numberRequest = -1;
            this.index = index;
            if (isFromShop) numberRequest = -999;
        }

        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return targetProduction;//mục tiêu sản xuất sản phẩm
            }
            else
            {
                return targetSell;//mục tiêu bán số lượng sản phẩm
            }
        }
        public override int getType()
        {
            return idProduct;
        }
    }
}
