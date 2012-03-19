namespace Adotic
{
    internal interface IMapper<in TInput, out TOutput>
    {
        TOutput MapFrom(TInput reader);
    }
}