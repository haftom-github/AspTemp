namespace AspTemp.Shared;

public interface ICleanable<out TClean>
{
    TClean Clean { get; }
}