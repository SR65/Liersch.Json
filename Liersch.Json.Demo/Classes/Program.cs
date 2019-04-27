﻿//----------------------------------------------------------------------------
//
// Copyright © 2013-2019 Dipl.-Ing. (BA) Steffen Liersch
// All rights reserved.
//
// Steffen Liersch
// Robert-Schumann-Straße 1
// 08289 Schneeberg
// Germany
//
// Phone: +49-3772-38 28 08
// E-Mail: S.Liersch@gmx.de
//
//----------------------------------------------------------------------------

using System;

namespace Liersch.Json
{
  //--------------------------------------------------------------------------

  static class Program
  {
    static void Main()
    {
      try
      {
        new UnitTest1().Run();
        new UnitTest2().Run();

#if !NETMF
        new UnitTest3().Run();
#endif

        Examples.RunExample1();
        Examples.RunExample2();
        Examples.RunExample3();
        Examples.RunExample4();

#if !NETMF
        ExamplesWithReflection.Run();
#endif
      }
      catch(Exception e)
      {
        Console.WriteLine(e.ToString());
      }

      Console.WriteLine("[Press any key!]");
      Console.ReadKey(true);
    }
  }

  //--------------------------------------------------------------------------
}