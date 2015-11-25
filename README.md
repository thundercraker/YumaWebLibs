# YumaJSONLib
Shameless copy paste of Google JSON reader. Not as good ofc, but it werks.

Doc

Look at the example classes within the Library class. It's all pretty straightforward, but as a short explaination:

#Reading

Create a class, then use the [YJSKey()] attribute to define it as a key within the JSON object. For example

{ "bla" : "bla bla" }

will corelate to

public class Jobj {
    [YJSKey()]
    public string bla;
}

if you want the member field to have a different name use [YJSKey("bla")] and set the member name to whatever you wish.

Once you've set up the relevant class, get a JSONObject instance and use the Parse function. 

> string jsonString = "{ \"bla\" : \"bla bla\" }";
> JsonObject obj = JsonObject.JsonObjectFactoryFactoryBuilderFactory();
> Jobj result = obj.Parse<Jobj>(jsonString, new Jobj());

#Writing

Assuming you have the earlier Jobj (result) and JsonObject (obj)

> string json = obj.ToJSON(resutlt);

will give you

> { "bla" : "bla bla" }

#Warnings

1) Use List<T> not arrays, I couldn't be bothered to do two implementations. :3 Donot use any other IList implementation either.
2) Methods have to be public, if you want private values you'll be to make a branch/edit and allow the reflection of non-public members with Flags in GetFields

