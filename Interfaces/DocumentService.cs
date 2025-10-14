using Microsoft.EntityFrameworkCore;
using Paper_Route.Models.Documents;
using Paper_Route.User;
using System.Threading;
using System.Threading.Tasks;

namespace Paper_Route.Interfaces
{
    public class DocumentService : IDocumentService
    {

        private readonly ApplicationDbContext _context; // database context

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Read collections
        public async Task<IReadOnlyList<Document>> GetAllDocumentsForCurrentUserAsync(string userId, CancellationToken ct = default)
        {
            return await _context.Documents
                                 .AsNoTracking()
                                 .Where(d => d.UserId == userId)
                                 .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DocumentCase>> GetAllDocumentCasesForCurrentUserAsync(string userId, CancellationToken ct = default)
        {
            return await _context.DocumentCases
                                 .AsNoTracking()
                                 .Where(dc => dc.UserId == userId)
                                 .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DocumentCase>> GetAllDocumentCasesAsync(CancellationToken ct = default)
        {
            return await _context.DocumentCases
                                 .AsNoTracking()
                                 .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DocumentCase>> GetAllDocumentCasesActiveAsync(CancellationToken ct = default)
        {
            return await _context.DocumentCases
                                 .AsNoTracking()
                                 .Where(dc => dc.Status == CaseStatus.Pending || dc.Status == CaseStatus.InProgress)
                                 .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DocumentTypeEntity>> GetAllDocumentTypesAsync(CancellationToken ct = default)
        {
            return await _context.DocumentTypes
                                 .AsNoTracking()
                                 .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DocumentRequestType>> GetAllDocumentRequestTypesAsync(CancellationToken ct = default)
        {
            return await _context.DocumentRequestTypes
                                 .AsNoTracking()
                                 .ToListAsync(ct);
        }

        // Single-entity getters (nullable if not found)
        public async Task<Document?> GetDocumentByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Documents
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<DocumentCase?> GetDocumentCaseByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.DocumentCases
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(dc => dc.Id == id, ct);
        }

        public async Task<DocumentRequestType?> GetDocumentRequestTypeByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.DocumentRequestTypes
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<DocumentTypeEntity?> GetDocumentTypeByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.DocumentTypes
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        // Create operations
        public async Task<DocumentTypeEntity> CreateDocumentTypeAsync(string name, string? description, bool isRequiredForVerification, CancellationToken ct = default)
        {
            var entity = new DocumentTypeEntity
            {
                Name = name,
                Description = description,
                IsRequiredForVerification = isRequiredForVerification
            };

            _context.DocumentTypes.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<DocumentRequestType> CreateDocumentRequestTypeAsync(string name, string? description, CancellationToken ct = default)
        {
            var req = new DocumentRequestType
            {
                Name = name,
                Description = description
            };

            _context.DocumentRequestTypes.Add(req);
            await _context.SaveChangesAsync(ct);
            return req;
        }

        public async Task<DocumentCase> CreateDocumentCaseAsync(string userId, int requestTypeId, string? description, CancellationToken ct = default)
        {
            var docCase = new DocumentCase
            {
                UserId = userId,
                RequestTypeId = requestTypeId,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                Status = CaseStatus.Pending
            };

            _context.DocumentCases.Add(docCase);
            await _context.SaveChangesAsync(ct);
            return docCase;
        }

        // Upload document 
        public async Task<Document> UploadDocumentAsync(string userId, int documentTypeId, IFormFile file, CancellationToken ct = default)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var document = new Document
            {
                UserId = userId,
                DocumentTypeId = documentTypeId,
                FileName = file.FileName,
                ContentType = file.ContentType ?? "application/octet-stream",
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow,
                IsVerified = false
            };



            _context.Documents.Add(document);
            await _context.SaveChangesAsync(ct);
            return document;
        }

        // Update operations (return false if not found)
        public async Task<bool> UpdateDocumentTypeAsync(int id, string name, string? description, bool isRequiredForVerification, CancellationToken ct = default)
        {
            var entity = await _context.DocumentTypes.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (entity == null) return false;

            entity.Name = name;
            entity.Description = description;
            entity.IsRequiredForVerification = isRequiredForVerification;
            _context.DocumentTypes.Update(entity);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> UpdateDocumentRequestTypeAsync(int id, string name, string? description, bool isActive, CancellationToken ct = default)
        {
            var req = await _context.DocumentRequestTypes.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (req == null) return false;

            req.Name = name;
            req.Description = description;
            req.IsActive = isActive;
            _context.DocumentRequestTypes.Update(req);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> UpdateDocumentCaseAsync(int id, int requestTypeId, string? description, CancellationToken ct = default)
        {
            var dc = await _context.DocumentCases.FirstOrDefaultAsync(d => d.Id == id, ct);
            if (dc == null) return false;

            dc.RequestTypeId = requestTypeId;
            dc.Description = description;
            _context.DocumentCases.Update(dc);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        // Delete operations
        public async Task<bool> DeleteDocumentAsync(int id, CancellationToken ct = default)
        {
            var doc = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id, ct);
            if (doc == null) return false;

            _context.Documents.Remove(doc);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteDocumentCaseAsync(int id, CancellationToken ct = default)
        {
            var dc = await _context.DocumentCases.FirstOrDefaultAsync(d => d.Id == id, ct);
            if (dc == null) return false;

            _context.DocumentCases.Remove(dc);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteDocumentRequestTypeAsync(int id, CancellationToken ct = default)
        {
            var req = await _context.DocumentRequestTypes.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (req == null) return false;

            _context.DocumentRequestTypes.Remove(req);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteDocumentTypeAsync(int id, CancellationToken ct = default)
        {
            var type = await _context.DocumentTypes.FirstOrDefaultAsync(t => t.Id == id, ct);
            if (type == null) return false;

            _context.DocumentTypes.Remove(type);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        // Verification
        public async Task<bool> VerifyDocumentAsync(int id, string verifiedByUserId, CancellationToken ct = default)
        {
            var doc = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id, ct);
            if (doc == null) return false;

            doc.IsVerified = true;
            doc.VerifiedById = verifiedByUserId;
            doc.VerifiedAt = DateTime.UtcNow;
            _context.Documents.Update(doc);
            await _context.SaveChangesAsync(ct);
            return true;
        }

    }
}
