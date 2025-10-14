using Paper_Route.Models.Documents;

namespace Paper_Route.Models
{
    public class DocumentsVaultViewModel
    {
        public IEnumerable<Document> Documents { get; set; }
        public IEnumerable<DocumentTypeEntity> DocumentTypes { get; set; }
        public IEnumerable<DocumentRequestType> RequestTypes { get; set; }
        public IEnumerable<DocumentCase> ActiveRequests { get; set; }
        public IEnumerable<DocumentTypeEntity> RequiredDocuments { get; set; }


        public DocumentsVaultViewModel(
            IEnumerable<Document> _documents,
            IEnumerable<DocumentTypeEntity> _docTypes,
            IEnumerable<DocumentRequestType> _requestTypes,
            IEnumerable<DocumentCase> _activeRequests,
            IEnumerable<DocumentTypeEntity> _requiredDocuments)
        {
            Documents = _documents;
            DocumentTypes = _docTypes;
            RequestTypes = _requestTypes;
            ActiveRequests = _activeRequests;
            RequiredDocuments = _requiredDocuments;
        }
    }
}
