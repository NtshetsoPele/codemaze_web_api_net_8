namespace Entities.Exceptions;

public sealed class CollectionByIdsBadRequestException() : 
    BadRequestException(Resources.CompanyCollectionMismatchErrorMsg);