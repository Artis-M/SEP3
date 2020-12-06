using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;
using Application.SCMediator;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatroomsController : ControllerBase
    {
        private IChatroomService chatroomService;

        public ChatroomsController(IChatroomService chatroomService)
        {
            this.chatroomService = chatroomService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Chatroom>>> GetAllChatrooms()
        {
            try
            {
                //await chatroomService.requestChatrooms();
                IList<Chatroom> chatrooms = await chatroomService.GetAllChatrooms();
                return Ok(chatrooms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Chatroom>> GetChatRoomById([FromRoute] string id)
        {
            try
            {
                Chatroom chatroom = await chatroomService.GetChatroomByID(id);
                return Ok(chatroom);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> AddNewChatroom([FromBody] Chatroom chatroom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await chatroomService.AddNewChatroom(chatroom);
                return Created($"/{chatroom.id}", chatroom);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteChatroom([FromRoute] string id)
        {
            try
            {
                await chatroomService.DeleteChatRoom(id);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<Message>> SendMessage([FromBody] Message message, [FromRoute] string chatRoomId)
        {
            try
            {
                await chatroomService.SendMessage(chatRoomId, message);
                return Ok("message sent: " + message.message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<Message>> AddUser([FromBody] User user, [FromRoute] string chatRoomId)
        {
            try
            {
                await chatroomService.AddUser(chatRoomId, user);
                return Ok("user added: " + user.Username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<Message>> RemoveUser([FromBody] User user, [FromRoute] string chatRoomId)
        {
            try
            {
                await chatroomService.RemoveUser(chatRoomId, user);
                return Ok("user removed: " + user.Username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}