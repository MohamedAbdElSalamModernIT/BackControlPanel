using AutoMapper;

namespace Common.Interfaces.Mapper
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}