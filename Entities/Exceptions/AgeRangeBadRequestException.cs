namespace Entities.Exceptions;

public sealed class AgeRangeBadRequestException() : 
    BadRequestException("Max age can't be less than min age.");