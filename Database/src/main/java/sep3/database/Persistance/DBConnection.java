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

    /**
     * Constructor that initiate the connection to database
     */
    private DBConnection()
    {
        mongoClient = MongoClients.create(
                "mongodb+srv://admin:admin@sep.b0aui.mongodb.net/ChatSystem?retryWrites=true&w=majority");
        database = mongoClient.getDatabase("ChatSystem");
    }

    /**
     * Get connection as singleton
     * @return connection to database
     */
    public static DBConnection setConnection()
    {
    if(connection == null)
    {
        connection = new DBConnection();
    }
    return connection;
    }

    /**
     * Get database
     * @return MongoDatabase
     */
    public MongoDatabase getDatabase() {
        return database;
    }
}
