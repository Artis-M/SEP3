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
            Chatroom chatroom = await chatroomService.GetChatroomByID(id);
            try
            {
                if (chatroom == null)
                {
                    return NotFound();
                }

                return chatroom;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("user/chatrooms/{id}")]
        public async Task<ActionResult<List<Chatroom>>> GetChatRoomsByUserId([FromRoute] string id)
        {
            Console.WriteLine("Chatrooms requested.");
          
            List<Chatroom> chatrooms = await chatroomService.GetChatroomByUserID(id);

            try
            {
                if (chatrooms == null)
                {
                    return NotFound();
                }

                return Ok(chatrooms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> AddNewChatroom([FromBody] Chatroom chatroom)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine(ModelState.ToString());
                return BadRequest(ModelState);
            }

            try
            {
                Console.WriteLine($"Creating new chatroom {chatroom.name}");
                await chatroomService.AddNewChatroom(chatroom);
                return Created($"/{chatroom._id}", chatroom);
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
        [Route("sendMessage/{chatRoomId}")]
        public async Task<ActionResult<Message>> SendMessage([FromBody] Message message, [FromRoute] string chatRoomId)
        {
            try
            {
                await chatroomService.SendMessage(chatRoomId, message);
                Console.WriteLine("got a message from the client");
                return Ok("message sent: " + message.message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("addUser/{id}")]
        public async Task<ActionResult<Message>> JoinChatroom([FromBody] string userID, [FromRoute] string chatRoomId)
        {
            try
            {
                await chatroomService.AddUser(chatRoomId, userID);
                return Ok("user added: " + userID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("removeUser/{id}")]
        public async Task<ActionResult<Message>> LeaveChatroom([FromBody] string userID, [FromRoute] string chatRoomId)
        {
            try
            {
                await chatroomService.RemoveUser(chatRoomId, userID);
                return Ok("user removed: " + userID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}