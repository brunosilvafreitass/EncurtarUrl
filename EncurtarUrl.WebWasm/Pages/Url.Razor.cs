using EncurtarUrl.Core.Handlers;
using EncurtarUrl.Core.Models;
using EncurtarUrl.Core.Requests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EncurtarUrl.WebWasm.Components.Pages;

public partial class EncurtadorPage : ComponentBase
{
    public bool IsBusy { get; set; } = false;
    public ShortenedUrlRequest ImputModel { get; set; } = new();
    public Url Url { get; set; } = new();

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IUrlHandler Handler { get; set; } = null!;

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
}