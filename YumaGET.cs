using System;
using System.Collections.Generic;
using System.Reflection;

namespace YumaWebLib.Http
{
	public class GetRequest
	{
		Dictionary<string, object> parameters;

		GetRequest ()
		{
			parameters = new Dictionary<string, object> ();
		}

		GetRequest (object requestObject) {
			parameters = new Dictionary<string, object> ();
			foreach(FieldInfo field in requestObject.GetType().GetFields()) {
				//Log (field.Name);
				string fieldName = field.Name;
				bool isKey = false;
				Attribute[] attrs = Attribute.GetCustomAttributes (field);
				foreach (Attribute attr in attrs) {
					if(attr is YWLKey) {
						isKey = true;
						YWLKey key = (YWLKey) attr;
						if(key.name.Length > 0) {
							fieldName = key.name;
							break;
						}
					}
				}
				if (isKey)
					parameters.Add (fieldName, field.GetValue (requestObject));
			}
		}

		public static GetRequest GetRequestBuilder() {
			return new GetRequest ();
		}

		public static GetRequest GetRequestBuilder(object requestObject) {
			return new GetRequest (requestObject);
		}

		public GetRequest AddParam(string name, object value) {
			parameters.Add (name, value);
			return this;
		}

		public string GetRequestString() {
			string req = "";
			foreach (KeyValuePair<string, object> kvp in parameters) {
				string key = kvp.Key;
				string value = kvp.Value + "";

				req += Uri.EscapeUriString(key) + "=" + Uri.EscapeUriString(value) + "&";
			}
			req = (req.Length > 0) ? req.Substring(0, req.Length - 2) : req;
			return req;
		}

		public static bool BE_QUIET = false;

		public static void Log(string m) {
			if(!BE_QUIET)
				Console.WriteLine (m);
		}
	}

	//

	/**/
}

