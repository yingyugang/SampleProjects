using System;

[Serializable]
public class RecyclePoint
{
	public int lg;
	public int ur;
	public int sr;
	public int r;
	public int n;
	public RecyclePoint oldRecyclePoint;

	public int this [int index] {
		get {
			switch (index) {
			case 0:
				return lg;
			case 1:
				return ur;
			case 2:
				return sr;
			case 3:
				return r;
			case 4:
				return n;
			default:
				return 0;
			}
		}
		set { 
			switch (index) {
			case 0:
				lg = value;
				break;
			case 1:
				ur = value;
				break;
			case 2:
				sr = value;
				break;
			case 3:
				r = value;
				break;
			case 4:
				n = value;
				break;
			}
		}
	}

	public RecyclePoint Clone ()
	{
		oldRecyclePoint = new RecyclePoint ();
		oldRecyclePoint.lg = this.lg;
		oldRecyclePoint.ur = this.ur;
		oldRecyclePoint.sr = this.sr;
		oldRecyclePoint.r = this.r;
		oldRecyclePoint.n = this.n;
		return oldRecyclePoint;
	}
}