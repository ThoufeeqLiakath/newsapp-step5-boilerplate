using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Exceptions;
using UserService.Models;
using UserService.Services;
namespace UserService.Controllers
{
    /*
     * As in this assignment, we are working with creating RESTful web service, hence annotate
     * the class with [ApiController] annotation and define the controller level route as per 
     * REST Api standard.
     * 
     */
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /* Implement HttpVerbs and its Functionality asynchronously*/

        /*
         * Define a handler method which will get us the user by a userId.
         * This handler method should return any one of the status messages basis on
         * different situations: 
         * 1. 200(OK) - If the news found successfully.
         * This handler method should map to the URL "/api/user/{userId}" using HTTP GET method
         */
        [HttpGet]
        [Route("/api/user/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                return Ok(await _userService.GetUser(userId));
            }
            catch (UserNotFoundException nnf)
            {
                return NotFound(nnf.Message);
            }
            catch (UserAlreadyExistsException nnf)
            {
                return Conflict(nnf.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /*
        * Define a handler method which will create a specific UserProfile by reading the
        * Serialized object from request body and save the user details in a User table
        * in the database.
        * 
        * Please note that AddUser method should add a userdetails and also handle the exception using 
        * ExceptionHandler.This handler method should return any one of the status messages 
        * basis on different situations: 
        * 1. 201(CREATED) - If the userProfile details created successfully. 
        * 2. 409(CONFLICT) - If the userId conflicts with any existing userId
        * 
        * This handler method should map to the URL "/api/user" using HTTP POST method
        */

        [HttpPost]
        [Route("/api/user")]
        public async Task<IActionResult> Post([FromBody]UserProfile user)
        {
            try
            {
                return Created("", await _userService.AddUser(user));
            }
            catch (UserNotFoundException nnf)
            {
                return NotFound(nnf.Message);
            }
            catch (UserAlreadyExistsException nnf)
            {
                return Conflict(nnf.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /*
        * Define a handler method which will update a specific user by reading the
        * Serialized object from request body and save the updated user details in a
        * user table in database handle exception as well.
        * This handler method should return any one of the status messages basis on different situations: 
        * 1. 200(OK) - If the user updated successfully. 
        * 2. 404(NOT FOUND) - If the user with specified userId is not found. 
        * 
        * This handler method should map to the URL "/api/user/{userId}" using HTTP PUT method.
        */
        [HttpPut]
        [Route("/api/user/{userId}")]
        public async Task<IActionResult> Put(string userId, UserProfile user)
        {
            try
            {
                return Ok(await _userService.UpdateUser(userId, user));
            }
            catch (UserNotFoundException nnf)
            {
                return NotFound(nnf.Message);
            }
            catch (UserAlreadyExistsException nnf)
            {
                return Conflict(nnf.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /*
             * Define a handler method which will delete a specified UserProfile details from a database.
             * This handler method should return any one of the status messages basis on
             * different situations: 
             * 1. 200(OK) - If the userProfile deleted successfully from database. 
             * 2. 404(NOT FOUND) - If the userProfile details with specified userId is not found.
             * This handler method should map to the URL "/api/user/{userId}" using HTTP Delete
             * method" where "id" should be replaced by a valid userId without {}
        */
        [HttpDelete]
        [Route("/api/user/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                return Ok(await _userService.DeleteUser(userId));
            }
            catch (UserNotFoundException nnf)
            {
                return NotFound(nnf.Message);
            }
            catch (UserAlreadyExistsException nnf)
            {
                return Conflict(nnf.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
