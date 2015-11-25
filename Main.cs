using System;
using System.Collections.Generic;
using System.Reflection;

namespace YumaJsonLib
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			JsonObject obj = JsonObject.JsonObjectFactoryFactoryBuilderFactory();
			JsonObject.DemoJsonObject result = obj.Parse<JsonObject.DemoJsonObject> (JsonObject.DemoJsonObjectJSON (), new JsonObject.DemoJsonObject());
			Console.WriteLine(obj.ToJSON (result));
			Console.WriteLine ("");
			List<JsonObject.ListDemoJsonObject> listResult = obj.Parse<List<JsonObject.ListDemoJsonObject>> (JsonObject.DemoListJsonObjectJSON (), new List<JsonObject.ListDemoJsonObject>());
			Console.WriteLine(obj.ListToJSON (listResult));
			Console.WriteLine ("Done");
		}
	}
}
