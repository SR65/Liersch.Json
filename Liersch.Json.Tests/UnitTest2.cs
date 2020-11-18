﻿/*--------------------------------------------------------------------------*\
::
::  Copyright © 2013-2020 Steffen Liersch
::  https://www.steffen-liersch.de/
::
\*--------------------------------------------------------------------------*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Liersch.Json.Tests
{
  [TestClass]
  public class UnitTest2
  {
    [TestMethod]
    public void TestSystematically1()
    {
      JsonNodeType nt;

      nt=JsonNodeType.Missing;
      Check1("{other: 123}", false, false, false, false, nt, false, 0, 0, 0, null);

      nt=JsonNodeType.Null;
      Check1("{value: null}", true, false, false, false, nt, false, 0, 0, 0, null);

      nt=JsonNodeType.Boolean;
      Check1("{value: false}", false, false, false, true, nt, false, 0, 0, 0, "false");
      Check1("{value: true}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1("{value: true }", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1("{value:true}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" {value:true}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { value:true}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { value :true}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { value : true}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { value : true }", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { value : true } ", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { value :\ttrue\t}\t", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1("{\"value\":true}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { \"value\" : true } ", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1(" { 'value' : true } ", false, false, false, true, nt, true, 1, 1, 1, "true"); // Single-quotation marks are not allowed for JSON expressions

      nt=JsonNodeType.Number;
      Check1("{value: 123}", false, false, false, true, nt, true, 123, 123, 123, "123");
      Check1("{value: 1.23}", false, false, false, true, nt, true, 1, 1, 1.23, "1.23");
      Check1("{value: 1.89}", false, false, false, true, nt, true, 2, 2, 1.89, "1.89");
      Check1("{value: 0.123}", false, false, false, true, nt, false, 0, 0, 0.123, "0.123");
      Check1("{value: .123}", false, false, false, true, nt, false, 0, 0, 0.123, "0.123");
      Check1("{value: 1e-100}", false, false, false, true, nt, false, 0, 0, 1e-100, "1E-100");
      Check1("{value: 1.23e-100}", false, false, false, true, nt, false, 0, 0, 1.23e-100, "1.23E-100");
      Check1("{value: 1e+100}", false, false, false, true, nt, true, 0, 0, 1e+100, "1E+100");
      Check1("{value: 1.23e+100}", false, false, false, true, nt, true, 0, 0, 1.23e+100, "1.23E+100");

      nt=JsonNodeType.String;
      Check1("{value: \"text\"}", false, false, false, true, nt, false, 0, 0, 0, "text");
      Check1("{value: \"text with \\\" escape sequence\"}", false, false, false, true, nt, false, 0, 0, 0, "text with \" escape sequence");
      Check1("{value: \"text with \\\' escape sequence\"}", false, false, false, true, nt, false, 0, 0, 0, "text with \' escape sequence");
      Check1("{value: \'text with \\\' escape sequence\'}", false, false, false, true, nt, false, 0, 0, 0, "text with \' escape sequence");
      Check1("{value: \"Special_\\0\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_\0");
      Check1("{value: \"Special_\\10\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_\b");
      Check1("{value: \"Special_\\108\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_\b8");
      Check1("{value: \"Special_\\101\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_A");
      Check1("{value: \"Special_\\1010\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_A0");
      Check1("{value: \"Special_\\1018\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_A8");
      Check1("{value: \"Special_\\u0041\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_A");
      Check1("{value: \"Special_\\\\_\\/_/_\\b\\t\\n\\v\\f\\r\"}", false, false, false, true, nt, false, 0, 0, 0, "Special_\\_/_/_\b\t\n\v\f\r");
      Check1("{value: \"null\"}", false, false, false, true, nt, false, 0, 0, 0, "null");
      Check1("{value: \"false\"}", false, false, false, true, nt, false, 0, 0, 0, "false");
      Check1("{value: \"FALSE\"}", false, false, false, true, nt, false, 0, 0, 0, "FALSE");
      Check1("{value: \"true\"}", false, false, false, true, nt, true, 1, 1, 1, "true");
      Check1("{value: \"TRUE\"}", false, false, false, true, nt, false, 0, 0, 0, "TRUE");
      Check1("{value: \"123\"}", false, false, false, true, nt, true, 123, 123, 123, "123");
      Check1("{value: 'single-quoted'}", false, false, false, true, nt, false, 0, 0, 0, "single-quoted"); // Single-quotation marks are not allowed for JSON expressions

      try
      {
        Check1("{value: INVALID}", false, false, false, true, JsonNodeType.Number, false, 0, 0, 0, "INVALID");
        Assert.Fail();
      }
      catch(JsonException)
      {
        // Ignored
      }
    }

    [TestMethod]
    public void TestSystematically2()
    {
      JsonNodeType nt;

      nt=JsonNodeType.Boolean;
      Check2(false, false, false, false, true, nt, false, 0, 0, 0, "false");
      Check2(true, false, false, false, true, nt, true, 1, 1, 1, "true");

      nt=JsonNodeType.Number;
      Check2(123, false, false, false, true, nt, true, 123, 123, 123, "123");
      Check2(1.23, false, false, false, true, nt, true, 1, 1, 1.23, "1.23");
      Check2(1.89, false, false, false, true, nt, true, 2, 2, 1.89, "1.89");
      Check2(1e-100, false, false, false, true, nt, false, 0, 0, 1e-100, "1E-100");
      Check2(1.23e-100, false, false, false, true, nt, false, 0, 0, 1.23e-100, "1.23E-100");
      Check2(1e+100, false, false, false, true, nt, true, 0, 0, 1e+100, "1E+100");
      Check2(1.23e+100, false, false, false, true, nt, true, 0, 0, 1.23e+100, "1.23E+100");
    }

    void Check1(
      string json,
      bool isNull, bool isArray, bool isObject, bool isValue, JsonNodeType valueType,
      bool valueBoolean, int valueInt32, long valueInt64, double valueNumber, string valueString)
    {
      JsonNode parsed=ParseObject(json);
      JsonNode n=parsed["value"];
      CheckInternal(n, isNull, isArray, isObject, isValue, valueType, valueBoolean, valueInt32, valueInt64, valueNumber, valueString);
      if(n.NodeType>=JsonNodeType.Boolean && n.NodeType<=JsonNodeType.String)
        CheckInternal(n.AsString, isNull, isArray, isObject, isValue, JsonNodeType.String, valueBoolean, valueInt32, valueInt64, valueNumber, valueString);
    }

    void Check2(
      JsonNode n,
      bool isNull, bool isArray, bool isObject, bool isValue, JsonNodeType valueType,
      bool valueBoolean, int valueInt32, long valueInt64, double valueNumber, string valueString)
    {
      CheckInternal(n, isNull, isArray, isObject, isValue, valueType, valueBoolean, valueInt32, valueInt64, valueNumber, valueString);
      CheckInternal(n.AsString, isNull, isArray, isObject, isValue, JsonNodeType.String, valueBoolean, valueInt32, valueInt64, valueNumber, valueString);
    }

    static void CheckInternal(
      JsonNode n,
      bool isNull, bool isArray, bool isObject, bool isValue, JsonNodeType valueType,
      bool valueBoolean, int valueInt32, long valueInt64, double valueNumber, string valueString)
    {
      Assert.AreEqual(isArray, n.IsArray);
      Assert.AreEqual(isNull, n.IsNull);
      Assert.AreEqual(isObject, n.IsObject);
      Assert.AreEqual(isValue, n.IsValue);

      Assert.AreEqual(valueType, n.NodeType);
      Assert.AreEqual(n.IsBoolean || n.IsNumber || n.IsString, n.IsValue);
      Assert.AreEqual(valueType==JsonNodeType.Boolean, n.IsBoolean);
      Assert.AreEqual(valueType==JsonNodeType.Number, n.IsNumber);
      Assert.AreEqual(valueType==JsonNodeType.String, n.IsString);

      Assert.AreEqual(valueBoolean, n.AsBoolean);
      Assert.AreEqual(valueInt32, n.AsInt32);
      Assert.AreEqual(valueInt64, n.AsInt64);
      Assert.IsTrue(Math.Abs(n.AsDouble-valueNumber)<=1e-7);
      Assert.AreEqual(valueString, n.AsString);
    }

    static JsonNode ParseObject(string json)
    {
      var parser=new JsonParser();
      parser.AreSingleQuotesAllowed=true;
      parser.AreUnquotedNamesAllowed=true;
      return parser.ParseObject(json);
    }
  }
}