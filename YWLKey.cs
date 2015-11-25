namespace YumaWebLib
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class YWLKey : System.Attribute
	{
		public string name;

		public YWLKey() {
			this.name = "";
		}

		public YWLKey(string name) {
			this.name = name;
		}

		public YWLKey(int index) {
			this.name = index + "";
		}
	}
}

