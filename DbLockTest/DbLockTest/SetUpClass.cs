using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DbLockTest
{
    [SetUpFixture]
    [ExcludeFromCodeCoverage]
    class SetUpClass
    {
        internal const string hostname = "https://localhost:44384/";
    }
}
