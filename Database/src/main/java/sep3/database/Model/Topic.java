package sep3.database.Model;

import org.bson.types.ObjectId;

public class Topic {
    private String _id;
    private String name;

    public Topic(String _id, String name) {
        this._id = _id;
        this.name = name;
    }

    public Topic(String name) {

        this.name = name;
    }

    public String get_id() {
        return _id;
    }

    public void set_id(String _id) {
        this._id = _id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    @Override
    public String toString() {
        return "Topic{" +
                "_id=" + _id +
                ", name='" + name + '\'' +
                '}';
    }
}
