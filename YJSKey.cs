namespace YumaJsonLib
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public class YJSKey : System.Attribute
	{
		public string name;

		public YJSKey() {
			this.name = "";
		}

		public YJSKey(string name) {
			this.name = name;
		}

		public YJSKey(int index) {
			this.name = index + "";
		}
	}
}

