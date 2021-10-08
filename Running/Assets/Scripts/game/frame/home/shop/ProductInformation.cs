using UnityEngine;
using System.Collections;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public class ProductInformation
{
	public string productName;
	public ProductType productType;
	public string productId;
	public IDs ids;

	public ProductInformation Init (string productName, ProductType productType, string productId)
	{
		ProductInformation productInformation = new ProductInformation ();
		productInformation.productName = productName;
		productInformation.productType = productType;
		productInformation.productId = productId;
		productInformation.ids = new IDs () { { 
				productInformation.productId, 
				AppleAppStore.Name
			}, {
				productInformation.productId,
				GooglePlay.Name
			}
		};
		return productInformation;
	}
}
