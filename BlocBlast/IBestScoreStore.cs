namespace BlockBlast
{
    public interface IBestScoreStore
    {
        int Load();
        void Save(int best);
    }
}