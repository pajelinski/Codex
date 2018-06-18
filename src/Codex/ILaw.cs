namespace Codex
{
    using Result;

    public interface ILaw
    {
        IResult<Nothing> Evaluate();
    }
}