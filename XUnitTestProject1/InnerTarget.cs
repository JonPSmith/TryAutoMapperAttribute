using AutoMapper;

namespace XUnitTestProject1
{
    [AutoMap(typeof(InnerSource))]
    public class InnerTarget
    {
        public string Name { get; set; }
    }
}