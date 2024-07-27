global using Microsoft.AspNetCore.Cors.Infrastructure; //CORS
global using CompanyEmployees.Extensions; //ServiceExtensions
global using Microsoft.AspNetCore.HttpOverrides; //ForwardedHeadersOptions
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Repository;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Contracts;
global using NLog;
global using LoggerService;
global using Service.Contracts;
global using Service;
global using CompanyEmployees.StartupAncillaries;
global using NLog.Web;
global using System.Diagnostics;
global using System.Reflection;
global using CompanyEmployees;
global using Presentation;
global using Mapping;
global using Microsoft.AspNetCore.Diagnostics;
global using Entities.ErrorModel;
global using CompanyEmployees.ExceptionHandler;
global using Entities.Exceptions;
global using System.Text;
global using Microsoft.AspNetCore.Mvc.Formatters;
global using Microsoft.Net.Http.Headers;
global using Shared.DTOs.Responses;
global using CompanyEmployees.Formatters;
global using Microsoft.AspNetCore.Mvc;
global using Presentation.ActionFilters;
global using Service.DataShaper;
global using Microsoft.Extensions.Options;
global using Asp.Versioning;
global using Presentation.Controllers;