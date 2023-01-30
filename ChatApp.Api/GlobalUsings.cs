﻿global using AutoMapper;
global using ChatApp.Api.Data;
global using ChatApp.Api.Dtos;
global using ChatApp.Api.Dtos.Requests;
global using ChatApp.Api.Dtos.Responses;
global using ChatApp.Api.Models;
global using ChatApp.Api.Utilities.Validation;
global using ChatApp.Shared.Http;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net.Mime;
global using System.Security.Claims;
global using System.Text;
global using static ChatApp.Api.Utilities.Authentication.Auth;
global using static ChatApp.Shared.Utilities.Enums;
global using ChatApp.Api.Utilities.Extentions;
global using System.Collections.ObjectModel;
global using static ChatApp.Api.Dtos.Responses.Handlers;
