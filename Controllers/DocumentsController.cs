using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paper_Route.Interfaces;
using Paper_Route.Models;
using Paper_Route.Models.Documents;
using Paper_Route.User;

public class DocumentsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly DocumentService _docService;

    public DocumentsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
        _docService = new DocumentService(_context);
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return RedirectToAction("Login", "Account");

        var documents = await _docService.GetAllDocumentsForCurrentUserAsync(currentUser.Id);
        var documentTypes = await _docService.GetAllDocumentTypesAsync();
        var requestTypes = await _docService.GetAllDocumentRequestTypesAsync();
        var activeRequests = await _docService.GetAllDocumentCasesActiveAsync();

        // Optional: populate required documents based on some logic
        var requiredDocuments = new List<DocumentTypeEntity>(); // or fetch based on selected request type

        var viewModel = new DocumentsVaultViewModel(
            documents,
            documentTypes,
            requestTypes,
            activeRequests,
            requiredDocuments
        );

        return View(viewModel);
    }
}
