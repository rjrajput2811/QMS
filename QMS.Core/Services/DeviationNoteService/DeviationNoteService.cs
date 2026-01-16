using QMS.Core.Models;
using QMS.Core.Repositories.DeviationNoteImplementationItemRepository;
using QMS.Core.Repositories.DeviationNoteItemsRepository;
using QMS.Core.Repositories.DeviationNoteRepository;

namespace QMS.Core.Services.DeviationNoteService;

public class DeviationNoteService : IDeviationNoteService
{
    private readonly IDeviationNoteRepository _deviationNoteRepository;
    private readonly IDeviationNoteItemsRepository _deviationNoteItemsRepository;
    private readonly IDeviationNoteImplementationItemRepository _deviationNoteImplementationItemRepository;

    public DeviationNoteService(IDeviationNoteRepository changeNoteRepository,
                                IDeviationNoteItemsRepository changeNoteItemsRepository,
                                IDeviationNoteImplementationItemRepository changeNoteImplementationItemRepository
    )
    {
        _deviationNoteRepository = changeNoteRepository;
        _deviationNoteItemsRepository = changeNoteItemsRepository;
        _deviationNoteImplementationItemRepository = changeNoteImplementationItemRepository;
    }

    public async Task<List<DeviationNoteViewModel>> GetDeviationNotesListAsync()
    {
        return await _deviationNoteRepository.GetDeviationNotesAsync();
    }
    public async Task<DeviationNoteViewModel> GetDeviationNotesDetailsAsync(int Id)
    {
        var changeNoteDetails = await _deviationNoteRepository.GetDeviationNotesDetailsAsync(Id);
        changeNoteDetails.Items = await _deviationNoteItemsRepository.GetDeviationNoteItemssDetailsAsync(changeNoteDetails.Id);
        changeNoteDetails.ImplementationItems = await _deviationNoteImplementationItemRepository.GetDeviationNoteImplementationItemsDetailsAsync(changeNoteDetails.Id);
        return changeNoteDetails;
    }

    public async Task<OperationResult> InsertDeviationNoteAsync(DeviationNoteViewModel model)
    {
        var result = new OperationResult();
        var newDeviationNoteId = await _deviationNoteRepository.InsertDeviationNoteAsync(model);
        if (newDeviationNoteId == 0)
        {
            result.Success = false;
            result.Message = "An unexpected error occured. please refresh the page and try again. If this error persist. Please Contact administrator.";
            return result;
        }

        foreach (var item in model.Items)
        {
            item.DeviationNoteId = newDeviationNoteId;
            result = await _deviationNoteItemsRepository.InsertDeviationNoteItemsAsync(item);
            if (!result.Success) { return result; }
        }

        foreach (var implementationItem in model.ImplementationItems)
        {
            implementationItem.DeviationNoteId = newDeviationNoteId;
            result = await _deviationNoteImplementationItemRepository.InsertDeviationNoteImplementationItemAsync(implementationItem);
            if (!result.Success) { return result; }
        }

        return result;
    }

    public async Task<OperationResult> UpdateDeviationNoteAsync(DeviationNoteViewModel model)
    {
        var result = await _deviationNoteRepository.UpdateDeviationNoteAsync(model);
        if (!result.Success) { return result; }

        result = await _deviationNoteItemsRepository.DeleteDeviationNoteItemsAsync(model.Id);
        if (!result.Success) { return result; }

        result = await _deviationNoteImplementationItemRepository.DeleteDeviationNoteImplementationItemAsync(model.Id);
        if (!result.Success) { return result; }

        foreach (var item in model.Items)
        {
            item.DeviationNoteId = model.Id;
            result = await _deviationNoteItemsRepository.InsertDeviationNoteItemsAsync(item);
            if (!result.Success) { return result; }
        }

        foreach (var implementationItem in model.ImplementationItems)
        {
            implementationItem.DeviationNoteId = model.Id;
            result = await _deviationNoteImplementationItemRepository.InsertDeviationNoteImplementationItemAsync(implementationItem);
            if (!result.Success) { return result; }
        }

        return result;
    }

    public async Task<OperationResult> DeleteDeviationNoteAsync(int Id)
    {
        var result = await _deviationNoteRepository.DeleteDeviationNoteAsync(Id);
        if (!result.Success) { return result; }

        result = await _deviationNoteItemsRepository.DeleteDeviationNoteItemsAsync(Id);
        if (!result.Success) { return result; }

        result = await _deviationNoteImplementationItemRepository.DeleteDeviationNoteImplementationItemAsync(Id);
        return result;
    }
}
