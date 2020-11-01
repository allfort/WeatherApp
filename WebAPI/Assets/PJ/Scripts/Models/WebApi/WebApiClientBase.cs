using Retrofit;

public abstract class WebApiClientBase {

    RetrofitAdapter adapter; // キャッシュ用（キャッシュしないとAdapterをBuildする度にGameObjectが作成されるため）

    protected abstract string Endpoint { get; }

    RetrofitAdapter Adapter {
        get {
            if (adapter == null) {
                adapter = new RetrofitAdapter.Builder ()
                    .SetEndpoint (Endpoint)
                    .EnableLog (true)
                    .Build ();
            }
            return adapter;
        }
    }

    protected T GetService<T> () => Adapter.Create<T> ();
}