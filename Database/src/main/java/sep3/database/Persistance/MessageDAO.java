package sep3.database.Persistance;


import com.google.gson.Gson;
import com.mongodb.client.*;
import com.mongodb.MongoClientURI;
import org.bson.Document;
import sep3.database.Model.Message;

import java.util.ArrayList;

public class MessageDAO {
    private MongoCollection<Document> collection;
    private MongoClient mongoClient;
    private MongoClientURI uri;
    private MongoDatabase database;

    public MessageDAO(String uri) {
        mongoClient = MongoClients.create(
                "mongodb+srv://admin:admin@test.b0aui.mongodb.net/SEP?retryWrites=true&w=majority");
        database = mongoClient.getDatabase("SEP");
    }

    public void addMessage(Message message)
    {
        collection = database.getCollection("Messages");
        Document document = new Document();
        document.append("message",message.getMessage());
        document.append("authorID",message.getAuthorID());
        collection.insertOne(document);

    }

    public ArrayList<Message> getMessages()
    {
        Gson gson = new Gson();
        collection = database.getCollection("Messages");
        ArrayList<Message> messages = new ArrayList<>();
        MongoCursor<Document> cursor = collection.find().iterator();
        try {
            while (cursor.hasNext()) {
                String json = cursor.next().toJson();
                Message message = gson.fromJson(json,Message.class);
                messages.add(message);
            }
        } finally {
            cursor.close();
        }

        return messages;
    }
}
