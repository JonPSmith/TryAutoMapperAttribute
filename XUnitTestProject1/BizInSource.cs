using System.Collections.Generic;
using AutoMapper;

namespace XUnitTestProject1
{
    
    public class BizInSource
    {
        public int MyInt { get; set; }

        public List<InnerSource> SourceList { get; set; }
    }
}