﻿namespace Entities.Exceptions;

public sealed class IdParametersBadRequestException() : 
    BadRequestException(Resources.EmptyIdCollectionErrorMsg);