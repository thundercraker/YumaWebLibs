using System;
using System.Collections.Generic;
using System.Reflection;
using YumaWebLib.Json;
using YumaWebLib.Http;

class MainClass
{
	public static void Main (string[] args)
	{
		JSONDemo ();
		GETDemo ();
		Console.WriteLine ("Done");
	}

	public static void JSONDemo() {
		JsonObject obj = JsonObject.JsonObjectFactoryFactoryBuilderFactory();
		JsonObject.DemoJsonObject result = obj.Parse<JsonObject.DemoJsonObject> (JsonObject.DemoJsonObjectJSON (), new JsonObject.DemoJsonObject());
		Console.WriteLine(obj.ToJSON (result));
		Console.WriteLine ("");
		List<JsonObject.ListDemoJsonObject> listResult = obj.Parse<List<JsonObject.ListDemoJsonObject>> (JsonObject.DemoListJsonObjectJSON (), new List<JsonObject.ListDemoJsonObject>());
		Console.WriteLine(obj.ListToJSON (listResult));
	}

	public static void GETDemo() {
		string request = GetRequest.RequestBuilder ()
			.AddParam ("name", "yumashish")
			.AddParam ("lastname", "subba")
			.AddParam ("password", "gaga2014")
			.AddParam ("address", "718 Muffin Apartment 269 Moo 4,\n Amphur Bang Bor, Samutprakarn Thailand")
			.AddParam ("id", 3456)
			.GetRequestString ();
		Console.WriteLine (request);
	}
}
