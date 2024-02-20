
using Blazored.SessionStorage;

namespace eShop.UI.Storage.Services;
//session storage is removed when the browser is closed

public class SessionStorage(ISessionStorageService sessionStorage) : IStorageService
{
    //we need js library here: Blazored, to store in browser
    public async Task<T> GetAsync<T>(string key) =>
        await sessionStorage.GetItemAsync<T>(key);

    public async Task RemoveAsync(string key) =>
        await sessionStorage.RemoveItemAsync(key);

    public async Task SetAsync<T>(string key, T value) =>
        await sessionStorage.SetItemAsync(key, value);
}
