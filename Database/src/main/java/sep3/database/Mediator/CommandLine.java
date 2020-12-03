package sep3.database.Mediator;

import org.bson.types.ObjectId;

public class CommandLine {
    private String Command;
    private String variableUser;
    private String variableChatroom;
    private String SpecificOrder;

    public String getCommand() {
        return Command;
    }

    public void setCommand(String command) {
        Command = command;
    }

    public String getVariableUser() {
        return variableUser;
    }

    public void setVariableUser(String variableUser) {
        this.variableUser = variableUser;
    }

    public String getVariableChatroom() {
        return variableChatroom;
    }

    public void setVariableChatroom(String variableChatroom) {
        this.variableChatroom = variableChatroom;
    }

    public String getSpecificOrder() {
        return SpecificOrder;
    }

    public void setSpecificOrder(String specificOrder) {
        SpecificOrder = specificOrder;
    }
}
