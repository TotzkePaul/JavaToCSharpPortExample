package mathExpression;


import java.util.List;

/**
 * Created by Paul on 4/18/2015.
 */
public class Node<T>
{
    private T data;
    private List<Node<T>> children;
    public Node(T data){
        this.data = data;
    }

    public T getData() {
        return data;
    }
    public void setData(T data) {
        this.data = data;
    }
    public List<Node<T>> getChildren() {
        return children;
    }
    public Node<T> get(int location){
        if(location>=children.size()){ return null; }
        return children.get(location);
    }
    public boolean isLeaf(){
        return children.size()==0;
    }
    public void setChildren(List<Node<T>> children) {
        this.children = children;
    }
    public void addChild(Node<T> child) { this.children.add(child); }

}
