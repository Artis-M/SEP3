using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication.SignalR;

namespace Application.Controllers
{
    /// <summary>
    /// Controller class to publish and receive data from Tier 1 regarding chat rooms
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ChatroomsController : ControllerBase
    {
        /// <summary>
        /// Injection of a service through constructor and a Hub of SignalR
        /// </summary>
        private IChatroomService chatroomService;

        private readonly IHubContext<ChatHub> _hubContext;

        public ChatroomsController(IHubContext<ChatHub> hubContext, IChatroomService chatroomService)
        {
            this.chatroomService = chatroomService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Publishing all the chat rooms present
        /// </summary>
        /// <returns>List of all chat rooms</returns>
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

        /// <summary>
        /// Publishing all chat rooms by a given topic
        /// </summary>
        /// <param name="topic">Topic provided via route from Tier 1</param>
        /// <returns>List of chat rooms with the topic given</returns>
        [HttpGet]
        [Route("chatrooms/topic/{topic}")]
        public async Task<ActionResult<List<Chatroom>>> GetChatroomsByTopic([FromRoute] string topic)
        {
            List<Chatroom> chatrooms = await chatroomService.GetChatroomsByTopic(topic);
            foreach (var VARIABLE in chatrooms)
            {
                Console.Out.WriteLine(VARIABLE.name);
            }

            try
            {
                if (chatrooms == null)
                {
                    return NotFound();
                }

                return chatrooms;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Publishing a chat room object by a provided chat room ID
        /// </summary>
        /// <param name="chatRoomId">Id of chat room to be sent, the ID is gotten via route</param>
        /// <returns>Chat room with given ID</returns>
        [HttpGet]
        [Route("{chatRoomId}")]
        public async Task<ActionResult<Chatroom>> GetChatRoomById([FromRoute] string chatRoomId)
        {
            // Console.WriteLine("Getting chat room:" + chatRoomId);
            Chatroom chatroom = await chatroomService.GetChatroomById(chatRoomId);
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

        /// <summary>
        /// Publishing a private chat room between two friends
        /// </summary>
        /// <param name="userId">Friend 1 from route</param>
        /// <param name="userId1">Friend 2 from body</param>
        /// <returns>Private chat room</returns>
        [HttpGet]
        [Route("private/{userId}/{userId1}")]
        public async Task<ActionResult<Chatroom>> GetPrivateChatroom([FromRoute] string userId,
            [FromRoute] string userId1)
        {
            Chatroom chatroom = await chatroomService.GetPrivateChatroom(userId, userId1);
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

        /// <summary>
        /// Publishing a list of chat rooms that have a specific user in them
        /// </summary>
        /// <param name="id">Participant in the chat rooms</param>
        /// <returns>List of chat rooms the user participates in</returns>
        [HttpGet]
        [Route("user/chatrooms/{id}")]
        public async Task<ActionResult<List<Chatroom>>> GetChatRoomsByUserId([FromRoute] string id)
        {
            List<Chatroom> chatrooms = await chatroomService.GetChatroomByUserId(id);

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

        /// <summary>
        /// Adding a new chat room
        /// </summary>
        /// <param name="chatroom">Chat room to be added</param>
        /// <returns>Action result</returns>
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
                await chatroomService.AddNewChatroom(chatroom);
                return Created($"/{chatroom._id}", chatroom);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Deleting a chat room
        /// </summary>
        /// <param name="id">ID of chat room that should be deleted</param>
        /// <returns>Action result</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteChatroom([FromRoute] string id)
        {
            Console.WriteLine($"Deleting room: {id}");
            try
            {
                await chatroomService.DeleteChatRoom(id);
                await _hubContext.Clients.Group(id).SendAsync("ReceiveChatroomUpdate", null);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Sending a new message to a chat room
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="chatRoomId">ID of chat room the message should be sent to</param>
        /// <returns>Action message</returns>
        [HttpPost]
        [Route("sendMessage/{chatRoomId}")]
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

        /// <summary>
        /// User wanting to join a chat room
        /// </summary>
        /// <param name="userID">User's ID that is wanting to join</param>
        /// <param name="chatRoomId">Chat room ID, the chat room the User wants to join to</param>
        /// <returns>Action message</returns>
        [HttpPatch]
        [Route("addUser/{chatRoomId}")]
        public async Task<ActionResult<Message>> JoinChatroom([FromBody] string userID, [FromRoute] string chatRoomId)
        {
            try
            {
                await chatroomService.AddUser(chatRoomId, userID);
                Chatroom chatroom = await chatroomService.GetChatroomById(chatRoomId);
                await _hubContext.Clients.Group(chatRoomId).SendAsync("ReceiveChatroomUpdate", chatroom);
                return Ok("user added: " + userID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Leaving a chatroom
        /// </summary>
        /// <param name="userID">User ID to be leaving gotten via body</param>
        /// <param name="chatRoomId">Chat room ID to be left gotten via route</param>
        /// <returns>Action message</returns>
        [HttpPatch]
        [Route("removeUser/{chatRoomId}")]
        public async Task<ActionResult<Message>> LeaveChatroom([FromBody] string userID, [FromRoute] string chatRoomId)
        {
            try
            {
                // Console.Out.WriteLine(userID+" SFDGFGFSDDFGDBFCF"+chatRoomId);
                await chatroomService.RemoveUser(chatRoomId, userID);
                Chatroom chatroom = await chatroomService.GetChatroomById(chatRoomId);
                await _hubContext.Clients.Group(chatRoomId).SendAsync("ReceiveChatroomUpdate", chatroom);
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