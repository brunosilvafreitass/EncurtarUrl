using System.Text.RegularExpressions;
using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace EncurtarUrl.WebWasm.Components.Pages;

public partial class EncurtadorPage : ComponentBase
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;
    public bool IsBusy { get; set; } = false;
    public ShortenedUrlRequest ImputModel { get; set; } = new();
    public Url Url { get; set; } = new();

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IUrlHandler Handler { get; set; } = null!;
    public bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        // Regex melhorada para validar URLs
        return Regex.IsMatch(url,
            @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
            RegexOptions.IgnoreCase);
    }
    public async Task EncurtarUrlAsync()
    {
        IsBusy = true;
        try
        {
            var result = await Handler.CreateShortUrlAsync(ImputModel);
            Url.ShortenedUrl = result.Data!.ShortenedUrl;
            StateHasChanged();
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message!, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message!, Severity.Error);
            }
        }
        catch (Exception ex)
        {

            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
    public async Task CopyToClipboard(string text)
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        Snackbar.Add("URL copiada para a área de transferência!", Severity.Success);
    }
}
