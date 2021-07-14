using System;
using System.Collections.Generic;
using System.Text;

namespace LWY.ServiceSub
{
    public class ExampleServiceImpl: IExampleService
    {
        public string GetStr()
        {
            return "123";
        }

    }
}
