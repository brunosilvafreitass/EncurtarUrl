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
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
    public bool IsBusy { get; set; } = false;
    public ShortenedUrlRequest ImputModel { get; set; } = new();
    public Url Url { get; set; } = new();

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IUrlHandler Handler { get; set; } = null!;

    public async Task EncurtarUrlAsync()
    {
        IsBusy = true;
        Url.ShortenedUrl = string.Empty; // Limpa a URL anterior
        StateHasChanged(); // Atualiza a UI para esconder o resultado antigo

        try
        {
            var result = await Handler.CreateShortUrlAsync(ImputModel);
            if (result.IsSuccess && result.Data != null)
            {
                Url.ShortenedUrl = result.Data.ShortenedUrl;
                Snackbar.Add(result.Message ?? "URL encurtada com sucesso!", Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message ?? "Ocorreu um erro ao encurtar a URL.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
            StateHasChanged(); // Garante que a UI seja atualizada após a operação
        }
    }
    public async Task CopyToClipboard(string text)
    {
        await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        Snackbar.Add("URL copiada para a área de transferência!", Severity.Success);
    }
}