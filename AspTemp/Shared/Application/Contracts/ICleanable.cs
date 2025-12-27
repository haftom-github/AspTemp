namespace AspTemp.Shared.Application.Contracts;

public interface ICleanable<out TClean>
{
    TClean Clean { get; }
}