﻿Liersch.Json - JSON Support for .NET
====================================

Liersch.Json provides support for parsing and generating JSON expressions. The library is written in pure C# 3.0 and should work for most .NET platforms. At the moment the following platforms are directly supported:

- .NET Framework 3.5, 4.5 and Mono
- .NET Core 1.0
- .NET Standard 1.0
- .NET Micro Framework 4.4 (excluding reflection-based features)


Getting Started
---------------

- "Liersch.Json.sln" - demo solution based on NET Framework 4.5
- "Liersch.Json_net35.csproj" - library project for .NET Framework 3.5 and Mono
- "Liersch.Json_net45.csproj" - library project for .NET Framework 4.5 and Mono
- "Liersch.Json_netcoreapp1.0.csproj" - library project for .NET Core 1.0
- "Liersch.Json_netstandard1.0.csproj" - library project for .NET Standard 1.0
- "Liersch.Json_netmf.csproj" - library project for .NET Micro Framework 4.4


SLJsonParser
------------

The parser class SLJsonParser is easy to use. The input JSON expression should have a correct format. Otherwise a SLJsonException is thrown. In JSON expressions strings must be delimited with double-quotation marks. The parser also accepts single-quotation marks.

```cs
public static void RunExample1()
{
  string jsonExpression=@"
  {
    addressBook: [
      {lastName: 'Average', firstName: 'Joe'},
      {lastName: 'Doe', firstName: 'Jane'},
      {lastName: 'Smith', firstName: 'John'}
    ]
  }";

  SLJsonNode root=SLJsonParser.Parse(jsonExpression);
  SLJsonNode book=root["addressBook"];
  if(book.IsArray)
  {
    int c=book.Count;
    for(int i=0; i<c; i++)
    {
      SLJsonNode entry=book[i];
      string ln=entry["lastName"];
      string fn=entry["firstName"];
      Console.WriteLine(fn+" "+ln);
    }
  }
}
```


SLJsonNode
----------

The parser result is an instance of SLJsonNode. It can be used to analyze the parsed JSON expression. SLJsonNode implements IEnumerable. For arrays and objects all sub nodes are enumerated. In addition there is a property Names. It can be used for objects to enumerate the property names.

```cs
public static void RunExample2()
{
  string jsonExpression=RetrieveJsonExample();
  PrintNode(SLJsonParser.Parse(jsonExpression), 0);
}

static void PrintNode(SLJsonNode node, int level)
{
  if(level<=0)
    level=1;

  switch(node.NodeType)
  {
    case SLJsonNodeType.Array:
      Console.WriteLine("(Array)");
      foreach(SLJsonNode item in node)
      {
        Indent(level);
        PrintNode(item, level+1);
      }
      break;

    case SLJsonNodeType.Object:
      Console.WriteLine("(Object)");
      foreach(string name in node.Names)
      {
        Indent(level);
        Console.Write(name+" = ");
        PrintNode(node[name], level+1);
      }
      break;

    case SLJsonNodeType.Boolean:
    case SLJsonNodeType.Number:
    case SLJsonNodeType.String:
      Console.WriteLine(node.AsString+" ("+node.NodeType.ToString()+")");
      break;

    default:
      Console.WriteLine("("+node.NodeType.ToString()+")");
      break;
  }
}

static void Indent(int level)
{
  Console.Write(new StringBuilder().Append(' ', level*2));
}
```

The following properties can be used to read and write values: AsBoolean, AsInt32, AsInt64, AsDouble and AsString. If on reading a property, the value cannot be converted to the corresponding data type, the default value of the data type is returned instead.

If accessing a missing object or if using an invalid array index, no exception is thrown. Instead an empty node is returned. If a value setter is used for a not existing value, the value is created. SLJsonNode can also be used to create new JSON expressions.

```cs
public static void RunExample3()
{
  SLJsonNode root=new SLJsonNode();
  root["addressBook"]=CreateAddressBook();
  Console.WriteLine(root.AsJson);
}

static SLJsonNode CreateAddressBook()
{
  SLJsonNode book=new SLJsonNode();

  book[0]["LastName"]="Average";
  book[0]["firstName"]="Joe";

  book[1]["LastName"]="Doe";
  book[1]["firstName"]="Jane";

  book[2]["LastName"]="Smith";
  book[2]["firstName"]="John";

  return book;
}
```


SLJsonMonitor
-------------

The function SLJsonNode.CreateMonitor can be used to create an instance of class SLJsonMonitor. It's only allowed for root nodes and it must not be called several times.

Property SLJsonMonitor.IsModified is set to true on any change.

Property SLJsonMonitor.IsReadOnly can be used to disallow changing any node.

If passing root nodes to external code, CreateMonitor should always be used before. Otherwise the external code could cause unexpected side effects.


SLJsonWriter
------------

The writer class SLJsonWriter has a very small footprint. It has a good performance, but no checks for incorrect use.


SLJsonSerializer and SLJsonDeserializer
---------------------------------------

The classes SLJsonSerializer and SLJsonDeserializer are based on reflection. Fields and properties to be processed by serialization and deserialization must be marked with the attribute SLJsonMemberAttribute. Only public fields and properties should be marked with this attribute. For deserialization a public standard constructor is required.

```cs
class Example
{
  [SLJsonMember("IntegerArray", SLJsonMemberType.ValueArray)]
  public int[] IntegerArray;

  [SLJsonMember("StringValue")]
  public string StringValue;

  public string NotSerializedString;
}
```

In the following example an instance of a serializable class is created, serialized and deserialized again.

```cs
Example e1=new Example();
e1.IntegerArray=new int[] { 10, 20, 30, 700, 800 };
e1.StringValue="Example Text";
e1.NotSerializedString="Other Text";

string json=new SLJsonSerializer().SerializeObject(e1).ToString();
Example e2=new SLJsonDeserializer().Deserialize<Example>(json);
```

The reflection-based serialization and deserialization are unavailable for the .NET Micro Framework.


License
-------

Consider the license terms for the use of this software in whole or in part. The license terms are in file "Liersch.Json_License.txt".


Copyright
---------

Copyright © 2013-2017 Dipl.-Ing. (BA) Steffen Liersch  
http://www.steffen-liersch.de/

The software is maintained and published here:  
https://github.com/steffen-liersch/liersch.json
