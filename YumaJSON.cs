using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/* Created by Yumashish Subba
* I cannot be bothered to make a license, please just drop my name in there somewhere
* so that the next unfortunate soul who has to read through our (or my, if you're that good) god awful code can know exactly
* who messed up their day.
* 
* IT WAS ME.
*/ 

namespace YumaWebLib.Json {
	public class JsonObject {
		public static bool BE_QUIET = false;

		JsonObject() {
			Log ("You can disable log messages by setting the bool value BE_QUIET to true");
		}


		public class DemoJsonObject
		{
			[YWLKey()]
			public int id;

			[YWLKey()]
			public string name;

			[YWLKey("ListAddr")]
			public List<string> list;

			[YWLKey("InnerObject")]
			public InnerObject obj;

			public class InnerObject
			{
				[YWLKey()]
				public int id;

				[YWLKey()]
				public List<WeHaveToGoDeeper> chinkystare;

				public class WeHaveToGoDeeper
				{
					[YWLKey()]
					public double french_horn;
				}
			}
		}
			
		public class ListDemoJsonObject {

			[YWLKey("id")]
			public int _id;

			[YWLKey()]
			public int uid;

			[YWLKey("public")]
			public int isPublic;

			[YWLKey()]
			public string name;

			[YWLKey()]
			public string creation_date;

			[YWLKey()]
			public string start_code;

			[YWLKey()]
			public string run_code;

			[YWLKey()]
			public string data;

		}

		public static string DemoJsonObjectJSON() {
			return "{\"id\":\"1\", \"name\":\"object name\", \"ListAddr\":[\"1\", \"2\", \"3\"], \"InnerObject\":{\"id\":\"42\", \"chinkystare\":[{\"french_horn\":\"234.34534\"}, {\"french\":\"44.454\"}]}}";
		}

		public static string DemoListJsonObjectJSON() {
			return "[{\"id\":\"4\",\"uid\":\"1\",\"public\":\"1\",\"name\":\"Starfighter\",\"data\":\"\",\"creation_date\":\"2015-10-21 06:40:54\",\"start_code\":null,\"run_code\":null},{\"id\":\"5\",\"uid\":\"1\",\"public\":\"0\",\"name\":\"Warfighter\",\"data\":\"\",\"creation_date\":\"2015-10-21 06:40:54\",\"start_code\":null,\"run_code\":null}]";
		}

		public static JsonObject JsonObjectFactoryFactoryBuilderFactory() {
			//harhar
			return new JsonObject();
		}

		//jsonify methods
		public string ToJSON(Object o) {
			
			return ObjectToJSON(o);
		}

		string FieldToJSON(FieldInfo field, object classObject) {
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
			Log ("-------------- Parsing field " + fieldName + " (" + field.FieldType + ") ---------------------");
			if (!isKey) {
				Log ("Field is not a Key!");
				Log ("-------------- Parsing field " + fieldName + " ---------------------");
				return "";
			}

			return ItemToJSON (field.GetValue (classObject), field.FieldType);
			//return null;
		}

		string ItemToJSON(object item, Type itemType) {

			Log ("--Extracting item value for type (" + itemType + ")--");
			//Type itemType = itemObject.GetType ();
			if (itemType.IsPrimitive || itemType.Equals (typeof(string))) {
				return "\"" + item + "\"";
			} else if (itemType.IsArray) {
				throw new Exception ("This library does not handle basic arrays, please use a List instead."); 
			} else if(itemType.IsGenericType) {
				Log ("Item is a generic type");
				if(itemType.GetGenericTypeDefinition() == typeof(List<>)) {
					//Type listType = itemType.GetGenericArguments()[0];
					return ListToJSON((IList) item);
				}
			} else {
				Log ("Item is an object type");
				//nested jsonobject object
				object nestedObject = Activator.CreateInstance(itemType);
				return ObjectToJSON(item);
			}
			throw new Exception ("Field " + itemType + " cannot be handled by this library, you'll have to implement this yourself");
		}

		string ObjectToJSON(object item) {
			string final = "{";
			int cut = 0;
			foreach (FieldInfo field in item.GetType().GetFields()) {
				final += "\"" + field.Name + "\" : " + FieldToJSON(field, item) + ", ";
				cut = 2;
			}
			final = final.Substring (0, final.Length - cut);
			return final + "}";
		}

		public string ListToJSON(IList list) {
			string final = "[";

			Type listType = list.GetType().GetGenericArguments()[0];
			int cut = 0;
			foreach (object item in list) {
				final += ItemToJSON (item, listType) + ", ";
				cut = 2;
			}
			final = final.Substring (0, final.Length - cut);

			return final + "]";
		}

		//-----------Parsing methods-----------

		public T Parse<T>(string json, T classObject) {
			JSONNode root = JSON.Parse (json);
			//if array
			string firstChar = json.Substring(0, 1);
			if (firstChar == "{") {
				return (T)ExtractObject (root, classObject);
			} else if (firstChar == "[") {
				if (classObject.GetType ().IsGenericType &&
				    classObject.GetType ().GetGenericTypeDefinition () == typeof(List<>))
					return (T)ExtractList (root, (IList) classObject);
				else
					throw new Exception ("The json object is an array, you must pass a List<> object");
			} else {
				return (T)ExtractItem (root, typeof(T));
			}
		}

		void ExtractField(JSONNode node, FieldInfo field, ref object classObject) {
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
			Log ("-------------- Parsing field " + fieldName + " (" + field.FieldType + ") ---------------------");
			if (!isKey) {
				Log ("Field is not a Key!");
				Log ("-------------- Parsing field " + fieldName + " ---------------------");
				return;
			}
			Log ("Raw value for field name: " + node [fieldName]);
			object fieldObject;
			if (node[fieldName] != null) {
				fieldObject = ExtractItem (node [fieldName], field.FieldType);
				Log ("Field resolved value: " + fieldObject);
			} else {
				if(field.FieldType.Equals(typeof(string))) {
					string fo = "";
					fieldObject = fo;
				} else {
					try{
						fieldObject = Activator.CreateInstance (field.FieldType);
					} catch (MissingMethodException mme) {
						fieldObject = null;
					}
				}
				Log ("No json value for this key + " + fieldName + " +, using default value");
			}
			field.SetValue(classObject, Convert.ChangeType(fieldObject, field.FieldType));

			Log ("-------------- Parsing field " + fieldName + " ---------------------");
		}

		object ExtractItem(JSONNode node, Type itemType) {
			Log ("--Extracting item value for type (" + itemType + ")--");
			//Type itemType = itemObject.GetType ();
			if (itemType.IsPrimitive) {
				Log ("Item is a primitive type, typecode " + Type.GetTypeCode(itemType));
				if (IsNumericTypeNullable (itemType)) {
					Log ("Item is numeric");
					Double d;
					if (Double.TryParse (node.Value, out d)) {
						return Convert.ChangeType (d, itemType);
					} else {
						throw new Exception ("The provided JSON Node does not contain a readable numeric field"
						+ ". [Be advised that because of my laziness, all numeric fields are initially casted into" +
						"a double then into other data types. If the numeric value in your json is say, a BigInt or " +
							"BigRational you will need to implement this yourself.] [Actual Value: " + node.Value + "]");
					}
				} else if (itemType.Equals (typeof(bool))) {
					Log ("Item is boolean");
					return node.AsBool;
				} else {
					Log ("Item is unknown primitive");
				}
			} else if (itemType.IsArray) {
				throw new Exception ("This library does not handle basic arrays, please use a List instead.");
			} else if (itemType.Equals (typeof(string))) {
				Log ("Item is a string type");
				return node.Value;
			} else if(itemType.IsGenericType) {
				Log ("Item is a generic type");
				if(itemType.GetGenericTypeDefinition() == typeof(List<>)) {
					//Type listType = itemType.GetGenericArguments()[0];
					return ExtractList(node, (IList) Activator.CreateInstance(itemType));
				}
			} else {
				Log ("Item is an object type");
				//nested jsonobject object
				object nestedObject = Activator.CreateInstance(itemType);
				return ExtractObject(node, nestedObject);
			}
			throw new Exception ("Field " + itemType + " cannot be handled by this library, you'll have to implement this yourself");
			//return null;
		}

		object ExtractObject(JSONNode node, object classObject) {
			Log ("========== Parsing object " + classObject.GetType ().Name + " =====================");
			Log ("Fields found (" + classObject.GetType().GetFields().Length + ")");
			foreach(FieldInfo field in classObject.GetType().GetFields()) {
				Log (field.Name);
				ExtractField(node, field, ref classObject);
			}
			Log ("========== Parsed object " + classObject.GetType ().Name + " ======================");
			return classObject;
		}

		IList ExtractList(JSONNode node, IList listObject) {
			Log ("========== Parsing list " + listObject.GetType ().Name + " =====================");
			Type listType = listObject.GetType().GetGenericArguments()[0];
			foreach (JSONNode item in node.AsArray.Childs) {
				listObject.Add(ExtractItem(item, listType));
			}

			return listObject;
			Log ("========== Parsed list " + listObject.GetType ().Name + " ======================");
		}

		public static bool IsNumericTypeNullable(Type t) {
			return IsNumericType (Type.GetTypeCode (t)) || 
				IsNumericType (Type.GetTypeCode(Nullable.GetUnderlyingType (t)));
		}

		public static bool IsNumericType(TypeCode tc)
		{   
			switch (tc)
			{
			case TypeCode.Byte:
			case TypeCode.SByte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Decimal:
			case TypeCode.Double:
			case TypeCode.Single:
				return true;
			default:
				return false;
			}
		}

		public static void Log(string m) {
			if(!BE_QUIET)
				Console.WriteLine (m);
		}
	}
}