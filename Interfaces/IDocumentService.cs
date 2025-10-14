using Paper_Route.Models.Documents;

public interface IDocumentService
{
    Task<Document?> GetDocumentByIdAsync(int id, CancellationToken ct = default);
    Task<Document> UploadDocumentAsync(string userId, int documentTypeId, IFormFile file, CancellationToken ct = default);
    Task<bool> DeleteDocumentAsync(int id, CancellationToken ct = default);
    Task<bool> VerifyDocumentAsync(int id, string verifiedByUserId, CancellationToken ct = default);

    Task<DocumentTypeEntity?> GetDocumentTypeByIdAsync(int id, CancellationToken ct = default);
    Task<DocumentTypeEntity> CreateDocumentTypeAsync(string name, string? description, bool isRequiredForVerification, CancellationToken ct = default);
    Task<bool> UpdateDocumentTypeAsync(int id, string name, string? description, bool isRequiredForVerification, CancellationToken ct = default);
    Task<bool> DeleteDocumentTypeAsync(int id, CancellationToken ct = default);

    Task<DocumentRequestType?> GetDocumentRequestTypeByIdAsync(int id, CancellationToken ct = default);
    Task<DocumentRequestType> CreateDocumentRequestTypeAsync(string name, string? description, CancellationToken ct = default);
    Task<bool> UpdateDocumentRequestTypeAsync(int id, string name, string? description, bool isActive, CancellationToken ct = default);
    Task<bool> DeleteDocumentRequestTypeAsync(int id, CancellationToken ct = default);

    Task<DocumentCase?> GetDocumentCaseByIdAsync(int id, CancellationToken ct = default);
    Task<DocumentCase> CreateDocumentCaseAsync(string userId, int requestTypeId, string? description, CancellationToken ct = default);
    Task<bool> UpdateDocumentCaseAsync(int id, int requestTypeId, string? description, CancellationToken ct = default);
    Task<bool> DeleteDocumentCaseAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<DocumentCase>> GetAllDocumentCasesForCurrentUserAsync(string userId, CancellationToken ct = default);
    Task<IReadOnlyList<DocumentCase>> GetAllDocumentCasesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DocumentCase>> GetAllDocumentCasesActiveAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Document>> GetAllDocumentsForCurrentUserAsync(string userId, CancellationToken ct = default);
    Task<IReadOnlyList<DocumentTypeEntity>> GetAllDocumentTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<DocumentRequestType>> GetAllDocumentRequestTypesAsync(CancellationToken ct = default);


}
