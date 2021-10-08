using UnityEngine;
using System.Collections;

namespace Swimming 
{

	public class Wood :  Enemy
	{

		// Use this for initialization
		protected override void Start () 
		{
			base.Start();
		}

		public override void Init()
		{
			base.Init();
		}

		protected override void RandomMovemenType()
		{
			//m_MovementType = MovementType.Straight;
		}

		// Update is called once per frame
		protected override void Update ()
		{
			base.Update();
		}

		protected override void UpdateMovement ()
		{
			base.UpdateMovement();
		}

		protected override void OnTriggerEnter2D(Collider2D other)
		{
			base.OnTriggerEnter2D(other);
		}
	}

}