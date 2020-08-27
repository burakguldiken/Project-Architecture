﻿using Business.Abstract;
using Entities.Dto;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);

            if(!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var token = _authService.CreateAccessToken(userToLogin.Data);

            if(token.Success)
            {
                return Ok(token.Data);
            }

            return BadRequest(token.Message);

        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);

            if(!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);

            if(result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(userExists.Message);
        }
    }
}
