namespace Entities.Exceptions;

public sealed class CompanyCollectionBadRequest()
    : BadRequestException(Resources.MissingCompaniesErrorMsg);