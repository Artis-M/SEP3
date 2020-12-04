package sep3.database.Mediator;

import org.bson.types.ObjectId;

public class CommandLine {
    private String Command;
    private String json;

    public String getCommand() {
        return Command;
    }

    public void setCommand(String command) {
        Command = command;
    }

    public String getJson() {
        return json;
    }

    public void setJson(String json) {
        this.json = json;
    }
}
