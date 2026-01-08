using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Models;
using QMS.Core.Repositories.ChangeNoteImplementationItemRepository;
using QMS.Core.Repositories.ChangeNoteItemsRepository;
using QMS.Core.Repositories.ChangeNoteRepository;

namespace QMS.Core.Services.ChangeNoteService;

public class ChangeNoteService : IChangeNoteService
{
    private readonly QMSDbContext _dbContext;
    private readonly IChangeNoteRepository _changeNoteRepository;
    private readonly IChangeNoteItemsRepository _changeNoteItemsRepository;
    private readonly IChangeNoteImplementationItemRepository _changeNoteImplementationItemRepository;

    public ChangeNoteService(QMSDbContext dbContext,
                             IChangeNoteRepository changeNoteRepository,
                             IChangeNoteItemsRepository changeNoteItemsRepository,
                             IChangeNoteImplementationItemRepository changeNoteImplementationItemRepository
    )
    {
        _dbContext = dbContext;
        _changeNoteRepository = changeNoteRepository;
        _changeNoteItemsRepository = changeNoteItemsRepository;
        _changeNoteImplementationItemRepository = changeNoteImplementationItemRepository;
    }

    public async Task<List<ChangeNoteViewModel>> GetChangeNotesListAsync()
    {
        return await _changeNoteRepository.GetChangeNotesAsync();
    }
    public async Task<ChangeNoteViewModel> GetChangeNotesDetailsAsync(int Id)
    {
        var changeNoteDetails = await _changeNoteRepository.GetChangeNotesDetailsAsync(Id);
        changeNoteDetails.Items = await _changeNoteItemsRepository.GetChangeNoteItemssDetailsAsync(changeNoteDetails.Id);
        changeNoteDetails.ImplementationItems = await _changeNoteImplementationItemRepository.GetChangeNoteImplementationItemsDetailsAsync(changeNoteDetails.Id);
        return changeNoteDetails;
    }

    public async Task<OperationResult> InsertChangeNoteAsync(ChangeNoteViewModel model)
    {
        var result = new OperationResult();
        var newChangeNoteId = await _changeNoteRepository.InsertChangeNoteAsync(model);
        if(newChangeNoteId == 0)
        {
            result.Success = false;
            result.Message = "An unexpected error occured. please refresh the page and try again. If this error persist. Please Contact administrator.";
            return result;
        }

        foreach(var item in model.Items)
        {
            item.ChangeNoteId = newChangeNoteId;
            result = await _changeNoteItemsRepository.InsertChangeNoteItemsAsync(item);
            if(!result.Success) { return result; }
        }

        foreach (var implementationItem in model.ImplementationItems)
        {
            implementationItem.ChangeNoteId = newChangeNoteId;
            result = await _changeNoteImplementationItemRepository.InsertChangeNoteImplementationItemAsync(implementationItem);
            if (!result.Success) { return result; }
        }

        return result;
    }

    public async Task<OperationResult> UpdateChangeNoteAsync(ChangeNoteViewModel model)
    {
        var result = await _changeNoteRepository.UpdateChangeNoteAsync(model);
        if (!result.Success) { return result; }

        result = await _changeNoteItemsRepository.DeleteChangeNoteItemsAsync(model.Id);
        if (!result.Success) { return result; }

        result = await _changeNoteImplementationItemRepository.DeleteChangeNoteImplementationItemAsync(model.Id);
        if (!result.Success) { return result; }

        foreach (var item in model.Items)
        {
            item.ChangeNoteId = model.Id;
            result = await _changeNoteItemsRepository.InsertChangeNoteItemsAsync(item);
            if (!result.Success) { return result; }
        }

        foreach (var implementationItem in model.ImplementationItems)
        {
            implementationItem.ChangeNoteId = model.Id;
            result = await _changeNoteImplementationItemRepository.InsertChangeNoteImplementationItemAsync(implementationItem);
            if (!result.Success) { return result; }
        }

        return result;
    }

    public async Task<OperationResult> DeleteChangeNoteAsync(int Id)
    {
        var result = await _changeNoteRepository.DeleteChangeNoteAsync(Id);
        if (!result.Success) { return result; }

        result = await _changeNoteItemsRepository.DeleteChangeNoteItemsAsync(Id);
        if (!result.Success) { return result; }

        result = await _changeNoteImplementationItemRepository.DeleteChangeNoteImplementationItemAsync(Id);
        return result;
    }
}
