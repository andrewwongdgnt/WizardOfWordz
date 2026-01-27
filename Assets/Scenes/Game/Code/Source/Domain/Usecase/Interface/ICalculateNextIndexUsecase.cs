public interface ICalculateNextIndexUsecase
{
    public int Invoke(
       bool preferRight,
       int currentIndex,
       int max
       );
}