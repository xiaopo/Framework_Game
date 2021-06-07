//ม๚ิพ
public class SingleTemplate<T> where T :new()
{
   
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                (_instance as SingleTemplate<T>).OnInitialize();
            }

            return _instance;
        }
    }

    protected virtual void OnInitialize()
    {

    }

}
