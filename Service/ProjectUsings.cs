﻿global using ClientCompanies = System.Collections.Generic.IEnumerable<Shared.DTOs.Responses.ToClientCompany>;
global using Employees = System.Collections.Generic.IEnumerable<Entities.Models.Employee>;
global using ClientEmployees = System.Collections.Generic.IEnumerable<Shared.DTOs.Responses.ToClientEmployee>;
global using Companies = System.Collections.Generic.IEnumerable<Entities.Models.Company>;
global using CompanyIds = System.Collections.Generic.IEnumerable<System.Guid>;
global using NewCompanies = System.Collections.Generic.IEnumerable<Shared.DTOs.Requests.CompanyCreationRequest>;
global using Contracts;
global using Service.Contracts;
global using Entities.Models;
global using Shared.DTOs.Responses;
global using AutoMapper;
global using Entities.Exceptions;
global using Shared.DTOs.Requests;
global using Shared.ParameterObjects;
global using Shared.DTOs;
global using Shared.RequestFeatures;