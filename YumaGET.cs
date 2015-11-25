using System;
using System.Collections.Generic;
using System.Reflection;

namespace YumaWebLib.Http
{
	public class GetRequest : ParameterizedRequest
	{
		GetRequest() : base() {}

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

		public new static GetRequest RequestBuilder() {
			return new GetRequest ();
		}

		public static GetRequest RequestBuilder(object requestObject) {
			return new GetRequest (requestObject);
		}

		public new GetRequest AddParam (string key, object value)
		{
			return (GetRequest) base.AddParam (key, value);
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
	}

	//

	/**/
}

