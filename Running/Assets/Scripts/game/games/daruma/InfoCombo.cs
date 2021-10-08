using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Daruma
{
    public class InfoCombo : MonoBehaviour
    {
        public Text txtCombo;

        public void SetTextCombo(string str)
        {
            txtCombo.text = str;
        }
    }
}

