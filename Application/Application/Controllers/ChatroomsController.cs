using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;
using Application.SCMediator;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using WebApplication.SignalR;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatroomsController : ControllerBase
    {
        private IChatroomService chatroomService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatroomsController(IHubContext<ChatHub> hubContext, IChatroomService chatroomService)
        {
            this.chatroomService = chatroomService;
            _hubContext = hubContext;
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
        [Route("chatrooms/topic/{topic}")]
        public async Task<ActionResult<List<Chatroom>>> GetChatroomsByTopic([FromRoute] string topic)
        {
            List<Chatroom> chatrooms = await chatroomService.getChatroomsByTopic(topic);
            Console.Out.WriteLine("COntroller");
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

        [HttpGet]
        [Route("{chatRoomId}")]
        public async Task<ActionResult<Chatroom>> GetChatRoomById([FromRoute] string chatRoomId)
        {
           // Console.WriteLine("Getting chat room:" + chatRoomId);
            Chatroom chatroom = await chatroomService.GetChatroomByID(chatRoomId);
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
            //Console.WriteLine("Chatrooms requested.");
          
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
               // Console.WriteLine($"Creating new chatroom {chatroom.name}");
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
                await _hubContext.Clients.Group(id).SendAsync("ReceiveChatroomUpdate", null);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("sendMessage/{chatRoomId}")]
        public async Task<ActionResult<Message>> SendMessage([FromBody] Message message, [FromRoute] string chatRoomId)
        {
            try
            {
               // Console.Out.WriteLine(message.message);
                await chatroomService.SendMessage(chatRoomId, message);
                //Console.WriteLine("got a message from the client");
                return Ok("message sent: " + message.message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("addUser/{chatRoomId}")]
        public async Task<ActionResult<Message>> JoinChatroom([FromBody] string userID, [FromRoute] string chatRoomId)
        {
            try
            {
                Console.WriteLine($"{userID}, {chatRoomId}");
                await chatroomService.AddUser(chatRoomId, userID);
                Chatroom chatroom = await chatroomService.GetChatroomByID(chatRoomId);
                await _hubContext.Clients.Group(chatRoomId).SendAsync("ReceiveChatroomUpdate", chatroom);
                return Ok("user added: " + userID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("removeUser/{chatRoomId}")]
        public async Task<ActionResult<Message>> LeaveChatroom([FromBody] string userID, [FromRoute] string chatRoomId)
        {
            try
            {
               // Console.Out.WriteLine(userID+" SFDGFGFSDDFGDBFCF"+chatRoomId);
                await chatroomService.RemoveUser(chatRoomId, userID);
                Chatroom chatroom = await chatroomService.GetChatroomByID(chatRoomId);
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