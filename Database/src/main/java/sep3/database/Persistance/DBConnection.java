package sep3.database.Persistance;

import com.mongodb.MongoClientURI;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoClients;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;
import org.bson.Document;

public class DBConnection {
    private static DBConnection connection;
    private MongoClient mongoClient;
    private MongoClientURI uri;
    private MongoDatabase database;

    private DBConnection()
    {
        mongoClient = MongoClients.create(
                "mongodb+srv://admin:admin@test.b0aui.mongodb.net/SEP?retryWrites=true&w=majority");
        database = mongoClient.getDatabase("SEP");
    }
    public static DBConnection setConnection()
    {
    if(connection == null)
    {
        connection = new DBConnection();
    }
    return connection;
    }

    public MongoDatabase getDatabase() {
        return database;
    }
}
