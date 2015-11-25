using System;
using System.Collections.Generic;

namespace YumaWebLib
{
	public class ParameterizedRequest : SilenceableLoggingObject
	{
		protected Dictionary<string, object> parameters;

		protected ParameterizedRequest() {
			parameters = new Dictionary<string, object> ();
		}

		public static ParameterizedRequest RequestBuilder() {
			return new ParameterizedRequest ();
		}

		public virtual ParameterizedRequest AddParam(string key, object value) {
			parameters.Add (key, value);
			return this;
		}
	}
}

