using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.Features;
using AutoMapper.Mappers;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private class NamedProfile : Profile
        {
            public NamedProfile(string profileName) : base(profileName)
            {
            }
        }

        //taken from https://github.com/AutoMapper/AutoMapper/blob/master/src/AutoMapper/Configuration/MapperConfigurationExpression.cs
        private Profile HandleAutoMapTypes(params Type[] types)
        {
            var autoMapAttributeProfile = new NamedProfile(nameof(AutoMapAttribute));

            foreach (var type in types)
            {
                foreach (var autoMapAttribute in type.GetCustomAttributes<AutoMapAttribute>())
                {
                    var mappingExpression = (MappingExpression)autoMapAttributeProfile.CreateMap(autoMapAttribute.SourceType, type);
                    autoMapAttribute.ApplyConfiguration(mappingExpression);

                    foreach (var memberInfo in type.GetMembers(BindingFlags.Public | BindingFlags.Instance))
                    {
                        foreach (var memberConfigurationProvider in memberInfo.GetCustomAttributes().OfType<IMemberConfigurationProvider>())
                        {
                            mappingExpression.ForMember(memberInfo.Name, cfg => memberConfigurationProvider.ApplyConfiguration(cfg));
                        }
                    }
                }
            }

            return autoMapAttributeProfile;
        }


        [Fact]
        public void Test1()
        {
            //SETUP
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile(HandleAutoMapTypes(typeof(BizInTarget), typeof(InnerTarget))));
            var mapper = new Mapper(configuration);

            var input = new BizInSource()
            {
                MyInt = 123,
                SourceList = new List<InnerSource>
                {
                    new InnerSource {  Name = "Hello"},
                    new InnerSource {  Name = "Goodbye"},
                }
            };

            //ATTEMPT
            var output = mapper.Map<BizInTarget>(input);

            //VERIFY
            Assert.Equal(123, output.MyInt);
            Assert.Equal(new string[] {"Hello", "Goodbye"}, output.TargetList.Select(x => x.Name));
        }
    }
}
