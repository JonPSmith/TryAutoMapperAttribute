using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace XUnitTestProject1
{
    [AutoMap(typeof(BizInSource))]
    public class BizInTarget
    {
        public int MyInt { get; set; }

        [SourceMember(nameof(BizInSource.SourceList))]
        public List<InnerTarget> TargetList { get; set; }
    }
}