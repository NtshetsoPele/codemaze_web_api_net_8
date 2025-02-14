﻿namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    
    public int PageNumber { get; init; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? OrderBy { get; init; }
    public string? Fields { get; init; }
}