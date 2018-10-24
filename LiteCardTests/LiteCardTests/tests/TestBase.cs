using LiteCardTests.app;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCardTests.tests
{
    public class TestBase
    {
        public Application app;

        [SetUp]
        public void start()
        {
            app = new Application();
        }

        [TearDown]
        public void stop()
        {
            app.Quit();
            app = null;
        }

    }



}
