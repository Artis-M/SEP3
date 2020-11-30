package sep3.database.Mediator;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import sep3.database.Model.Message;
import sep3.database.Persistance.MongoDBManager;
import sep3.database.Persistance.PersistanceInterface;

import java.util.ArrayList;
@RestController
@RequestMapping("/messages")
public class MessageController {
    @Autowired
   private PersistanceInterface persistanceInterface;
   public MessageController()
   {
       persistanceInterface = new MongoDBManager();
   }

    @GetMapping("")
   public ArrayList<Message> getMessages()
   {


       return persistanceInterface.getAllMessages();
   }

    @PostMapping("/Add")
   public void addMessage(@RequestBody final Message message)
   {
       persistanceInterface.addMessage(message);
   }
}
